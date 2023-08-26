#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

[Tooltip("Use to generate a collider on a target GameObject, then assign that collider to this GameObject.")]
public class AssignSpriteCollider : MonoBehaviour
{
    [Tooltip("The other GameObject that is the collider's source.")]
    public GameObject source;

    [Tooltip("The type of collider to assign")]
    public ColliderType type;

    [Tooltip("Set this to true to use the existing collider data on 'other', rather than creating one from scratch.")]
    public bool useExisting;

    [Tooltip("Set this to false to use zero offset for the generated collider.")]
    public bool useSourceOffset = true;

    public void GenerateAndAssignCollider()
    {
        Debug.Assert(transform.position == Vector3.zero, "current transform is non-zero, collider offset will likely be innacurate");

        // Validation
        if (source == null)
        {
            EditorUtility.DisplayDialog($"Error: {nameof(GenerateAndAssignCollider)}",
                $"'{nameof(GameObject)}:{nameof(source)}' is null. Please assign before generation. Script will halt now.",
                "Close");
            return;
        }
        if (useExisting && source.GetComponent<Collider2D>() == null)
        {
            EditorUtility.DisplayDialog($"Error: {nameof(GenerateAndAssignCollider)}",
                $"Script is configured to '{nameof(useExisting)}={useExisting}', " +
                $"but there is no Collider2D component on '{nameof(GameObject)}:{source.name}'. Script will halt now.",
                "Close");

            return;
        }

        Collider2D attachedCollider = null;

        // Assign collider based on configured type.
        switch (type)
        {
            case ColliderType.CIRCLE:
                attachedCollider = ApplyCircle(source);
                break;
            case ColliderType.POLYGON:
                attachedCollider = ApplyPolygon(source);
                break;
            case ColliderType.CAPSULE:
                attachedCollider = ApplyCapsule(source);
                break;
            default:
                Debug.LogError($"unhandled collider type: {type}");
                break;
        }

        if (!useSourceOffset && attachedCollider)
        {
            attachedCollider.offset = Vector2.zero;
        }
    }

    public Collider2D ApplyCircle(GameObject other)
    {
        CircleCollider2D original;

        RemoveAllAttachedColliders(gameObject);

        if (useExisting)
        {
            original = other.GetComponent<CircleCollider2D>();
        }
        else
        {
            RemoveAllAttachedColliders(other);
            original = other.AddComponent<CircleCollider2D>();
        }
        var attached = gameObject.AddComponent<CircleCollider2D>();

        CoreUtilities.CopyCollider(original, attached);

        if (!useExisting)
        {
            DestroyImmediate(original);
        }

        return attached;
    }

    public Collider2D ApplyPolygon(GameObject other)
    {
        PolygonCollider2D original;

        RemoveAllAttachedColliders(gameObject);

        if (useExisting)
        {
            original = other.GetComponent<PolygonCollider2D>();
        }
        else
        {
            RemoveAllAttachedColliders(other);
            original = other.AddComponent<PolygonCollider2D>();
        }
        var attached = gameObject.AddComponent<PolygonCollider2D>();

        CoreUtilities.CopyCollider(original, attached);

        if (!useExisting)
        {
            DestroyImmediate(original);
        }

        return attached;
    }

    public Collider2D ApplyCapsule(GameObject other)
    {
        CapsuleCollider2D original;

        RemoveAllAttachedColliders(gameObject);

        if (useExisting)
        {
            original = other.GetComponent<CapsuleCollider2D>();
        }
        else
        {
            RemoveAllAttachedColliders(other);
            original = other.AddComponent<CapsuleCollider2D>();
        }
        var attached = gameObject.AddComponent<CapsuleCollider2D>();

        CoreUtilities.CopyCollider(original, attached);

        if (!useExisting)
        {
            DestroyImmediate(original);
        }

        return attached;
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