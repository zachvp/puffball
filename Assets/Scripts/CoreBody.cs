using UnityEngine;
using UnityEngine.Events;

// split into separate 'PlatformBody' class?
public class CoreBody : MonoBehaviour
{
    public Rigidbody2D body;
    public float originalGravity;
    public RigidbodyType2D originalType;
    public UnityAction<Collider2D> OnAnyColliderEnter;
    
    public Vector2 velocity { get { return body.velocity; } set { Trigger(value); } }
    public float rotation { get { return body.rotation; } set { body.rotation = value; } }
    public Vector2 position { get { return body.position; } set { body.position = value; } }

    public void Awake()
    {
        originalGravity = body.gravityScale;
    }

    // -- MOVEMENT
    public void Trigger(Vector2 value)
    {
        StartCoroutine(CoreUtilities.PostFixedUpdateTask(() =>
        {
            body.velocity = value;
        }));
    }

    public void TriggerX(float value)
    {
        StartCoroutine(CoreUtilities.PostFixedUpdateTask(() =>
        {
            var result = body.velocity;
            result.x = value;

            body.velocity = result;
        }));
    }

    public void TriggerY(float value)
    {
        StartCoroutine(CoreUtilities.PostFixedUpdateTask(() =>
        {
            var result = body.velocity;
            result.y = value;

            body.velocity = result;
        }));
    }

    // -- STATE CONTROL
    public void StopVertical()
    {
        StartCoroutine(CoreUtilities.PostFixedUpdateTask(() =>
        {
            var newVelocity = body.velocity;

            newVelocity.y = 0;
            body.gravityScale = 0;

            body.velocity = newVelocity;
        }));
    }

    public void ResetVertical()
    {
        StartCoroutine(CoreUtilities.PostFixedUpdateTask(() =>
        {
            body.gravityScale = originalGravity;
        }));
    }

    public void FreezeRotation()
    {
        body.freezeRotation = true;
    }

    public void UnfreezeRotation()
    {
        body.freezeRotation = false;
    }

    // -- COLLISIONS
    public void OnCollisionEnter2D(Collision2D c)
    {
        if (OnAnyColliderEnter != null)
        {
            OnAnyColliderEnter(c.collider);
        }
    }

    public void OnTriggerEnter2D(Collider2D c)
    {
        if (OnAnyColliderEnter != null)
        {
            OnAnyColliderEnter(c);
        }
    }
}
