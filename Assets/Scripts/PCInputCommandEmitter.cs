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
                data.vVec2 = context.ReadValue<Vector2>();
                break;
            default:
                Debug.LogError($"Unhandled case: {actionType}");
                break;
        }
    }
}
