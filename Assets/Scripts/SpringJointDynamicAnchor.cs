using UnityEngine;

public class SpringJointDynamicAnchor : MonoBehaviour
{
    public SpringJoint2D spring;
    public Transform anchor;

    public Vector2 initialConnectedAnchorPos;

    public void Start()
    {
        initialConnectedAnchorPos = spring.connectedAnchor;
    }

    public void FixedUpdate()
    {
        spring.connectedAnchor = anchor.position;
    }
}
