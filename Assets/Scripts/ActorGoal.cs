using System;
using UnityEngine;

public class ActorGoal : MonoBehaviour
{
    // fixed links
    public CoreSignalsCollision signals;
    public TriggerVolume triggerTop;
    public TriggerVolume triggerBottom;
    public TriggerVolume triggerGoalAreal;

    // state
    public State state;

    // testing
    public int score;

    public void Awake()
    {
        triggerTop.onTraitFound += HandleTraitFoundTop;
        triggerBottom.onTraitFound += HandleTraitFoundBottom;
        triggerGoalAreal.onUpdateState += HandleUpdateStateGoalArea;
    }

    public void HandleUpdateStateGoalArea()
    {
        if (!triggerGoalAreal.isTriggered)
        {
            state = State.NONE;
        }
    }

    public void HandleTraitFoundTop(ContainerTrait trait)
    {
        if (trait.value.HasFlag(Trait.BALL))
        {
            if (state.HasFlag(State.TRIGGER_BOTTOM))
            {
                state = State.NONE;
            }
            else
            {
                state |= State.TRIGGER_TOP;
            }
        }
    }

    public void HandleTraitFoundBottom(ContainerTrait trait)
    {
        if (trait.value.HasFlag(Trait.BALL))
        {
            state |= State.TRIGGER_BOTTOM;

            if (state.HasFlag(State.TRIGGER_TOP))
            {
                Debug.Log("score!");
                state = State.NONE;

                SceneRefs.instance.scoreUI.text = $"SCORE: {++score}";

                // todo: handle this event
                Emitter.Send(Signals.instance.onScore);
            }
        }
    }

    [Flags]
    public enum State
    {
        NONE = 0,
        TRIGGER_TOP = 1 << 0,
        TRIGGER_BOTTOM = 1 << 1
    }
}
