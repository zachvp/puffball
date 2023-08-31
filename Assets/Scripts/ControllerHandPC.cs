using System;
using UnityEngine;

public class ControllerHandPC : MonoBehaviour
{
    // links
    public PCMetadata meta;
    public Rigidbody2D body;

    public Joint2D neutralJoint;
    public GameObject neutral;

    public MovementRadial radial;
    public Joint2D radialJoint;

    public TriggerVolume triggerGrab;
    public Collider2D colliderBody;

    // config
    public float deadzone = 0.05f;

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
                if (Mathf.Abs(args.vVec2.sqrMagnitude) > deadzone)
                {
                    StartCoroutine(CoreUtilities.TaskFixedUpdate(() =>
                    {
                        neutralJoint.enabled = false;
                        radialJoint.enabled = true;
                        radial.Move(args.vVec2);
                    }));
                }
                else
                {
                    StartCoroutine(CoreUtilities.TaskFixedUpdate(() =>
                    {
                        radialJoint.enabled = false;
                        neutralJoint.enabled = true;
                        radial.ResetState();
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
