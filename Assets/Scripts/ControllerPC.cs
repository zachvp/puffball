using System;
using UnityEngine;

public class ControllerPC : MonoBehaviour, IControlPC
{
    public PCMetadata meta;
    public MovementFollowTransform handNeutral;
    public MovementRadial handRadial;
    public float deadzone = 0.05f;

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
            case CoreActionMap.Player.MOVE_HAND:
                if (Mathf.Abs(args.vVec2.sqrMagnitude) > deadzone)
                {
                    handNeutral.type = MovementFollowTransform.FollowType.SNAP;
                    handRadial.RadialPosition(args.vVec2);

                }
                else
                {
                    handNeutral.type = MovementFollowTransform.FollowType.LAG;
                    handRadial.Reset();
                }
                
                break;
        }
    }
}
