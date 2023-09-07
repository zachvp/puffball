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

        triggerGrab.onTraitFound += HandleTraitFound;
    }

    public void HandleCommand(PCInputArgs args)
    {
        switch (args.type)
        {
            case CoreActionMap.Player.Action.MOVE_HAND:
                if (Mathf.Abs(args.vVec2.sqrMagnitude) > CoreConstants.DEADZONE_FLOAT_1)
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
            case CoreActionMap.Player.Action.GRIP:
                if (args.vBool && ball && triggerGrab.triggeredTraits.HasFlag(Trait.BALL))
                {
                    if (state == State.NONE)
                    {
                        ball.GrabNew(radial.target);
                        state = State.GRIP;
                    }
                    else
                    {
                        ball.Drop();
                        state = State.NONE;
                    }
                }

                break;
        }
    }

    public void HandleTraitFound(ContainerTrait trait)
    {
        if (trait.value.HasFlag(Trait.BALL))
        {
            ball = trait.GetComponentInChildren<Ball>();
        }
    }

    // -- Class definitions
    public enum State
    {
        NONE = 0,
        GRIP = 1 << 0,
    }
}
