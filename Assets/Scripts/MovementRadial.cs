using UnityEngine;

public class MovementRadial : MonoBehaviour
{
    public Transform root;
    public float range;

    public void Trigger(Vector2 input)
    {
        var inputVector3 = new Vector3(input.x, input.y, 0);

        transform.position = root.position + (inputVector3 * range);
    }
}
