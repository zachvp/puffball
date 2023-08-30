using System;
using UnityEngine;

public class ControllerHandPC : MonoBehaviour
{
    // links
    public PCMetadata meta;
    public Rigidbody2D body;
    public SpringJoint2D spring;
    public GameObject neutral;
    public MovementRadial radial;


    public TriggerVolume triggerGrab;
    public Collider2D colliderBody;

    // config
    public float deadzone = 0.05f;

    // dynamic links
    private Ball ball;

    // state
    State state;

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
                    //radial.gameObject.SetActive(true);
                    radial.Move(args.vVec2);
                    body.drag = 8;
                    spring.dampingRatio = 0.75f;
                    //body.velocity = Vector2.zero;
                }
                else
                {
                    radial.ResetPosition();
                    //radial.gameObject.SetActive(false);
                    StartCoroutine(CoreUtilities.TaskDelayed(1.2f, () =>
                    {
                        body.drag = 1;
                        spring.dampingRatio = 0.25f;
                    }));
                }
                
                break;





            case CoreActionMap.Player.GRIP:
                if (args.vBool && triggerGrab.isTriggered)
                {
                    if (state == State.NONE)
                    {
                        var grabObject = triggerGrab.overlappingObjects[0];
                        Debug.Log($"triggered grab on object: {grabObject}");
                        ball = grabObject.GetComponent<Ball>();
                        Debug.Assert(ball != null, $"unable to find {nameof(Ball)} component");
                        ball.GrabNew(transform);
                        var collider = grabObject.GetComponent<Collider2D>();

                        Physics2D.IgnoreCollision(colliderBody, collider);
                        state |= State.GRIP;
                    }
                }
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
