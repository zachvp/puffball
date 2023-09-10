using System.Collections;
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
                var threshold = 0.01f;
                if (Mathf.Abs(args.handMove.sqrMagnitude) > threshold)
                {
                    neutral.SetActive(false);

                    radial.gameObject.SetActive(true);
                    radial.Move(args.handMove);
                }
                else
                {
                    StartCoroutine(CheckNeutralMovement(threshold));
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

    public IEnumerator CheckNeutralMovement(float threshold)
    {
        var delta = 0f;
        while (true)
        {
            foreach (var d in meta.commandEmitter.liveInputBuffer.buffer)
            {
                delta += d.handMove.sqrMagnitude;
            }

            if (delta < threshold)
            {
                yield return new WaitForSeconds(meta.commandEmitter.liveInputBuffer.interval);
            }
            else
            {
                break;
            }
        }

        neutral.SetActive(true);

        radial.gameObject.SetActive(false);
        //radial.ResetState();
        Debug.Log("reset");
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
