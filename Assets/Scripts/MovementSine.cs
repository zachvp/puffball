using UnityEngine;

public class MovementSine : MonoBehaviour
{
    public Transform origin;
    public short scale = 2;
    public float increment = 0.05f;

    public float current;
    public float velocity = 1;
    public float smoothTime = 0.1f;

    public void FixedUpdate()
    {
        var position = origin.position;

        position.x = position.x + Mathf.Sin(current) * scale;
        transform.position = position;

        // Update current.
        current += increment % (2 * Mathf.PI);
    }

    public void Reset()
    {
        current = 0;
        transform.position = origin.position;
    }
}
