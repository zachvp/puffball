using System;
using UnityEngine;

public class ControllerHandPC : MonoBehaviour
{
    // links
    public PCMetadata meta;
    public MovementFollowTransform handNeutral;
    public MovementRadial handRadial;

    // config
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
                    handRadial.Move(args.vVec2);
                }
                else
                {
                    handNeutral.type = MovementFollowTransform.FollowType.LAG;
                    handRadial.ResetPosition();
                }
                
                break;
        }
    }
}
