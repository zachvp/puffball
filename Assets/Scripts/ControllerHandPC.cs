using System.Collections;
using UnityEngine;

public class ControllerHandPC : MonoBehaviour
{
    // config
    public int interval = 1;
    public Vector2 throwBoost = new Vector2(5, 10);
    public float throwStrength = 16;

    // fixed links
    public PCMetadata meta;
    public GameObject neutral;
    public MovementRadial radial;
    public TriggerVolume triggerGrab;
    public Collider2D colliderBody;

    // dynamic links
    private ActorBall ball;

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
                var dot = Vector2.Dot(args.handMove.normalized, Vector2.up);

                SceneRefs.instance.uiDebug.text =
                    $"handMove.nrml: {args.handMove.normalized}\n" +
                    $"dot: {dot}\n" +
                    $"angle: {Mathf.Acos(dot) * Mathf.Rad2Deg}º";


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

                    indexHandSwingStart = -1;
                }
                break;

            case CoreActionMap.Player.Action.HAND_ACTION:
                if (args.vBool && ball && triggerGrab.triggeredTraits.HasFlag(Trait.BALL))
                {
                    //if (indexHandSwingStart < 0)
                    //{
                    //    Debug.LogWarning("start index is invalid value");
                    //    break;
                    //}

                    // todo: determine start and end point of hand gesture


                    var buffer = meta.commandEmitter.liveBuffer;
                    var handVelocity = args.handMove;
                    handVelocity.x *= Mathf.Lerp(1, throwBoost.x, handVelocity.x / 1);
                    handVelocity.y *= Mathf.Lerp(1, throwBoost.y, handVelocity.y / 1);
                    

                    // dot references in relation to clock:
                    // 12 noon: >0.9f
                    // 1 : < 0.9, > .71


                    /*
                     * float angleRad = Mathf.Acos(dotProduct);
                     * cos(angleRad) = dotProduct
                     * angleRad = arccos(lhs.x*rhs.x + lhs.y*rhs.y)
                     * cos(angleRad) = lhs.x*rhs.x + lhs.y*rhs.y
                     * 
                    */

                    ball.Throw(handVelocity);

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

                    indexHandSwingStart = -1;
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
            ball = trait.GetComponentInChildren<ActorBall>();
        }
    }

    // -- Class definitions
    public enum State
    {
        NONE = 0,
        GRIP = 1 << 0,
    }
}
