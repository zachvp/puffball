using UnityEngine;
using static Unity.VisualScripting.Member;

#if UNITY_EDITOR

[Tooltip("Use to generate a collider on a target GameObject, then assign that collider to this GameObject.")]
public class AssignSpriteCollider : MonoBehaviour
{
    [Tooltip("The other GameObject that is the collider's source.")]
    public GameObject other;

    [Tooltip("The type of collider to assign")]
    public ColliderType type;

    // todo: select collider type
    // todo: traverse hierarchy to find sprite collider

    public void GenerateAndAssignCollider()
    {
        Debug.Log("fired, generating collider");

        Debug.Assert(transform.position == Vector3.zero, "current transform is non-zero, collider offset will likely be innacurate");

        switch (type)
        {
            case ColliderType.CIRCLE:
                ApplyCircle(other);
                break;
            case ColliderType.POLYGON:
                ApplyPolygon(other);
                break;
            default:
                Debug.LogError($"unhandled collider type: {type}");
                break;
        }
    }

    public void ApplyCircle(GameObject other)
    {
        RemoveAllAttachedColliders(other);
        RemoveAllAttachedColliders(gameObject);

        var generated = other.AddComponent<CircleCollider2D>();
        var attached = gameObject.AddComponent<CircleCollider2D>();

        CoreUtilities.CopyCollider(generated, attached);
        DestroyImmediate(generated);
    }

    public void ApplyPolygon(GameObject other)
    {
        RemoveAllAttachedColliders(other);
        RemoveAllAttachedColliders(gameObject);

        var generated = other.AddComponent<PolygonCollider2D>();
        var attached = gameObject.AddComponent<PolygonCollider2D>();

        CoreUtilities.CopyCollider(generated, attached);
        DestroyImmediate(generated);
    }

    public void RemoveAllAttachedColliders(GameObject gameObject)
    {
        var all = gameObject.GetComponents<Collider2D>();

        foreach (var c in all)
        {
            DestroyImmediate(c);
        }
    }

    public enum ColliderType
    {
        POLYGON,
        CIRCLE,
        BOX,
        CAPSULE
    }
}

#endif