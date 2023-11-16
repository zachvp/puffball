using System;
using UnityEngine;

namespace ZCore
{
    public class CoreSignalsCollision : MonoBehaviour
    {
        public Action<Collider2D> onAnyColliderEnter;
        public Action<Collision2D> onCollisionBodyEnter;
        public Action<Collision2D> onCollisionBodyExit;
        public Action<Collision2D> onCollisionBodyStay;

        // -- collision events
        public void OnCollisionStay2D(Collision2D c)
        {
            Emitter.Send(onCollisionBodyStay, c);
        }

        public void OnCollisionEnter2D(Collision2D c)
        {
            Emitter.Send(onAnyColliderEnter, c.collider);
            Emitter.Send(onCollisionBodyEnter, c);
        }

        public void OnCollisionExit2D(Collision2D c)
        {
            Emitter.Send(onCollisionBodyExit, c);
        }

        // -- trigger events
        public void OnTriggerEnter2D(Collider2D c)
        {
            Emitter.Send(onAnyColliderEnter, c);
        }
    }
}