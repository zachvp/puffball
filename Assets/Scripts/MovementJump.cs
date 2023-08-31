using System;
using UnityEngine;

public class MovementJump : MonoBehaviour
{
    public Rigidbody2D body;

    public void Jump(float velocity)
    {
        var v = body.velocity;
        v.y = velocity;

        body.velocity = v;
    }
}
