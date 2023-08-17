using UnityEngine;
using System.Collections.Generic;

public class ActorStatePlatform : MonoBehaviour
{
    // proximity triggers
    public TriggerVolume right;
    public TriggerVolume left;
    public TriggerVolume down;

    // current state data
    public PlatformState platformState;
    public Direction2D triggerState;
    public float inputMove;

    // buffered state data
    public float bufferLifetime = 0.5f;
    public LinkedList<Direction2D> triggerStateBuffer;
    public LinkedList<float> inputMoveBuffer;

    public void Awake()
    {
        triggerStateBuffer = new LinkedList<Direction2D>();
        inputMoveBuffer = new LinkedList<float>();
    }

    public void Update()
    {
        triggerState = EnumHelper.FromBool(left.isTriggered, right.isTriggered, down.isTriggered, false);

        // state buffers
        var triggerEntry = triggerStateBuffer.AddLast(triggerState);
        var inputMoveEntry = inputMoveBuffer.AddLast(inputMove);

        StartCoroutine(CoreUtilities.DelayedTask(bufferLifetime, () =>
        {
            triggerStateBuffer.Remove(triggerEntry);
            inputMoveBuffer.Remove(inputMoveEntry);
        }));
    }
}
