using System;
using UnityEngine;

public class ControllerHandPC : MonoBehaviour
{
    // links
    public PCMetadata meta;

    public GameObject neutral;

    public MovementRadial radial;
    public DistanceJoint2D radialJoint;

    public TriggerVolume triggerGrab;
    public Collider2D colliderBody;

    // dynamic links
    private Ball ball;

    // state
    private State state;

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
                if (Mathf.Abs(args.vVec2.sqrMagnitude) > CoreConstants.DEADZONE_FLOAT_2)
                {
                    StartCoroutine(CoreUtilities.TaskFixedUpdate(() =>
                    {
                        neutral.SetActive(false);

                        radialJoint.enabled = true;
                        radial.Move(args.vVec2);
                    }));
                }
                else
                {
                    StartCoroutine(CoreUtilities.TaskFixedUpdate(() =>
                    {
                        radialJoint.enabled = false;
                        radial.ResetState();

                        neutral.SetActive(true);
                    }));
                }
                
                break;
            case CoreActionMap.Player.GRIP:
                // todo: implement
                break;
        }
    }

    // -- Class definitions
    [Flags]
    public enum State
    {
        NONE = 0,
        GRIP = 1 << 0,
        BLOCKED = 1 << 1
    }
}
