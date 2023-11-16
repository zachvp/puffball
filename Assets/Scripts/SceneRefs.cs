using UnityEngine;
using System.Collections.Generic;
using TMPro;
using ZCore;

public class SceneRefs : Singleton<SceneRefs>
{
    public TargetGoal targetGoal
        { get { return registry[ID.TARGET_GOAL] as TargetGoal; } }

    public ActorBall ball
        { get { return registry[ID.BALL] as ActorBall; } }

    public TextMeshProUGUI scoreUI
        { get { return registry[ID.UI_SCORE] as TextMeshProUGUI; } }

    public Camera camera
        { get { return registry[ID.CAMERA] as Camera; } }

    public readonly Dictionary<ID, object> registry;

    public SceneRefs()
    {
        registry = new Dictionary<ID, object>();
    }

    public enum ID
    {
        TARGET_GOAL,
        BALL,
        UI_SCORE,
        CAMERA
    }
}
