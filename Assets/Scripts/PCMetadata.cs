using System;
using UnityEngine;

public class PCMetadata : MonoBehaviour
{
    public Action onInitialized;

    // 1-time write vars
    [NonSerialized]
    public PCInputCommandEmitter commandEmitter;
    public int playerID;

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
