using System;
using UnityEngine;

public class ControllerHandPC : MonoBehaviour
{
    // links
    public PCMetadata meta;
    public Rigidbody2D body;
    public Rigidbody2D rootBody;

    public SpringJoint2D neutralJoint;
    public GameObject neutral;

    public MovementRadial radial;
    public DistanceJoint2D radialJoint;

    public TriggerVolume triggerGrab;
    public Collider2D colliderBody;

    public FixedJoint2D fixedJoint;

    // config
    public float deadzone = 0.05f;
    public float velocityLimit = 10;

    // dynamic links
    private Ball ball;

    // state
    private State state;

    Tuple<float, float> springState;

    public void Awake()
    {
        meta.onInitialized += () =>
        {
            meta.commandEmitter.onPCCommand += HandleCommand;
        };

        springState = new Tuple<float, float>(neutralJoint.dampingRatio, neutralJoint.frequency);
    }

    // todo: simply stop the rigid bodies in their current relative position when distance over a limit
    public void FixedUpdate()
    {
        //if (rootBody.velocity.sqrMagnitude > velocityLimit * velocityLimit)
        if ((rootBody.position - body.position).sqrMagnitude > velocityLimit * velocityLimit)
        {
            radialJoint.distance = (rootBody.position - body.position).magnitude;
            Debug.Log($"sqr mag: {(rootBody.position - body.position).sqrMagnitude}");
        }
        else
        {
            radialJoint.distance = 1.5f;
        }
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
                        //neutralJoint.enabled = false;
                        //radialJoint.enabled = true;
                        radial.Move(args.vVec2);
                    }));
                }
                else
                {
                    StartCoroutine(CoreUtilities.TaskFixedUpdate(() =>
                    {
                        //radialJoint.enabled = false;
                        //neutralJoint.enabled = true;
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
