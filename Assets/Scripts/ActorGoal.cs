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
            Debug.Log("nothing in goal area, reset state");
        }
    }

    public void HandleTraitFoundTop(ContainerTrait trait)
    {
        if (trait.value.HasFlag(Trait.BALL))
        {
            if (state.HasFlag(State.TRIGGER_BOTTOM))
            {
                Debug.LogWarning($"invalid score attempt");
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

                // todo: signal score event, which UI text responds to
                SceneRefs.instance.scoreUI.text = $"SCORE: {++score}";
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
