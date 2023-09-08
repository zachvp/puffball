using UnityEngine;
using System;

public class MovementFollowTransform : MonoBehaviour
{
    public FollowType type;
    public Transform anchor;
    public Vector3 offset;
    public bool usePhysics;

    [CoreConditional(nameof(usePhysics))]
    public Rigidbody2D body;

    [CoreConditional("type", FollowType.LAG)]
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

    public void OnEnable()
    {
        enabled = true;
        Debug.Log($"enabled: {enabled}");
    }

    public void OnDisable()
    {
        enabled = false;
        Debug.Log($"enabled: {enabled}");
    }

    public void LagUpdate()
    {
        var toAnchor = anchor.position - transform.position;

        if (toAnchor.sqrMagnitude < CoreConstants.DEADZONE_FLOAT_1)
        {
            UpdatePosition(anchor.position);
            t = 0;
        }
        else
        {
            var modPos = Vector3.Lerp(transform.position, anchor.position, Mathf.Min(1, t / time));

            if (usePhysics)
            {
                t += Time.fixedDeltaTime;
            }
            else
            {
                t += Time.deltaTime;
            }

            UpdatePosition(modPos + offset);
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
            StartCoroutine(CoreUtilities.TaskFixedUpdate(() =>
            {
                body.position = position;
            }));

            //body.MoveKinematic(position);
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
