using UnityEngine;

public class SpringJointDynamicAnchor : MonoBehaviour
{
    public SpringJoint2D spring;
    public DistanceJoint2D distance;
    public Transform anchor;

    public Vector2 initialConnectedAnchorPos;

    public void Start()
    {
        if (spring)
        {
            initialConnectedAnchorPos = spring.connectedAnchor;
        }
        else if (distance)
        {
            initialConnectedAnchorPos = distance.connectedAnchor;
        }

        Debug.Assert(spring || distance, $"no joints are configured, this script will do nothing");
    }

    public void FixedUpdate()
    {
        if (spring)
        {
            spring.connectedAnchor = anchor.position;
        }
        else if (distance)
        {
            distance.connectedAnchor = anchor.position;
        }
    }
}
