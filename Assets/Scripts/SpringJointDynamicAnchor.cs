using UnityEngine;

public class SpringJointDynamicAnchor : MonoBehaviour
{
    public SpringJoint2D joint;
    public Transform anchor;

    public Vector2 initialConnectedAnchorPos;

    public void Start()
    {
        initialConnectedAnchorPos = joint.connectedAnchor;
    }

    public void FixedUpdate()
    {
        joint.connectedAnchor = anchor.position;
    }
}
