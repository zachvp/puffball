using System;
using UnityEngine;

public class MovementAxis : MonoBehaviour
{
    public Rigidbody2D body;

    public void VelocityX(float velocity)
    {
        var v = body.velocity;
        v.x = velocity;
        body.velocity = v;
    }

    public void VelocityY(float velocity)
    {
        var v = body.velocity;
        v.y = velocity;
        body.velocity = v;
    }
}
