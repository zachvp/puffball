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

    // debug
    public TrackMinMax tracker;

    public void Awake()
    {
        tracker = new TrackMinMax();

        meta.onInitialized += () =>
        {
            meta.commandEmitter.onPCCommand += HandleCommand;

            UIDebug.Instance.Register("handMove", () => { return meta.commandEmitter.data.handMove; });
            UIDebug.Instance.Register("handMoveSqrMag", () => { return meta.commandEmitter.data.handMove.sqrMagnitude; });
        };

        triggerGrab.onTraitFound += HandleTraitFound;
    }

    public void Update()
    {
        if (!meta.commandEmitter.isCursor && meta.commandEmitter.data.handMove.sqrMagnitude < Mathf.Epsilon)
        {
            neutral.SetActive(true);

            radial.gameObject.SetActive(false);
            radial.ResetState();
        }
    }

    public void HandleCommand(PCInputArgs args)
    {
        switch (args.type)
        {
            case CoreActionMap.Player.Action.MOVE_HAND:
                if (args.handMove.sqrMagnitude > 0)
                {
                    neutral.SetActive(false);

                    radial.gameObject.SetActive(true);
                    radial.Move(args.handMove);
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
                    var handVelocity = args.handMove * throwDirectionCoefficient;
                    var total = handVelocity * throwBoost;

                    ball.Throw(total);

                    // todo: debug
                    if (TestDefault.Instance.isDebug)
                    {
                        tracker.Update(handVelocity.magnitude);
                        //var text =
                        //    $"vel: {handVelocity}\n" +
                        //    $"total: {total}\n" +
                        //    $"start idx: {indexHandSwingStart}\n" +
                        //    $"curr idx: {buffer.index}";

                        //SceneRefs.instance.uiDebug.text = text;
                    }
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
