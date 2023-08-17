using UnityEngine;
using System;

public class LogicScore : MonoBehaviour
{
    public Collider2D top;
    public Collider2D bottom;
    public Collider2D scoreZone;
    public State state;
    public int score;

    [NonSerialized]
    public CoreBody ballBody;
    [NonSerialized]
    public Collider2D ballCollider;

    public void Start()
    {
        ballBody = SceneRefs.instance.ball.body;
        ballCollider = SceneRefs.instance.ball.mainCollider;

        ballBody.OnAnyColliderEnter += HandleBallColliderEntry;
    }

    public void HandleBallColliderEntry(Collider2D c)
    {
        if (c == top)
        {
            if (!state.HasFlag(State.BOTTOM))
            {
                state |= State.TOP;
            }
            else
            {
                Debug.Log("INVALID SHOT: score from under the basket");
            }
        }
        else if (c == bottom)
        {
            if (state.HasFlag(State.TOP))
            {
                score++;
                SceneRefs.instance.scoreUI.text = score.ToString();
            }

            state = State.BOTTOM;
        }

        if (!ballCollider.IsTouching(scoreZone))
        {
            state = State.NONE;
        }
    }

    [Flags]
    public enum State
    {
        NONE = 0,
        TOP = 1 << 0,
        BOTTOM = 1 << 1
    }
}
