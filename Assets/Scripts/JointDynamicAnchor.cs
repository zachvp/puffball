using UnityEngine;
using System;

public class JointDynamicAnchor : MonoBehaviour
{
    // todo: extend coreConditional to support more args
    [CoreConditional(nameof(distance))]
    public SpringJoint2D spring;

    [CoreConditional(nameof(spring))]
    public DistanceJoint2D distance;

    [CoreConditional(nameof(spring))]
    public FixedJoint2D fixedJoint;

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
        else if (fixedJoint)
        {
            initialConnectedAnchorPos = fixedJoint.connectedAnchor;
        }

        Debug.Assert(spring || distance || fixedJoint, $"no joints are configured, this script will do nothing");
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
        else if (fixedJoint)
        {
            fixedJoint.connectedAnchor = anchor.position;
        }
    }
}
