using UnityEngine;

public class SpringJointDynamicAnchor : MonoBehaviour
{
    public SpringJoint2D joint;
    public Transform anchor;

    public void FixedUpdate()
    {
        joint.connectedAnchor = anchor.position;
    }
}
