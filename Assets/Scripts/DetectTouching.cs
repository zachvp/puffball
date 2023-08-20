using UnityEngine;

public class DetectTouching : MonoBehaviour
{
    // todo: consider moving this group to a struct
    public LayerMask mask;
    public Collider2D target;
    public Collider2D[] overlappingObjects = new Collider2D[1];

    public bool isBelow;
    public Collision2D key;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        var contacts = new ContactPoint2D[1];

        foreach (var c in contacts)
        {
            Debug.Log(c.normal);

            if (Vector2.Dot(Vector2.up, c.normal) > CoreConstants.THRESHOLD_DOT_PRECISE)
            {
                isBelow = true;
                key = collision;
                overlappingObjects[0] = collision.collider;
            }
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.Equals(key))
        {
            isBelow = false;
            key = null;
        }
    }
}
