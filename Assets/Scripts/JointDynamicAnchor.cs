using UnityEngine;
using System;

public class JointDynamicAnchor : MonoBehaviour
{
    public Transform anchor;

    public AnchoredJoint2D joint;
    public TargetJoint2D target;
    public RelativeJoint2D relative;

    public void Update()
    {
        if (joint)
        {
            joint.connectedAnchor = anchor.position;
        }
        if (target)
        {
            target.target = anchor.position;
        }
    }

    public void OnDisable()
    {
        if (joint)
        {
            joint.enabled = false;
        }
        if (target)
        {
            target.enabled = false;
        }
        if (relative)
        {
            relative.enabled = false;
        }
    }

    public void OnEnable()
    {
        if (target)
        {
            target.enabled = true;

        }
        if (joint)
        {
            joint.enabled = true;
        }
        if (relative)
        {
            relative.enabled = true;
        }
    }
}
