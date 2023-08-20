using UnityEngine;
using System.Collections.Generic;

public class DetectTouching : MonoBehaviour
{
    // todo: consider moving this group to a struct
    public LayerMask mask;
    public Collider2D target;
    public Collider2D[] overlappingObjects = new Collider2D[1];

    public bool isBelow;
    public HashSet<Collision2D> keys = new HashSet<Collision2D>();

    public bool Check(Collider2D c)
    {
        var r = Physics2D.CircleCast(c.bounds.center, 0.5f, Vector2.down, 10, mask);

        return isBelow = r.collider != null; ;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        var contacts = new ContactPoint2D[1];

        collision.GetContacts(contacts);

        foreach (var c in contacts)
        {
            //Debug.Log(c.normal);

            if (c.normal.y > 0.9f)
            {
                isBelow = true;
                Debug.Log("is below");
                keys.Add(collision);
                overlappingObjects[0] = collision.collider;
            }
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (keys.Contains(collision))
        {
            Debug.Log("left below");
            keys.Remove(collision);
            isBelow = keys.Count > 0;
        }
    }
}
