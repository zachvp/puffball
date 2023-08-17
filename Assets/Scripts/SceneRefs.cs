using System.Collections.Generic;
using TMPro;

public class SceneRefs : Singleton<SceneRefs>
{
    public TargetGoal targetGoal
        { get { return registry[ID.TARGET_GOAL] as TargetGoal; } }
    public Ball ball
        { get { return registry[ID.BALL] as Ball; } }
    public TextMeshProUGUI scoreUI
        { get { return registry[ID.UI_SCORE] as TextMeshProUGUI; } }

    public readonly Dictionary<ID, object> registry;

    public SceneRefs()
    {
        registry = new Dictionary<ID, object>();
    }

    public enum ID
    {
        TARGET_GOAL,
        BALL,
        UI_SCORE
    }
}
