using UnityEngine;

public class MovementSettle : MonoBehaviour
{
    public Rigidbody2D body;
    public float threshold = 0.1f;

    public void FixedUpdate()
    {
        if (body.velocity.sqrMagnitude < threshold)
        {
            body.velocity = Vector2.zero;
        }
    }
}
