using UnityEngine.InputSystem;
using UnityEngine;

using System;

public class TestKinematicBody : MonoBehaviour
{
    public Rigidbody2D body;
    public Collider2D attachedCollider;
    public TriggerVolume triggerDown;
    public Command command;
    public Command commandPrev;

    public Vector2 velocity;

    public float speed = 5;
    public float jumpStrength = 10;
    public float gravity = 1;

    public void Update()
    {
        if (Keyboard.current.upArrowKey.wasPressedThisFrame && triggerDown.isTriggered)
        {
            command |= Command.JUMP;
        }
        
        // horizontal
        if (Keyboard.current.rightArrowKey.isPressed && !Keyboard.current.leftArrowKey.isPressed)
        {
            command |= Command.MOVE_RIGHT;
        }
        else if (Keyboard.current.leftArrowKey.isPressed && !Keyboard.current.rightArrowKey.isPressed)
        {
            command |= Command.MOVE_LEFT;
        }
        else
        {
            command |= Command.MOVE_NONE;
        }
    }

    public void FixedUpdate()
    {
        Move0();
    }

    private void Move0()
    {
        var newPos = body.position;

        if (command.HasFlag(Command.JUMP))
        {
            velocity.y = jumpStrength;
            command &= ~Command.JUMP;
        }
        else if (triggerDown.isTriggered)
        {
            velocity.y = 0;
        }
        else
        {
            velocity.y -= gravity;
        }

        if (command.HasFlag(Command.MOVE_RIGHT))
        {
            velocity.x = speed;
            command &= ~Command.MOVE_RIGHT;
        }
        if (command.HasFlag(Command.MOVE_LEFT))
        {
            velocity.x = -speed;
            command &= ~Command.MOVE_LEFT;
        }
        if (command.HasFlag(Command.MOVE_NONE))
        {
            velocity.x = 0;
            command &= ~Command.MOVE_NONE;
        }

        newPos += velocity * Time.fixedDeltaTime;

        newPos = CoreUtilities.RoundTo(newPos, CoreConstants.UNIT_ROUND_POSITION);

        body.MovePosition(newPos);

        // todo: if any part of the character is touching something else, and RigidBody does not change position after X time, zero out velocity
    }

    [Flags]
    public enum Command
    {
        NONE = 0,
        JUMP = 1 << 0,
        MOVE_LEFT = 1 << 1,
        MOVE_RIGHT = 1 << 2,
        MOVE_NONE = 1 << 3,
        JUMP_PHASE_1 = 1 << 4,
    }
}
