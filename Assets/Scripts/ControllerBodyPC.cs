using System;
using UnityEngine;

[RequireComponent(typeof(MovementAxis))]
public class ControllerBodyPC : MonoBehaviour
{
    public PCMetadata meta;
    public Rigidbody2D body;
    public MovementAxis movement;

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
                if (args.vBool)
                {
                    body.velocity = CoreUtilities.SetY(body.velocity, jumpStrength);
                }
                break;
            case CoreActionMap.Player.MOVE:
                body.velocity = CoreUtilities.SetX(body.velocity, walkSpeed * args.vFloat);
                break;
        }
    }
}
