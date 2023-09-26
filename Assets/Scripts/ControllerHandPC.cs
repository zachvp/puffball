using System.Collections;
using UnityEngine;

public class ControllerHandPC : MonoBehaviour
{
    // config
    public int interval = 1;
    public Vector2 throwBoost = new Vector2(5, 10);
    public float throwDirectionCoefficient = 10;

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
        if (!meta.commandEmitter.isCursor && meta.commandEmitter.data.handMove.sqrMagnitude < CoreConstants.DEADZONE_FLOAT_2)
        {
            neutral.SetActive(true);

            radial.gameObject.SetActive(false);
            radial.ResetState();
            indexHandSwingStart = -1;
        }

        //SceneRefs.instance.uiDebug.text =
        //    $"handMove: {meta.commandEmitter.data.handMove}\n" +
        //    $"handMove sqMag: {meta.commandEmitter.data.handMove.sqrMagnitude}";
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

                    // todo: determine start and end point of hand gesture
                    if (indexHandSwingStart < 0 && meta.commandEmitter.data.handMove.magnitude > 0.9f
                        && state == State.GRIP)
                    {
                        Debug.DrawRay(transform.position, meta.commandEmitter.data.handMove * 2, Color.red, 3);
                        Debug.Log($"start swing");
                        indexHandSwingStart = meta.commandEmitter.liveBuffer.index;
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
                    var buffer = meta.commandEmitter.liveBuffer;
                    var handVelocity = args.handMove * throwDirectionCoefficient;
                    var total = handVelocity * throwBoost;

                    ball.Throw(total);

                    // todo: debug
                    if (TestDefault.Instance.isDebug)
                    {
                        tracker.Update(handVelocity.magnitude);
                        var text =
                            $"vel: {handVelocity}\n" +
                            $"total: {total}\n" +
                            $"start idx: {indexHandSwingStart}\n" +
                            $"curr idx: {buffer.index}";

                        SceneRefs.instance.uiDebug.text = text;
                    }

                    indexHandSwingStart = -1;
                }
                break;
        }
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
        GRIP = 1,
    }
}
