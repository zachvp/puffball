using System;
using UnityEngine;

// todo: remove
public class CoreBodyHybrid : MonoBehaviour
{
    // -- links
    public Rigidbody2D body;

    [NonSerialized]
    public CoreSignalsCollision signals;

    // -- properties
    public Vector2 velocity;
    public float rotation { get { return body.rotation; } set { body.rotation = value; } }
    public Vector2 position { get { return body.position; } set { body.position = value; } }

    // -- config // todo: move to scriptable object
    public float stepCoefficient = 0.5f;
    public float gravity = 1; 
    public float originalGravity;

    public void Awake()
    {
        Debug.Assert(body != null, $"expected {nameof(Rigidbody2D)} to be attached");
        Debug.Assert(body.bodyType == RigidbodyType2D.Dynamic, $"expected attached {nameof(Rigidbody2D)} to be {nameof(RigidbodyType2D.Dynamic)}");

        originalGravity = gravity;

        // todo: simply add the component
        signals = GetComponent<CoreSignalsCollision>();

        if (signals = null)
        {
            signals = gameObject.AddComponent<CoreSignalsCollision>();
        }

        //Debug.LogFormat();
    }

    public void MoveBasedOnVelocity()
    {
        var newPos = body.position;
        newPos += velocity * (Time.fixedDeltaTime * stepCoefficient);

        newPos = CoreUtilities.RoundTo(newPos, CoreConstants.UNIT_ROUND_POSITION);

        Move(newPos);
    }

    public void Move(Vector2 position)
    {
        body.MovePosition(position);
    }

    public void ResetVertical()
    {
        gravity = originalGravity;
    }

    public void StopVertical()
    {
        gravity = 0;
        velocity.y = 0;
    }

    public void ToggleRotationFreeze(bool value)
    {
        body.freezeRotation = value;
    }
}