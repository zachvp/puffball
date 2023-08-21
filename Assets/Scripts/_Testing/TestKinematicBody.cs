using UnityEngine.InputSystem;
using UnityEngine;

using System;

public class TestKinematicBody : MonoBehaviour
{
    public Rigidbody2D body;
    public Collider2D attachedCollider;
    public TriggerVolume triggerDown;
    public ActorStateTrigger trigger;
    public Command command;
    public Command commandPrev;

    public Vector2 velocity;
    public Vector3 previousPosition;

    public float speed = 5;
    public float jumpStrength = 10;
    public float gravity = 1;

    public void Update()
    {
        if (Keyboard.current.upArrowKey.wasPressedThisFrame && triggerDown.isTriggered)
        {
            command |= Command.JUMP;
        }

        if (!triggerDown.isTriggered)
        {
            command |= Command.FALL;
        }
        
        // horizontal
        if (Keyboard.current.rightArrowKey.isPressed && !Keyboard.current.leftArrowKey.isPressed && !trigger.right.isTriggered)
        {
            command |= Command.MOVE_RIGHT;
        }
        else if (Keyboard.current.leftArrowKey.isPressed && !Keyboard.current.rightArrowKey.isPressed && !trigger.left.isTriggered)
        {
            command |= Command.MOVE_LEFT;
        }
        else
        {
            command |= Command.MOVE_NONE;
            command &= ~Command.MOVE_LEFT;
            command &= ~Command.MOVE_RIGHT;
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
        else if (command.HasFlag(Command.FALL))
        {
            velocity.y -= gravity;
            command &= ~Command.FALL;
        }
        else if (triggerDown.isTriggered)
        {
            velocity.y = 0;
        }

        if (command.HasFlag(Command.MOVE_RIGHT) && !command.HasFlag(Command.MOVE_NONE))
        {
            velocity.x = speed;
            command &= ~Command.MOVE_RIGHT;
        }
        if (command.HasFlag(Command.MOVE_LEFT) && !command.HasFlag(Command.MOVE_NONE))
        {
            velocity.x = -speed;
            command &= ~Command.MOVE_LEFT;
        }

        if (command.HasFlag(Command.MOVE_NONE))
        {
            velocity.x = 0;
            command &= ~Command.MOVE_NONE;
        }

        newPos += velocity * (Time.fixedDeltaTime / 2);

        newPos = CoreUtilities.RoundTo(newPos, CoreConstants.UNIT_ROUND_POSITION);

        body.MovePosition(newPos);

    }

    [Flags]
    // todo: stop hardcoding values
    public enum Command
    {
        NONE = 0,
        FALL = 1 << 0,
        JUMP = 1 << 1,
        MOVE_LEFT = 1 << 2,
        MOVE_RIGHT = 1 << 3,
        MOVE_NONE = 1 << 4,
        CORRECTED_Y = 1 << 6
    }
}
