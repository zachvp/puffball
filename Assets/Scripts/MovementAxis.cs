using System;
using UnityEngine;

public class MovementAxis : MonoBehaviour
{
    public Rigidbody2D body;

    public void MoveX(float velocity)
    {
        var v = body.velocity;
        v.x = velocity;
        body.velocity = v;
    }

    public void MoveY(float velocity)
    {
        var v = body.velocity;
        v.y = velocity;
        body.velocity = v;
    }
}
