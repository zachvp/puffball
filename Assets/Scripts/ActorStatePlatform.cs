using UnityEngine;
using System;
using System.Collections.Generic;

// todo: migrate to scriptable object that spawns at runtime
[Serializable]
public class ActorStatePlatform : MonoBehaviour
{
    public ActorStateTrigger trigger;

    // current state data
    public PlatformState platformState;
    public Direction2D triggerState;
    public float inputMove;

    // buffered state data
    public float bufferLifetime = 0.5f;
    public LinkedList<Direction2D> triggerStateBuffer = new LinkedList<Direction2D>();
    public LinkedList<float> inputMoveBuffer = new LinkedList<float>();

    public void Update()
    {
        triggerState = EnumHelper.FromBool(trigger.left.isTriggered, trigger.right.isTriggered, trigger.down.isTriggered, false);

        // state buffers
        var triggerEntry = triggerStateBuffer.AddLast(triggerState);
        var inputMoveEntry = inputMoveBuffer.AddLast(inputMove);

        StartCoroutine(CoreUtilities.TaskDelayed(bufferLifetime, triggerEntry, inputMoveEntry, (trigger, input) =>
        {
            triggerStateBuffer.Remove(trigger);
            inputMoveBuffer.Remove(input);
        }));
    }
}

[Serializable]
public struct ActorStateTrigger
{
    // proximity triggers
    public TriggerVolume right;
    public TriggerVolume left;
    public TriggerVolume down;
}

[Flags]
public enum PlatformState
{
    NONE = 0,
    JUMP = 1 << 0,
    MOVE = 1 << 1,
    MOVE_NEUTRAL = 1 << 2,
    WALL_JUMP = 1 << 3,
    WALL_JUMPING = 1 << 4,
    WALL_CLING = 1 << 5,
    WALL_RELEASE = 1 << 6,
}
