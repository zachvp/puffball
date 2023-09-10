using System;
using UnityEngine;

public class ControllerHandPC : MonoBehaviour
{
    // links
    public PCMetadata meta;

    public GameObject neutral;

    public MovementRadial radial;

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
                if (Mathf.Abs(args.vVec2.sqrMagnitude) > CoreConstants.DEADZONE_FLOAT_0)
                {
                    //var neutralComponents = neutral.GetComponents<Behaviour>();
                    //foreach (var c in neutralComponents)
                    //{
                    //    c.enabled = false;
                    //}
                    neutral.SetActive(false);

                    //var radialComponents = radial.GetComponents<Behaviour>();
                    //foreach (var c in radialComponents)
                    //{
                    //    c.enabled = true;
                    //}

                    radial.gameObject.SetActive(true);
                    radial.Move(args.vVec2);
                }
                else
                {
                    // todo: clean up
                    //var neutralComponents = neutral.GetComponents<Behaviour>();
                    //foreach (var c in neutralComponents)
                    //{
                    //    c.enabled = true;
                    //}
                    neutral.SetActive(true);

                    //var radialComponents = radial.GetComponents<Behaviour>();
                    //foreach (var c in radialComponents)
                    //{
                    //    c.enabled = false;
                    //}
                    radial.gameObject.SetActive(false);
                    radial.ResetState();
                    Debug.Log("reset");
                    // todo 0: only reset if there has been 0 movement input for X frames
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
