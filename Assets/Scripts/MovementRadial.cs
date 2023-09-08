using UnityEngine;
using System;

public class MovementRadial : MonoBehaviour
{
    public Transform anchor;
    public Transform target;

    public float range;
    public Vector3 offset;

    public bool usePhysics;
    [NonSerialized]
    public Rigidbody2D body;

    public void Awake()
    {
        if (usePhysics)
        {
            body = target.GetComponent<Rigidbody2D>();
        }

        Debug.Assert(body == usePhysics, $"mismatch between body reference and configuration");
    }

    public Vector3 Move(Vector2 input)
    {
        var finalAnchorPos = anchor.position + offset;
        var newPos = (Vector2) finalAnchorPos + (input * range);

        if (usePhysics)
        {
            body.position = newPos;
        }
        else
        {
            target.position = newPos;
        }

        return newPos;
    }

    public Vector3 ResetState()
    {
        if (usePhysics)
        {
            body.position = anchor.position;
        }
        else
        {
            target.position = anchor.position;
        }

        return anchor.position;
    }
}
