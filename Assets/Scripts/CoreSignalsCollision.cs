using System;
using UnityEngine;

public class CoreSignalsCollision : MonoBehaviour
{
    public Action<Collider2D> onAnyColliderEnter;
    public Action<Collision2D> onCollisionBodyEnter;
    public Action<Collision2D> onCollisionBodyExit;
    public Action<Collision2D> onCollisionBodyStay;


    // -- COLLISIONS
    public void OnCollisionStay2D(Collision2D c)
    {
        //Debug.Log($"stay collision:  {c.gameObject.name}");
        Emitter.Send(onCollisionBodyStay, c);
    }

    public void OnCollisionEnter2D(Collision2D c)
    {
        //Debug.Log("core body: on collision");

        Emitter.Send(onAnyColliderEnter, c.collider);
        Emitter.Send(onCollisionBodyEnter, c);
    }

    public void OnCollisionExit2D(Collision2D c)
    {
        Emitter.Send(onCollisionBodyExit, c);
    }

    public void OnTriggerEnter2D(Collider2D c)
    {
        Emitter.Send(onAnyColliderEnter, c);
    }
}
