using UnityEngine;

public class MovementRadial : MonoBehaviour
{
    public Transform anchor;
    public Transform target;
    public float range;

    public Vector3 Move(Vector2 input)
    {
        var inputVector3 = new Vector3(input.x, input.y, 0);
        var newPos = anchor.position + (inputVector3 * range);

        target.position = newPos;

        return newPos;
    }

    public Vector3 ResetPosition()
    {
        target.position = anchor.position;

        return anchor.position;
    }
}
