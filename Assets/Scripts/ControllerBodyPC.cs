using System;
using UnityEngine;

[RequireComponent(typeof(MovementJump))]
public class ControllerBodyPC : MonoBehaviour
{
    public PCMetadata meta;
    public MovementJump movementJump;

    public float jumpStrength = 10;

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
        }
    }
}
