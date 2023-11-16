using System;
using UnityEngine;

public class PCMetadata : MonoBehaviour
{
    public Action onInitialized;

    // fixed links
    public ControllerBodyPC controllerBody;
    public ControllerHandPC controllerHand;

    // 1-time write vars
    [NonSerialized]
    public PCInputCommandEmitter commandEmitter;

    public void OnEnable()
    {
        Signals.instance.onPCCommandEmitterSpawn += HandlePCCommandEmitterSpawn;
    }

    public void HandlePCCommandEmitterSpawn(PCInputCommandEmitter emitter)
    {
        if (commandEmitter == null)
        {
            commandEmitter = emitter;
        }

        Emitter.Send(onInitialized);
    }
}
