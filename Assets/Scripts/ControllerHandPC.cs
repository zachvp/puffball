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
    public Transform anchorHold;

    // dynamic links
    private ActorBall ball;

    // state
    public State state;

    public void Awake()
    {
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
                var hasBall = triggerGrab.triggeredTraits.HasFlag(Trait.BALL);

                if (args.grip && ball && hasBall)
                {
                    if (state == State.NONE)
                    {
                        ball.Grab(anchorHold);
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
                if (args.handAction
                    && ball
                    && triggerGrab.triggeredTraits.HasFlag(Trait.BALL)
                    && state == State.GRIP)
                {
                    var handVelocity = args.handMove * throwDirectionCoefficient * throwBoost;

                    ball.Throw(handVelocity);
                    state = State.NONE;
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
