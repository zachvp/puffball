using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class PCInputCommandEmitter : MonoBehaviour
{
    public PlayerInput playerInput;
    public CoreActionMap.Type actionMapType;
    public Action<PCInputArgs> onPCCommand;

    // Contains the combination of the most recent input data.
    public PCInputArgs data;

    // Mouse-specific data
    private BufferInterval<Vector2> bufferMouse = new BufferInterval<Vector2>(8, CoreConstants.UNIT_TIME_SLICE);
    public Vector2 relativeOrigin;
    public float mouseLength = 4;
    public Vector2 currentMouse;

    public void Awake()
    {
        data.playerIndex = playerInput.playerIndex;
    }

    public void OnEnable()
    {
        playerInput.onActionTriggered += HandleActionTriggered;
    }

    public void OnDisable()
    {
        playerInput.onActionTriggered -= HandleActionTriggered;
    }

    public void Start()
    {
        Emitter.Send(Signals.instance.onPCCommandEmitterSpawn, this);

        relativeOrigin = Mouse.current.position.ReadValue();
    }

    public void Update()
    {
        currentMouse = Mouse.current.position.ReadValue();

        if (CoreConstants.CONTROL_SCHEME_KEYBOARD_MOUSE.Equals(playerInput.currentControlScheme))
        {
            UpdateRelativeOrigin(Mouse.current);
        }
    }

    public void UpdateRelativeOrigin(Mouse mouse)
    {
        var diff = Vector2.zero;

        bufferMouse.Add(mouse.position.ReadValue(), Time.time);

        // accumulate each difference between all buffered distances
        for (var i = 0; i < bufferMouse.buffer.Length; i++)
        {
            for (var j = 1; j < bufferMouse.buffer.Length; j++)
            {
                if (i != j)
                {
                    diff += bufferMouse.buffer[j] - bufferMouse.buffer[i];
                }
            }
        }

        // reset the relative cursor origin if there's no meaningful difference
        if (diff.sqrMagnitude < CoreConstants.DEADZONE_FLOAT_0)
        {
            // offset the origin by the current input value
            relativeOrigin = mouse.position.ReadValue() - (data.vVec2 * mouseLength);
        }
    }

    public void HandleActionTriggered(InputAction.CallbackContext context)
    {
        // Check if player input is active. The input system triggers an event when it's disabled
        // (e.g. when scene is unloaded), which means some downstream objects may be accessed
        // after they've been deallocated.
        if (playerInput.isActiveAndEnabled && CoreActionMap.GetActionMap(context.action.actionMap.name) == actionMapType)
        {
            var actionType = CoreActionMap.GetPlayerAction(context.action.name);

            UpdateData(actionType, context);

            Emitter.Send(Signals.instance.onPCCommand, data);
            Emitter.Send(onPCCommand, data);
        }
    }

    public void UpdateData(CoreActionMap.Player.Action actionType, InputAction.CallbackContext context)
    {
        data.type = actionType;

        switch (actionType)
        {
            case CoreActionMap.Player.Action.JUMP:
            case CoreActionMap.Player.Action.GRIP:
            case CoreActionMap.Player.Action.START:
            case CoreActionMap.Player.Action.HAND_ACTION:
                data.vBool = context.phase == InputActionPhase.Performed;
                break;

            case CoreActionMap.Player.Action.MOVE:
                data.vFloat = context.ReadValue<float>();
                break;

            case CoreActionMap.Player.Action.MOVE_HAND:
                if (context.control.device is Mouse)
                {
                    var mouse = Mouse.current;

                    data.vVec2 = mouse.position.ReadValue() - relativeOrigin;
                    data.vVec2 = data.vVec2.normalized * (data.vVec2.magnitude / mouseLength);
                    data.vVec2 = Vector2.ClampMagnitude(data.vVec2, 1);

                    //CoreUtilities.DrawScreenLine(SceneRefs.instance.camera, relativeOrigin, mouse.position.ReadValue());
                }
                else
                {
                    data.vVec2 = context.ReadValue<Vector2>();
                }

                break;

            default:
                Debug.LogError($"Unhandled case: {actionType}");
                break;
        }
    }
}
