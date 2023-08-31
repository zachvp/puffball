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

    public TriggerVolume triggerGrab;
    public Collider2D colliderBody;

    // config
    public float deadzone = 0.05f;

    // dynamic links
    private Ball ball;

    // state
    private State state;
    private BufferInterval<Vector2> inputMove = new BufferInterval<Vector2>(8, 1/60f);
    public Vector2 dbgHandMove;

    public void Awake()
    {
        meta.onInitialized += () =>
        {
            meta.commandEmitter.onPCCommand += HandleCommand;
        };
    }

    public void Update()
    {
        var value = meta.commandEmitter.playerInput.currentActionMap["Move Hand"].ReadValue<Vector2>();

        inputMove.Add(value, Time.time);

        var delta = Vector2.zero;
        foreach (var i in inputMove.buffer)
        {
            delta += value - i; 
        }

        if (delta.sqrMagnitude < 0.1f)
        {
            //Debug.Log("detect no move delta");
        }
        else
        {
            //Debug.Log($"move delta too big: {delta}");
            //body.drag = 1;
            //spring.dampingRatio = 0.25f;
        }
    }

    public void HandleCommand(PCInputArgs args)
    {
        switch (args.type)
        {
            case CoreActionMap.Player.MOVE_HAND:
                if (Mathf.Abs(args.vVec2.sqrMagnitude) > deadzone)
                {
                    neutralJoint.enabled = false;
                    radial.Move(args.vVec2);
                }
                else
                {
                    radial.ResetPosition();
                    neutralJoint.enabled = true;
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
