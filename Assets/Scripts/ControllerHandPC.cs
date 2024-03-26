using UnityEngine;
using ZCore;

public class ControllerHandPC : MonoBehaviour
{
    // config
    public int interval = 1;
    public Vector2 throwBoost = new Vector2(5, 10);
    public float throwDirectionCoefficient = 10;

    // fixed links
    public PCMetadata meta;
    public MovementFollowTransform neutral;
    public MovementRadial radial;
    public TriggerVolume triggerGrab;
    public Transform anchorHold;

    // dynamic links
    private ActorBall ball;

    // state
    public State state;

    public void Start()
    {
        meta.onInitialized += () =>
        {
            meta.commandEmitter.onPCCommand += HandleCommand;
            //meta.controllerHand = this;

            UIDebug.Instance.Register("handMove", () => { return meta.commandEmitter.data.handMove; });
            UIDebug.Instance.Register("handMoveSqrMag", () => { return meta.commandEmitter.data.handMove.sqrMagnitude; });
        };

        triggerGrab.onTraitFound += HandleTraitFound;
    }

    public void Update()
    {
        if (!meta.commandEmitter.isCursor && meta.commandEmitter.data.handMove.sqrMagnitude < Mathf.Epsilon)
        {
            neutral.gameObject.SetActive(true);

            radial.gameObject.SetActive(false);
            radial.ResetState();
        }
    }

    public void HandleCommand(PCInputArgs args)
    {
        switch (args.type)
        {
            case CoreActionMap.Player.Action.MOVE_HAND:
                if (state != State.STICK && args.handMove.sqrMagnitude > 0)
                {
                    neutral.gameObject.SetActive(false);

                    radial.gameObject.SetActive(true);
                    radial.Move(args.handMove);
                }
                break;

            case CoreActionMap.Player.Action.GRIP:
                var hasBall = triggerGrab.triggeredTraits.HasFlag(Trait.BALL);

                if (state != State.STICK && args.grip && ball && hasBall)
                {
                    if (state == State.NONE)
                    {
                        neutral.type = MovementFollowTransform.FollowType.SNAP;
                        neutral.anchor = neutral.initialAnchor;
                        neutral.offset = neutral.initialOffset;
                        ball.Grab(anchorHold);
                        state = State.GRIP;
                    }
                    else if (state == State.GRIP)
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

                    neutral.type = MovementFollowTransform.FollowType.SNAP;
                    neutral.anchor = ball.transform;
                    neutral.offset.y += 0.3f;
                    neutral.gameObject.SetActive(true);
                    radial.gameObject.SetActive(false);
                    // todo: 
                    ball.Throw(handVelocity);
                    state = State.STICK;
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
        if (trait.value.HasFlag(Trait.PLAYER))
        {
            // todo: create 'toast'/popup type UI to display message
            Debug.Log("overlap player");

        }
    }

    // todo: streamline state toggling
    public void Toggle(State s)
    {
        switch (s)
        {
            case State.GRIP:
                break;
            case State.STICK:
                break;
        }
    }

    // -- Class definitions
    public enum State
    {
        NONE = 0,
        GRIP = 1,
        STICK = 2
    }
}
