using UnityEngine;
using System;

public class JointDynamicAnchor : MonoBehaviour
{
    public Transform anchor;

    public AnchoredJoint2D joint;
    public TargetJoint2D relative;

    public void Awake()
    {
        Debug.Assert(joint || relative, "no joint linked");
    }

    public void FixedUpdate()
    {
        if (joint)
        {
            joint.connectedAnchor = anchor.position;
        }
        if (relative)
        {
            relative.target = anchor.position;
        }
    }

    public void OnDisable()
    {
        if (joint)
        {
            joint.enabled = false;
        }
        if (relative)
        {
            relative.enabled = false;
        }
    }

    public void OnEnable()
    {
        if (relative)
        {
            relative.enabled = true;

        }
        if (joint)
        {
            joint.enabled = true;
        }
    }
}
