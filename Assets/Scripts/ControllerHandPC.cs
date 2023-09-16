using System.Collections;
using UnityEngine;

public class ControllerHandPC : MonoBehaviour
{
    // config
    public int interval = 1;
    public Vector2 throwBoost = Vector2.one;

    // fixed links
    public PCMetadata meta;
    public GameObject neutral;
    public MovementRadial radial;
    public TriggerVolume triggerGrab;
    public Collider2D colliderBody;

    // dynamic links
    private Ball ball;

    // state
    public State state;
    public int indexHandSwingStart = -1;

    // debug
    public TrackMinMax tracker;

    public void Awake()
    {
        tracker = new TrackMinMax();

        meta.onInitialized += () =>
        {
            meta.commandEmitter.onPCCommand += HandleCommand;
        };

        triggerGrab.onTraitFound += HandleTraitFound;
    }

    public void Update()
    {
        var moveHand = meta.commandEmitter.playerInput.actions[CoreActionMap.Player.MOVE_HAND].ReadValue<Vector2>();
        if (!meta.commandEmitter.isCursor && moveHand.sqrMagnitude < CoreConstants.DEADZONE_FLOAT_2)
        {
            neutral.SetActive(true);

            radial.gameObject.SetActive(false);
            radial.ResetState();

            StartCoroutine(CoreUtilities.TaskDelayed(0.5f, () =>
            {
                indexHandSwingStart = -1;
            }));
        }
    }

    public void HandleCommand(PCInputArgs args)
    {
        switch (args.type)
        {
            case CoreActionMap.Player.Action.MOVE_HAND:
                
                if (args.handMove.sqrMagnitude > CoreConstants.DEADZONE_FLOAT_2)
                {
                    neutral.SetActive(false);

                    radial.gameObject.SetActive(true);
                    radial.Move(args.handMove);

                    if (indexHandSwingStart < 0)
                    {
                        indexHandSwingStart = meta.commandEmitter.liveBuffer.index;
                        Debug.Log("start hand swing");
                    }
                }
                break;

            case CoreActionMap.Player.Action.GRIP:
                if (args.vBool && ball && triggerGrab.triggeredTraits.HasFlag(Trait.BALL))
                {
                    if (state == State.NONE)
                    {
                        ball.Grab(radial.target);
                        state = State.GRIP;
                    }
                    else
                    {
                        ball.Drop();
                        state = State.NONE;
                    }
                }
                break;

            case CoreActionMap.Player.Action.HAND_ACTION:
                if (args.vBool && ball && triggerGrab.triggeredTraits.HasFlag(Trait.BALL))
                {
                    if (indexHandSwingStart < 0)
                    {
                        Debug.LogWarning("start index is invalid value");
                        break;
                    }

                    var handVelocity = args.handMove * throwBoost;
                    var buffer = meta.commandEmitter.liveBuffer;
                    SceneRefs.instance.uiDebug.text = "";

                    var i = indexHandSwingStart;
                    var diff = 0;
                    var strength = 0f;
                    SceneRefs.instance.uiDebug.text = "";
                    while (i != buffer.index)
                    {
                        //handVelocity += CoreUtilities.Abs(buffer.data[i].handMove - buffer.data[buffer.Previous(i)].handMove);
                        //strength += CoreUtilities.Abs(buffer.data[i].handMove);
                        //handVelocity += buffer.data[i].handMove - buffer.data[buffer.Previous(i)].handMove;
                        SceneRefs.instance.uiDebug.text += $"{buffer.data[i].handMove}\n";
                        i = buffer.Next(i);
                        diff++;
                        
                        //handVelocity += buffer[i].handMove + buffer[i - interval].handMove;
                    }

                    ////handVelocity *= 1 + (buffer.data.Length - diff) / buffer.data.Length;
                    handVelocity *= strength;


                    // todo: debug
                    if (TestDefault.Instance.isDebug)
                    {
                        tracker.Update(handVelocity.magnitude);
                        var text =
                            $"{handVelocity}\n" +
                            $"cur: {tracker.current}\n" +
                            $"min: {tracker.min}\n" +
                            $"max: {tracker.max}\n";

                        //SceneRefs.instance.uiDebug.text = text;
                    }

                    ball.Throw(handVelocity);
                }
                break;
        }
    }

    public IEnumerator CheckNeutralMovement(float threshold)
    {
        var delta = 0f;
        var buffer = new PCInputArgs[meta.commandEmitter.liveBuffer.data.Length];
        meta.commandEmitter.liveBuffer.data.CopyTo(buffer, 0);
        while (true)
        {
            //foreach (var d in meta.commandEmitter.liveBuffer.buffer)
            for (var i = 0; i < 8; i++)
            {
                delta += buffer[i].handMove.sqrMagnitude;
            }

            if (delta < threshold)
            {
                yield return new WaitForSeconds(meta.commandEmitter.liveBuffer.interval);
            }
            else
            {
                break;
            }
        }

        neutral.SetActive(true);

        radial.gameObject.SetActive(false);
        radial.ResetState();
        Debug.Log("reset hand to neutral");
    }

    public void HandleTraitFound(ContainerTrait trait)
    {
        if (trait.value.HasFlag(Trait.BALL))
        {
            ball = trait.GetComponentInChildren<Ball>();
        }
    }

    // -- Class definitions
    public enum State
    {
        NONE = 0,
        GRIP = 1 << 0,
    }
}
