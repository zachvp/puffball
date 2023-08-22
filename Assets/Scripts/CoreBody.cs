using UnityEngine;

[RequireComponent(typeof(CoreSignalsCollision))]
public class CoreBody : MonoBehaviour, IBody
{
    // Links
    public Rigidbody2D body;

    // One-time write state.
    public float originalGravity;

    // Action events
    public CoreSignalsCollision signals;

    // Properties
    public Vector2 velocity { get { return body.velocity; } set { Velocity(value); } }
    public float rotation { get { return body.rotation; } set { body.rotation = value; } }
    public Vector2 position { get { return body.position; } set { Move(value); } }

    public void Awake()
    {
        originalGravity = body.gravityScale;
    }

    #region IBody

    // -- movement
    public void Velocity(Vector2 value)
    {
        StartCoroutine(CoreUtilities.TaskFixedUpdate(() =>
        {
            body.velocity = value;
        }));
    }

    // todo: remove this and VelocityY; callers should use utility method instead to update Vector components.
    public void VelocityX(float value)
    {
        StartCoroutine(CoreUtilities.TaskFixedUpdate(() =>
        {
            var result = body.velocity;
            result.x = value;

            body.velocity = result;
        }));
    }

    public void VelocityY(float value)
    {
        StartCoroutine(CoreUtilities.TaskFixedUpdate(() =>
        {
            var result = body.velocity;
            result.y = value;

            body.velocity = result;
        }));
    }

    public void Move(Vector2 position)
    {
        body.position = position;
    }

    public void MoveKinematic(Vector2 position)
    {
        body.MovePosition(position);
    }

    public void StopVertical()
    {
        StartCoroutine(CoreUtilities.TaskFixedUpdate(() =>
        {
            body.gravityScale = 0;
            VelocityY(0);
        }));
    }

    public void ResetVertical()
    {
        StartCoroutine(CoreUtilities.TaskFixedUpdate(() =>
        {
            body.gravityScale = originalGravity;
        }));
    }

    public void ToggleRotationFreeze(bool value)
    {
        body.freezeRotation = value;
    }

    #endregion
}
