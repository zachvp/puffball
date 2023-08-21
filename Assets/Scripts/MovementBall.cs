using UnityEngine;
using System.Collections.Generic;

public class MovementBall : MonoBehaviour
{
    public CoreBody body;
    public Dictionary<Vector2, int> normals = new Dictionary<Vector2, int>();
    public Vector2 velocity;
    public float gravity = 1;
    public bool isGrounded;

    public void Start()
    {
        body.onCollisionBodyEnter += HandleCollisionBody;
        body.onCollisionBodyExit += HandleCollisionExitBody;
    }

    public void Update()
    {
        if (isGrounded)
        {
            velocity.y = 10;
            isGrounded = false;
        }
        else
        {
            velocity.y -= gravity * Time.fixedDeltaTime;
        }
    }

    public void FixedUpdate()
    {
        var newPos = body.position;

        // Update body to new velocity-driven position.
        newPos += velocity * (Time.fixedDeltaTime * 0.5f);
        newPos = CoreUtilities.RoundTo(newPos, CoreConstants.UNIT_ROUND_POSITION);
        body.MoveKinematic(newPos);
    }

    public void HandleCollisionExitBody(Collision2D collision)
    {
        var points = new ContactPoint2D[1];
        var count = collision.GetContacts(points);

        Debug.Log("on collision exit");

        for (var i = 0; i < count; i++)
        {
            var p = points[i];

            Debug.Log($"normal: {p.normal}");
            CoreUtilities.Decrement(normals, p.normal);
        }
    }

    public void HandleCollisionBody(Collision2D collision)
    {
        var points = new ContactPoint2D[1];
        var count = collision.GetContacts(points);

        for (var i = 0; i < count; i++)
        {
            var p = points[i];

            Debug.Log($"normal: {p.normal}");
            CoreUtilities.Increment(normals, p.normal);

            if (Vector2.Dot(Vector2.up, p.normal) > CoreConstants.THRESHOLD_DOT_LOOSE)
            {
                Debug.Log("below!");
                //velocity.y = 15;
                velocity.y = 0;
                isGrounded = true;
            }
        }
    }
}
