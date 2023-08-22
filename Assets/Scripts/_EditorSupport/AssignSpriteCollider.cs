#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

[Tooltip("Use to generate a collider on a target GameObject, then assign that collider to this GameObject.")]
public class AssignSpriteCollider : MonoBehaviour
{
    [Tooltip("The other GameObject that is the collider's source.")]
    public GameObject other;

    [Tooltip("The type of collider to assign")]
    public ColliderType type;

    [Tooltip("Set this to true to use the existing collider data on 'other', rather than creating one from scratch.")]
    public bool useExisting;

    public void GenerateAndAssignCollider()
    {
        Debug.Assert(transform.position == Vector3.zero, "current transform is non-zero, collider offset will likely be innacurate");

        // Validation
        if (other == null)
        {
            EditorUtility.DisplayDialog($"Error: {nameof(GenerateAndAssignCollider)}",
                $"'{nameof(GameObject)}:{nameof(other)}' is null. Please assign before generation. Script will halt now.",
                "Close");
            return;
        }
        if (useExisting && other.GetComponent<Collider2D>() == null)
        {
            EditorUtility.DisplayDialog($"Error: {nameof(GenerateAndAssignCollider)}",
                $"Script is configured to '{nameof(useExisting)}={useExisting}', " +
                $"but there is no Collider2D component on '{nameof(GameObject)}:{other.name}'. Script will halt now.",
                "Close");

            return;
        }

        // Assign collider based on configured type.
        switch (type)
        {
            case ColliderType.CIRCLE:
                ApplyCircle(other);
                break;
            case ColliderType.POLYGON:
                ApplyPolygon(other);
                break;
            case ColliderType.CAPSULE:
                ApplyCapsule(other);
                break;
            default:
                Debug.LogError($"unhandled collider type: {type}");
                break;
        }
    }

    public void ApplyCircle(GameObject other)
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
    }

    public void ApplyPolygon(GameObject other)
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
    }

    public void ApplyCapsule(GameObject other)
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