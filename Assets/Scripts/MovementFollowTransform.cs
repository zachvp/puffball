using UnityEngine;

public class MovementFollowTransform : MonoBehaviour
{
    public Transform root;
    public bool followX = true;
    public bool followY = true;
    public bool followZ = true;
    public Vector3 offset;
    public bool usePhysics;

    [CoreConditional(nameof(usePhysics))]
    public CoreBody body;

    public void LateUpdate()
    {
        var modPosition = transform.position;

        if (followX)
        {
            modPosition.x = root.position.x;
        }
        if (followY)
        {
            modPosition.y = root.position.y;
        }
        if (followZ)
        {
            modPosition.z = root.position.z;
        }

        modPosition += offset;

        if (usePhysics)
        {
            body.position = modPosition;
        }
        else
        {
            transform.position = modPosition;
        }
    }
}
