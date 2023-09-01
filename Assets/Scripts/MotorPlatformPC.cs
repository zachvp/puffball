using UnityEngine;
using System;

public class MotorPlatformPC : MonoBehaviour
{
    // -- read vars
    public CoreBody body;
    public ActorStatePlatform state;
    public PCMetadata metadata;

    // -- config vars
    public float jumpStrength = 10;
    public float groundMoveSpeed = 10;
    public float airMoveSpeed = 5;
    public float maxSpeedX = 15;
    public float wallJumpDelay = 0.1f;
    public Vector2 wallJumpSpeed = new Vector2Int(20, 10);

    // -- write vars
    public float adjustedVelocityX;

    public void Awake()
    {
        metadata.onInitialized += () =>
        {
            metadata.commandEmitter.onPCCommand += HandleCommand;
        };
    }

    public void HandleCommand(PCInputArgs args)
    {
        // update state according to input.
        switch (args.type)
        {
            case CoreActionMap.Player.JUMP:
                // ground jump
                if (args.vBool && state.trigger.down.isTriggered)
                {
                    state.platformState |= PlatformState.JUMP;
                    state.platformState &= ~PlatformState.WALL_JUMPING;
                }

                // wall jump 
                else if (!state.platformState.HasFlag(PlatformState.WALL_JUMPING) &&
                        IsWallJumpState())
                {
                    state.platformState |= PlatformState.WALL_JUMP;
                    state.platformState &= ~PlatformState.MOVE;
                }
                break;

            case CoreActionMap.Player.MOVE:
                state.inputMove = args.vFloat;

                if (Mathf.Abs(state.inputMove) > CoreConstants.DEADZONE_FLOAT_0)
                {
                    state.platformState |= PlatformState.MOVE;
                    state.platformState &= ~PlatformState.MOVE_NEUTRAL;
                }
                else
                {
                    state.platformState |= PlatformState.MOVE_NEUTRAL;
                    state.platformState &= ~PlatformState.MOVE;
                }
                break;
        }
    }

    public void FixedUpdate()
    {
        // todo: implement air movement
        if (state.platformState.HasFlag(PlatformState.MOVE))
        {
            if (state.trigger.down.isTriggered)
            {
                adjustedVelocityX = groundMoveSpeed * state.inputMove;
            }
            else
            {
                adjustedVelocityX = groundMoveSpeed * state.inputMove;
            }
        }
        else if (state.platformState.HasFlag(PlatformState.MOVE_NEUTRAL))
        {
            adjustedVelocityX = 0;
        }

        if (state.platformState.HasFlag(PlatformState.JUMP))
        {
            body.VelocityY(jumpStrength);

            state.platformState &= ~PlatformState.JUMP;
        }
        if (state.platformState.HasFlag(PlatformState.WALL_JUMP))
        {
            var velocity = wallJumpSpeed;
            velocity.x *= state.inputMove;

            body.VelocityY(velocity.y);
            adjustedVelocityX = velocity.x;

            state.platformState &= ~PlatformState.WALL_JUMP;
            state.platformState |= PlatformState.WALL_JUMPING;
        }

        adjustedVelocityX = Mathf.Clamp(adjustedVelocityX, -maxSpeedX, maxSpeedX);
        body.VelocityX(adjustedVelocityX);
    }

    public bool IsWallClingState()
    {
        // check if next to a wall and input is pressing into wall.
        var right = state.inputMove > CoreConstants.DEADZONE_FLOAT_0 && state.triggerState.HasFlag(Direction2D.RIGHT);
        var left = state.inputMove < -CoreConstants.DEADZONE_FLOAT_0 && state.triggerState.HasFlag(Direction2D.LEFT);

        return left || right;
    }

    public bool IsWallJumpState()
    {
        // check if next to a wall and input is pressing away from wall.
        var right = state.triggerStateBuffer.Contains(Direction2D.LEFT);
        var left = state.triggerStateBuffer.Contains(Direction2D.RIGHT);

        if (right || left)
        {
            foreach (var item in state.inputMoveBuffer)
            {
                if (item > CoreConstants.DEADZONE_FLOAT_0)
                {
                    right &= true;
                }
                if (item < -CoreConstants.DEADZONE_FLOAT_0)
                {
                    left &= true;
                }
            }
        }

        return left || right;
    }

    public bool IsCurrentWallJumpState()
    {
        // check if next to a wall and input is pressing away from wall.
        var right = state.inputMove > CoreConstants.DEADZONE_FLOAT_0 && state.triggerStateBuffer.Contains(Direction2D.LEFT);
        var left = state.inputMove < -CoreConstants.DEADZONE_FLOAT_0 && state.triggerStateBuffer.Contains(Direction2D.RIGHT);

        return left || right;
    }
}
