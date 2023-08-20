using UnityEngine;
using System;

public class MovementFollowTransform : MonoBehaviour
{
    public FollowType type;
    public Transform anchor;
    public Vector3 offset;
    public bool usePhysics;

    [CoreConditional(nameof(usePhysics))]
    public CoreBody body;

    // todo: add core conditional support for FollowType
    public float time = 1f;

    [NonSerialized]
    public float t;

    public void Update()
    {
        switch (type)
        {
            case FollowType.SNAP:
                SnapUpdate();
                break;
            case FollowType.LAG:
                LagUpdate();
                break;
        }
    }

    public void LagUpdate()
    {
        var toAnchor = anchor.position - transform.position; // TODO: FOOT

        if (toAnchor.sqrMagnitude < CoreConstants.DEADZONE_FLOAT)
        {
            UpdatePosition(anchor.position);
            t = 0;
        }
        else
        {
            var modPos = Vector3.Lerp(transform.position, anchor.position, Mathf.Min(1, t / time));

            t += Time.deltaTime;

            UpdatePosition(modPos);
        }
    }

    public void SnapUpdate()
    {
        UpdatePosition(anchor.position + offset);
    }

    public void UpdatePosition(Vector3 position)
    {
        if (usePhysics)
        {
            body.position = position;
        }
        else
        {
            transform.position = position;
        }
    }

    public enum FollowType
    {
        SNAP,
        LAG
    }
}
