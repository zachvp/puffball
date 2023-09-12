using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerHandPC : MonoBehaviour
{
    // fixed links
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

    public void Update()
    {
        //var args = buffer.buffer[buffer.index];
        var args = meta.commandEmitter.playerInput.actions[CoreActionMap.Player.MOVE_HAND].ReadValue<Vector2>();
        SceneRefs.instance.uiDebug.text = args.ToString();
        if (args.sqrMagnitude < CoreConstants.DEADZONE_FLOAT_2)
        {
            neutral.SetActive(true);

            radial.gameObject.SetActive(false);
            radial.ResetState();
        }
    }

    public void HandleCommand(PCInputArgs args)
    {
        switch (args.type)
        {
            case CoreActionMap.Player.Action.MOVE_HAND:
                
                if (args.handMove.sqrMagnitude > CoreConstants.DEADZONE_FLOAT_2)
                {
                    neutral.SetActive(false);

                    radial.gameObject.SetActive(true);
                    radial.Move(args.handMove);
                }
                break;

            case CoreActionMap.Player.Action.GRIP:
                if (args.vBool && ball && triggerGrab.triggeredTraits.HasFlag(Trait.BALL))
                {
                    if (state == State.NONE)
                    {
                        ball.Grab(radial.target);
                        state = State.GRIP;
                    }
                    else
                    {
                        ball.Drop();
                        state = State.NONE;
                    }
                }
                break;

            case CoreActionMap.Player.Action.HAND_ACTION:
                if (args.vBool && ball && triggerGrab.triggeredTraits.HasFlag(Trait.BALL))
                {
                    // todo: examine buffer and determine input velocity
                    var handVelocity = Vector2.zero;
                    var buffer = meta.commandEmitter.liveBuffer.buffer;
                    var ui = SceneRefs.instance.uiDebug;
                    ui.text = "";
                    for (var i = 1; i < buffer.Length; i++)
                    {
                        handVelocity += CoreUtilities.Abs(buffer[i].handMove - buffer[i - 1].handMove);
                        //handVelocity += Vector2.Dot(buffer[i].handMove, buffer[i - 1].handMove);
                        //handVelocity += (buffer[i].handMove - buffer[i - 1].handMove);

                        //ui.text += $"{buffer[i].handMove}\n";
                        //ui.text += $"{CoreUtilities.Abs(buffer[i].handMove - buffer[i - 1].handMove)}\n";

                        //handVelocity.x += d.handMove.x;
                        //handVelocity.y += d.handMove.y;
                    }

                    ui.text = $"{handVelocity}";

                    //Debug.Log($"hand vel: {handVelocity}");

                    ball.Throw(handVelocity);
                }
                break;
        }
    }

    public IEnumerator CheckNeutralMovement(float threshold)
    {
        var delta = 0f;
        var buffer = meta.commandEmitter.liveBuffer.buffer;
        while (true)
        {
            //foreach (var d in meta.commandEmitter.liveBuffer.buffer)
            for (var i = 0; i < 8; i++)
            {
                delta += buffer[i].handMove.sqrMagnitude;
            }

            if (delta < threshold)
            {
                yield return new WaitForSeconds(meta.commandEmitter.liveBuffer.interval);
            }
            else
            {
                break;
            }
        }

        neutral.SetActive(true);

        radial.gameObject.SetActive(false);
        radial.ResetState();
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
