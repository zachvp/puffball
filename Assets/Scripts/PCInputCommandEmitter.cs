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
    private BufferCircular<Vector2> bufferMouse = new BufferCircular<Vector2>(60);
    public Vector2 relativeOrigin;
    public float mouseLength = 4;
    public Camera currentCamera;

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
        var mouse = Mouse.current;
        var diff = Vector2.zero;

        // todo:
        bufferMouse.Add(mouse.position.ReadValue());

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

        if (diff.sqrMagnitude < CoreConstants.DEADZONE_FLOAT_0)
        {
            relativeOrigin = mouse.position.ReadValue();

            data.vVec2 = Vector2.zero;
            data.type = CoreActionMap.Player.Action.MOVE_HAND;
            Emitter.Send(onPCCommand, data);
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
                    var controlVec = (mouse.position.ReadValue() - relativeOrigin);
                    var normalized = controlVec.normalized * (controlVec.magnitude / mouseLength);
                    var cam = currentCamera;

                    //Debug.Log($"move hand; mouse pos: {mouse.position.ReadValue()}");
                    data.vVec2 = Vector2.ClampMagnitude(normalized, 1);
                    
                    Debug.DrawLine((Vector2) cam.ScreenToWorldPoint(relativeOrigin), (Vector2) cam.ScreenToWorldPoint(relativeOrigin + normalized), Color.red, 0.2f);
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
