using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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

    //public void LateUpdate()
    //{
    //    var fixedPos = body.position;

    //    fixedPos.x = CoreUtilities.RoundTo(fixedPos.x, 1f / 16f);
    //    fixedPos.y = CoreUtilities.RoundTo(fixedPos.y, 1f / 16f);

    //    CoreUtilities.PostFixedUpdateTask(() =>
    //    {
    //        //body.MovePosition(fixedPos);
    //        body.position = fixedPos;
    //    });

    //    //transform.position = fixedPos;
    //}

    public void FixedUpdate()
    {
        Move0();

        //var roundedPos = body.position;
        //roundedPos.Set(Mathf.RoundToInt(body.position.x), Mathf.RoundToInt(body.position.y));

        //body.MovePosition(roundedPos);
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
            //velocity.y += 0.001f; // fudge y to avoid getting stuck on ground
            command &= ~Command.MOVE_RIGHT;
        }
        if (command.HasFlag(Command.MOVE_LEFT))
        {
            velocity.x = -speed;
            //velocity.y += 0.001f; // fudge y to avoid getting stuck on ground
            command &= ~Command.MOVE_LEFT;
        }
        if (command.HasFlag(Command.MOVE_NONE))
        {
            velocity.x = 0;
            command &= ~Command.MOVE_NONE;
        }

        newPos += velocity * Time.fixedDeltaTime;

        newPos.x = CoreUtilities.RoundTo(newPos.x, CoreConstants.UNIT_ROUND_POSITION);
        newPos.y = CoreUtilities.RoundTo(newPos.y, CoreConstants.UNIT_ROUND_POSITION);

        body.MovePosition(newPos);

        // todo: if any part of the character is touching something else, and RigidBody does not change position after X time, zero out velocity
    }

    private void Move1()
    {
        if (command.HasFlag(Command.MOVE_RIGHT))
        {
            var newPos = body.position;

            newPos.x += 0.5f;

            body.MovePosition(newPos);
            command ^= Command.MOVE_RIGHT;
        }

        if (command.HasFlag(Command.MOVE_LEFT))
        {
            var newPos = body.position;

            newPos.x -= 0.5f;

            body.MovePosition(newPos);
            command ^= Command.MOVE_LEFT;
        }

        if (!triggerDown.isTriggered)
        {
            var newPos = body.position;

            newPos.y -= 0.75f;

            body.MovePosition(newPos);
        }

        if (command.HasFlag(Command.JUMP))
        {
            var newPos = body.position;

            newPos.y += 0.75f * 8 * Time.deltaTime;
            command ^= Command.JUMP;
            command |= Command.JUMP_PHASE_1;

            body.MovePosition(newPos);
        }
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
