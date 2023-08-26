using System;
using UnityEngine;

public class MotorHandPC : MonoBehaviour
{
    public TriggerVolume grabTrigger;
    public CoreBody body;
    public MotorPlatformPC motor;
    public Transform holdAnchor;
    public MovementRadial movementHeldPickup;
    public PCMetadata metadata;

    public float interactionBlockDelay = 0.5f;
    public State state;

    public void Awake()
    {
        metadata.onInitialized += () =>
        {
            metadata.commandEmitter.onPCCommand += HandleCommand;
        };
    }

    public void HandleCommand(PCInputArgs args)
    {
        var ball = SceneRefs.instance.ball;

        switch (args.type)
        {
            case CoreActionMap.Player.MOVE_HAND:
                movementHeldPickup.Move(args.vVec2);
                break;
            case CoreActionMap.Player.THROW:
                if (state == State.GRIP)
                {
                    ApplyThrow(args.vVec2);

                    state &= ~State.GRIP;
                    state |= State.BLOCKED;

                    StartCoroutine(CoreUtilities.TaskDelayed(interactionBlockDelay, () =>
                    {
                        state &= ~State.BLOCKED;
                        ball.ThrowReset();
                    }));
                }
                break;
            case CoreActionMap.Player.GRIP:
                if (grabTrigger.isTriggered)
                {
                    if (ball != null && state == State.NONE)
                    {
                        ball.Grab();

                        state |= State.BLOCKED;
                        state |= State.GRIP;

                        StartCoroutine(CoreUtilities.TaskDelayed(interactionBlockDelay, () =>
                        {
                            state &= ~State.BLOCKED;
                        }));
                    }
                }

                if (ball && state == State.GRIP)
                {
                    ball.ActivateRelease();

                    state |= State.BLOCKED;
                    state &= ~State.GRIP;

                    StartCoroutine(CoreUtilities.TaskDelayed(interactionBlockDelay, () =>
                    {
                        state &= ~State.BLOCKED;
                        ball.ActivatePickup();
                    }));
                }
                break;
        }
    }

    public void ApplyThrow(Vector2 inputDirection)
    {
        var ball = SceneRefs.instance.ball;
        var dotUpRight = Vector2.Dot(inputDirection, Vector2.right + Vector2.up);
        var dotUpLeft = Vector2.Dot(inputDirection, Vector2.left + Vector2.up);
        var dotRight = Vector2.Dot(inputDirection, Vector2.right);

        if (dotUpRight > CoreConstants.THRESHOLD_DOT_INPUT || dotUpLeft > CoreConstants.THRESHOLD_DOT_INPUT)
        {
            ball.Shoot(body.velocity,
                       inputDirection,
                       dotUpRight > CoreConstants.THRESHOLD_DOT_INPUT || dotUpLeft > CoreConstants.THRESHOLD_DOT_INPUT,
                       Mathf.Abs(motor.body.velocity.y) < CoreConstants.DEADZONE_VELOCITY);
        }

        if (Mathf.Abs(dotRight) > CoreConstants.THRESHOLD_DOT_INPUT)
        {
            // direct, straight throw
            //modVelocity.x = 1;
            ball.ThrowAttack(body.velocity, inputDirection);
            //state = State.STRAIGHT;
        }

        if (Vector2.Dot(Vector2.down, inputDirection) > 0.84f)
        {
            ball.Dribble(motor.body.velocity);
        }
    }

    // -- Class definitions
    [Flags]
    public enum State
    {
        NONE = 0,
        GRIP = 1 << 0,
        BLOCKED = 1 << 1
    }
}
