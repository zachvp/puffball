using UnityEngine;
using System;

public class JointDynamicAnchor : MonoBehaviour
{
    [CoreConditional(nameof(distance))]
    public SpringJoint2D spring;

    [CoreConditional(nameof(spring))]
    public DistanceJoint2D distance;

    public Transform anchor;

    [NonSerialized]
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
