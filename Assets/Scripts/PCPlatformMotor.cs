using UnityEngine;
using System;
using Unity.VisualScripting;

public class PCPlatformMotor : MonoBehaviour
{
    // -- read vars
    public CoreBody body;
    public ActorStatePlatform state;
    public PCMetadata metadata;

    public float jumpStrength = 100;
    public float groundMoveSpeed = 100;
    public float airMoveSpeed = 25;
    public float maxSpeedX = 200;
    public float wallJumpDelay = 0.1f;
    public Vector2 wallJumpSpeed = new Vector2Int(40, 80);

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
                if (state.down.isTriggered)
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

                if (Mathf.Abs(state.inputMove) > CoreConstants.DEADZONE_FLOAT)
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

    public void Update()
    {
        // wall cling & release
        if (!state.down.isTriggered)
        {
            if (IsWallClingState())
            {
                state.platformState |= PlatformState.WALL_CLING;
                state.platformState &= ~PlatformState.WALL_RELEASE;
                state.platformState &= ~PlatformState.WALL_JUMPING;
            }
            else
            {
                state.platformState |= PlatformState.WALL_RELEASE;
                state.platformState &= ~PlatformState.WALL_CLING;
            }

            if (IsCurrentWallJumpState())
            {
                state.platformState &= ~PlatformState.WALL_JUMPING;
            }
        }
        else
        {
            state.platformState &= ~PlatformState.WALL_CLING;
            state.platformState &= ~PlatformState.WALL_RELEASE;
            state.platformState &= ~PlatformState.WALL_JUMPING;
        }

        // todo: implement air movement
        if (state.platformState.HasFlag(PlatformState.MOVE))
        {
            if (state.down.isTriggered)
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
            body.TriggerY(jumpStrength);

            state.platformState &= ~PlatformState.JUMP;
        }
        if (state.platformState.HasFlag(PlatformState.WALL_JUMP))
        {
            var velocity = wallJumpSpeed;
            velocity.x *= state.inputMove;
            body.TriggerY(velocity.y);
            adjustedVelocityX = velocity.x;

            state.platformState &= ~PlatformState.WALL_JUMP;
            state.platformState |= PlatformState.WALL_JUMPING;
        }

        // wall cling
        if (state.platformState.HasFlag(PlatformState.WALL_CLING))
        {
            body.StopVertical();
        }

        // wall release
        if (state.platformState.HasFlag(PlatformState.WALL_RELEASE))
        {
            body.ResetVertical();

            state.platformState &= ~PlatformState.WALL_RELEASE;
            state.platformState &= ~PlatformState.WALL_CLING;
        }

        adjustedVelocityX = Mathf.Clamp(adjustedVelocityX, -maxSpeedX, maxSpeedX);

        if (Math.Abs(adjustedVelocityX) > 0)
        {
            body.TriggerX(adjustedVelocityX);
        }
    }

    public bool IsWallClingState()
    {
        // check if next to a wall and input is pressing into wall.
        var right = state.inputMove > CoreConstants.DEADZONE_FLOAT && state.triggerState.HasFlag(Direction2D.RIGHT);
        var left = state.inputMove < -CoreConstants.DEADZONE_FLOAT && state.triggerState.HasFlag(Direction2D.LEFT);

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
                if (item > CoreConstants.DEADZONE_FLOAT)
                {
                    right &= true;
                }
                if (item < -CoreConstants.DEADZONE_FLOAT)
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
        var right = state.inputMove > CoreConstants.DEADZONE_FLOAT && state.triggerStateBuffer.Contains(Direction2D.LEFT);
        var left = state.inputMove < -CoreConstants.DEADZONE_FLOAT && state.triggerStateBuffer.Contains(Direction2D.RIGHT);

        return left || right;
    }
}
