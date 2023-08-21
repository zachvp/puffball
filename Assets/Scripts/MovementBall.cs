using UnityEngine;
using System.Collections.Generic;

public class MovementBall : MonoBehaviour
{
    public CoreBody body;
    public Dictionary<Collider2D, int> collisionMap = new Dictionary<Collider2D, int>();
    public List<Vector2> normals = new List<Vector2>();
    public Vector2 velocity;
    [Range(0, 7)] public float gravity = 1;
    public float bounceVelocity = 20;
    public int bounceCount = 20;
    public float bounceCoefficient = 0.5f;

    public bool isGrounded;
    public int ticksBounce;
    public float gravityOffset;
    public float lastTime;
    public bool isSettled;

    public void Start()
    {
        body.onCollisionBodyEnter += HandleCollisionBody;
        body.onCollisionBodyExit += HandleCollisionExitBody;
    }

    public void Update()
    {
        if (!isGrounded)
        {
            var diff = (gravity + gravityOffset) * Time.fixedDeltaTime;

            //velocity.y -= Mathf.Min(32*gravity, diff);
            velocity.y -= diff;
        }
    }

    public void FixedUpdate()
    {
        var newPos = body.position;

        // Update body to new velocity-driven position.
        newPos += velocity * (Time.fixedDeltaTime * 0.5f);
        //newPos = CoreUtilities.RoundTo(newPos, CoreConstants.UNIT_ROUND_POSITION);
        body.MoveKinematic(newPos);
    }

    public void HandleCollisionExitBody(Collision2D collision)
    {
        var points = new ContactPoint2D[1];
        var count = collision.GetContacts(points);

        Debug.Log("on collision exit");
        collisionMap[collision.collider] -= 1;
        if (collisionMap[collision.collider] == 0)
        {
            collisionMap.Remove(collision.collider);
        }

        if (collisionMap.Count == 0)
        {
            isGrounded = false;

            if (isSettled)
            {
                isSettled = false;
            }
        }
    }

    public void HandleCollisionBody(Collision2D collision)
    {
        var points = new ContactPoint2D[1];
        var count = collision.GetContacts(points);

        Debug.Log($"collision collider: {collision.collider}");

        if (collisionMap.ContainsKey(collision.collider))
        {
            collisionMap[collision.collider] += 1;
        }
        else
        {
            collisionMap.Add(collision.collider, 1);
        }

        for (var i = 0; i < count; i++)
        {
            var p = points[i];

            Debug.Log($"normal: {p.normal}");
            //CoreUtilities.Increment(normals, collision);

            if (Vector2.Dot(Vector2.up, p.normal) > CoreConstants.THRESHOLD_DOT_LOOSE)
            {
                Debug.Log("below!");
                //velocity.y = 15;
                velocity.y = 0;
                isGrounded = true;

                if (Time.time - lastTime < 1f / 60f + 0.01f)
                {
                    Debug.Log("bail out");
                    return;
                }
                if (isSettled)
                {
                    velocity.y = 0;
                    lastTime = Time.time;
                    return;
                }

                if (!isSettled)
                {
                    if (ticksBounce < bounceCount)
                    {
                        velocity.y = bounceVelocity;
                        ticksBounce++;
                        isGrounded = false;
                        gravityOffset += gravity * bounceCoefficient;
                    }
                    else
                    {
                        gravityOffset = 0;
                        ticksBounce = 0;
                        velocity.y = 0;
                        isSettled = true;
                    }
                }

                lastTime = Time.time;
            }

            if (Vector2.Dot(Vector2.left, p.normal) > CoreConstants.THRESHOLD_DOT_LOOSE)
            {
                velocity.x = -10;
            }
        }
    }
}
