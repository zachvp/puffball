using UnityEngine;

public class MovementRadial : MonoBehaviour
{
    public DistanceJoint2D joint;
    public Transform anchor;
    public Transform target;
    public float range;
    public Vector2 scale = Vector2.one;

    public Vector3 Move(Vector2 input)
    {
        joint.enabled = true;

        var inputVector3 = new Vector3(input.x, input.y, 0);
        var newPos = anchor.position + (inputVector3 * range);

        //target.position = newPos;
        joint.connectedAnchor = newPos;
        //joint.distance = Vector3.Distance(anchor.position, newPos);

        Debug.DrawLine(anchor.position, newPos, Color.red, 4);

        return newPos;
    }

    public Vector3 ResetPosition()
    {
        joint.enabled = false;
        joint.connectedAnchor = anchor.position;
        //target.position = anchor.position;

        return anchor.position;
    }
}
