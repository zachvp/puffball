using System;
using UnityEngine;

public class CoreBodyHybrid : MonoBehaviour, IBody
{
    // -- links
    public Rigidbody2D body;
    public CoreSignalsCollision signals;

    // -- properties
    public Vector2 velocity { get { return _velocity; } set { Velocity(value); } }
    public float rotation { get { return body.rotation; } set { body.rotation = value; } }
    public Vector2 position { get { return body.position; } set { body.position = value; } }

    // -- config
    public float gravity = 1; // todo: move to scriptable object
    public float originalGravity; // todo: move to scriptable object

    // -- state
    public Vector2 _velocity;

    public void Awake()
    {
        Debug.Assert(body != null, $"expected {nameof(Rigidbody2D)} to be attached");
        Debug.Assert(body.bodyType == RigidbodyType2D.Dynamic, $"expected attached {nameof(Rigidbody2D)} to be {nameof(RigidbodyType2D.Dynamic)}");

        originalGravity = gravity;
    }

    #region IBody

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
        VelocityY(0);
    }

    public void ToggleRotationFreeze(bool value)
    {
        body.freezeRotation = value;
    }

    public void Velocity(Vector2 value)
    {
        _velocity = value;
    }

    public void VelocityX(float value)
    {
        _velocity.x = value;
    }

    public void VelocityY(float value)
    {
        _velocity.y = value;
    }

    #endregion
}
