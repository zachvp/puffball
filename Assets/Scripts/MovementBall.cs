using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovementBall : MonoBehaviour
{
    public CoreBody body;
    public Collider2D attachedCollider;
    public Dictionary<Collider2D, Collision2D> collisionMap = new Dictionary<Collider2D, Collision2D>();
    public Vector2 velocity;
    public float gravity;
    public float bounceCoefficient = 0.5f;
    public float settleThreshold = 1;

    public bool isGrounded;
    public Vector2 newPos;
    public Vector2 prevPos;
    public int repeatCount = int.MaxValue;
    public int countThreshold = 20;

    public void Start()
    {
        body.signals.onCollisionBodyEnter += HandleCollisionBodyEnter;
        body.signals.onCollisionBodyExit += HandleCollisionBodyExit;
        //body.onCollisionBodyStay += HandleCollisionBodyStay;

        attachedCollider = GetComponent<Collider2D>();
    }

    public void Update()
    {
        
    }

    public void FixedUpdate()
    {
        //if (prevPos.y == body.position.y)
        //{
        //    if (repeatCount < int.MaxValue && repeatCount > countThreshold)
        //    {
        //        Debug.Log($"newPos == bodyPos; repeated");
        //        velocity.y = 0;
        //    }
        //    else
        //    {
        //        repeatCount++;
        //    }
        //}

        //var results = new Collider2D[1];
        //var count = attachedCollider.OverlapCollider(new ContactFilter2D(), results);

        if (collisionMap.Count == 0)
        {
            velocity.y -= gravity * Time.fixedDeltaTime;
        }
        else
        {
            //velocity.y = 0;
            
        }

        // Update body to new velocity-driven position.
        newPos = body.position;
        newPos += velocity * (Time.fixedDeltaTime);
        prevPos = body.position;
        body.MoveKinematic(newPos);
    }

    public Vector2 CalcBounceVelocity(Collision2D collision, float coefficient)
    {
        var contacts = new ContactPoint2D[4];
        var count = collision.GetContacts(contacts);
        //var bounceVelocity = -velocity/2 * coefficient;
        //var bounceVelocity = -velocity * coefficient;
        var bounceVelocity = Vector2.zero;

        for (var i = 0; i < count; i++)
        {
            var c = contacts[i];

            bounceVelocity += c.normal * velocity.magnitude * coefficient;
        }

        return bounceVelocity;
    }

    public void HandleCollisionBodyEnter(Collision2D collision)
    {
        repeatCount = 0;
        collisionMap.Add(collision.collider, collision);

        var bounce = CalcBounceVelocity(collision, bounceCoefficient);

        if (Mathf.Abs(bounce.y * Time.fixedDeltaTime) < settleThreshold)
        {
            velocity.y = 0;
        }
        else
        {
            velocity = bounce;
        }
    }

    public void HandleCollisionBodyExit(Collision2D collision)
    {
        //var contacts = new ContactPoint2D[4];
        //Debug.Log($"exit contacts: {collision.contactCount}");
        collisionMap.Remove(collision.collider);
    }
}
