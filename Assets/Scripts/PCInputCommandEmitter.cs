using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class PCInputCommandEmitter : MonoBehaviour
{
    public PlayerInput playerInput;
    public CoreActionMap.Type actionMapType; // todo: this may not make sense since UpdateData assumes player action map
    public Action<PCInputArgs> onPCCommand;

    // Contains the combination of the most recent input data.
    public PCInputArgs data;

    public void Awake()
    {
        data.playerIndex = playerInput.playerIndex;
    }

    public void Start()
    {
        Emitter.Send(Signals.instance.onPCCommandEmitterSpawn, this);
    }

    public void OnEnable()
    {
        playerInput.onActionTriggered += HandleActionTriggered;
    }

    public void OnDisable()
    {
        playerInput.onActionTriggered -= HandleActionTriggered;
    }

    public void HandleActionTriggered(InputAction.CallbackContext context)
    {
        if (EnumHelper.GetActionMap(context.action.actionMap.name) == actionMapType)
        {
            var actionType = EnumHelper.GetPlayerAction(context.action.name);

            UpdateData(actionType, context);

            Emitter.Send(Signals.instance.onPCCommand, data);
            Emitter.Send(onPCCommand, data);
        }
    }

    public void UpdateData(CoreActionMap.Player actionType, InputAction.CallbackContext context)
    {
        data.type = actionType;

        switch (actionType)
        {
            case CoreActionMap.Player.JUMP:
            case CoreActionMap.Player.GRIP:
            case CoreActionMap.Player.START:
            case CoreActionMap.Player.THROW:
                data.vBool = context.phase == InputActionPhase.Started;
                break;
            case CoreActionMap.Player.MOVE:
                data.vFloat = context.ReadValue<float>();
                break;
            case CoreActionMap.Player.MOVE_HAND:
                data.vVec2 = context.ReadValue<Vector2>();
                break;
            default:
                Debug.LogError($"Unhandled case: {actionType}");
                break;
        }
    }
}
