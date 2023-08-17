using System;
using UnityEngine;

public class PCMetadata : MonoBehaviour
{
    // 1-time write vars
    public int playerID;
    public PCInputCommandEmitter commandEmitter;
    public Action onInitialized;

    public void OnEnable()
    {
        Signals.instance.onPCCommandEmitterSpawn += HandlePCCommandEmitterSpawn;
    }

    public void Start()
    {
        PCIDRegistry.instance.Register(this, (id) =>
        {
            playerID = id;
        });
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
