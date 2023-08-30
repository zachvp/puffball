using System;
using UnityEngine;

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

    // -- config
    public float stepCoefficient = 0.5f;
    public float gravity = 1; // todo: move to scriptable object
    public float originalGravity; // todo: move to scriptable object

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

    // todo: remove to enforce only velocity used?
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