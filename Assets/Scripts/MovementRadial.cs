using UnityEngine;

public class MovementRadial : MonoBehaviour
{
    public Transform root;
    public Transform target;
    public float range;

    public Vector3 Move(Vector2 input)
    {
        var inputVector3 = new Vector3(input.x, input.y, 0);
        var newPos = root.position + (inputVector3 * range);

        target.position = newPos;

        return newPos;
    }

    public Vector3 ResetPosition()
    {
        target.position = root.position;

        return root.position;
    }
}
