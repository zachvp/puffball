using System;
using UnityEngine;

[RequireComponent(typeof(MovementJump))]
public class ControllerBodyPC : MonoBehaviour
{
    public PCMetadata meta;
    public MovementJump movementJump; // todo: remove and only use movementAxis
    public MovementAxis movementAxis;

    public float jumpStrength = 10;
    public float walkSpeed = 8;

    public void Awake()
    {
        meta.onInitialized += () =>
        {
            meta.commandEmitter.onPCCommand += HandleCommand;
        };
    }

    public void HandleCommand(PCInputArgs args)
    {
        switch (args.type)
        {
            case CoreActionMap.Player.JUMP:
                movementJump.Jump(jumpStrength);
                break;
            case CoreActionMap.Player.MOVE:
                movementAxis.MoveX(walkSpeed * args.vFloat);
                break;
        }
    }
}
