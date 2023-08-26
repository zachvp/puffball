#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System;

[Tooltip("Use to generate a collider on a target GameObject, then assign that collider to this GameObject.")]
public class AssignSpriteCollider : MonoBehaviour
{
    public SpriteColliderConfig[] config = new SpriteColliderConfig[1];

    public void GenerateAndAssignCollider()
    {
        foreach (var c in config)
        {
            GenerateAndAssignCollider(c);
        }

        EditorUtility.DisplayDialog("Generation complete", "Finished generating colliders.", "Close");
    }

    public void GenerateAndAssignCollider(SpriteColliderConfig config)
    {
        Debug.Assert(transform.position == Vector3.zero, "current transform is non-zero, collider offset will likely be innacurate");

        // Validation
        if (config.source == null)
        {
            EditorUtility.DisplayDialog($"Error: {nameof(GenerateAndAssignCollider)}",
                $"'{nameof(GameObject)}:{nameof(config.source)}' is null. Please assign before generation. Script will halt now.",
                "Close");
            return;
        }
        if (config.useExisting && config.source.GetComponent<Collider2D>() == null)
        {
            EditorUtility.DisplayDialog($"Error: {nameof(GenerateAndAssignCollider)}",
                $"Script is configured to '{nameof(config.useExisting)}={config.useExisting}', " +
                $"but there is no Collider2D component on '{nameof(GameObject)}:{config.source.name}'. Script will halt now.",
                "Close");

            return;
        }

        Collider2D attachedCollider = null;

        // Assign collider based on configured type.
        switch (config.type)
        {
            case ColliderType.CIRCLE:
                //attachedCollider = ApplyCircle(config);
                attachedCollider = ApplyCircleNew(config);
                break;
            case ColliderType.POLYGON:
                attachedCollider = ApplyPolygon(config);
                break;
            case ColliderType.CAPSULE:
                attachedCollider = ApplyCapsule(config);
                break;
            default:
                Debug.LogError($"unhandled collider type: {config.type}");
                break;
        }

        if (!config.useSourceOffset && attachedCollider)
        {
            attachedCollider.offset = Vector2.zero;
        }
    }

    public Collider2D ApplyCircleNew(SpriteColliderConfig config)
    {
        var circle = ApplyShape<CircleCollider2D>(config, (original, attached) =>
        {
            CoreUtilities.CopyCollider(original, attached);
        });

        return circle;
    }

    public Collider2D ApplyCircle(SpriteColliderConfig config)
    {
        CircleCollider2D original;

        RemoveAllAttachedColliders(config.target);

        if (config.useExisting)
        {
            original = config.source.GetComponent<CircleCollider2D>();
        }
        else
        {
            RemoveAllAttachedColliders(config.source);
            original = config.source.AddComponent<CircleCollider2D>();
        }
        var attached = config.target.AddComponent<CircleCollider2D>();

        CoreUtilities.CopyCollider(original, attached);

        if (!config.useExisting)
        {
            DestroyImmediate(original);
        }

        return attached;
    }

    public Collider2D ApplyPolygon(SpriteColliderConfig config)
    {
        PolygonCollider2D original;

        RemoveAllAttachedColliders(config.target);

        if (config.useExisting)
        {
            original = config.source.GetComponent<PolygonCollider2D>();
        }
        else
        {
            RemoveAllAttachedColliders(config.source);
            original = config.source.AddComponent<PolygonCollider2D>();
        }
        var attached = config.target.AddComponent<PolygonCollider2D>();

        CoreUtilities.CopyCollider(original, attached);

        if (!config.useExisting)
        {
            DestroyImmediate(original);
        }

        return attached;
    }

    public Collider2D ApplyCapsule(SpriteColliderConfig config)
    {
        CapsuleCollider2D original;

        RemoveAllAttachedColliders(config.target);

        if (config.useExisting)
        {
            original = config.source.GetComponent<CapsuleCollider2D>();
        }
        else
        {
            RemoveAllAttachedColliders(config.source);
            original = config.source.AddComponent<CapsuleCollider2D>();
        }
        var attached = config.target.AddComponent<CapsuleCollider2D>();

        CoreUtilities.CopyCollider(original, attached);

        if (!config.useExisting)
        {
            DestroyImmediate(original);
        }

        return attached;
    }

    public T ApplyShape<T>(SpriteColliderConfig config, Action<T, T> copy) where T : Collider2D
    {
        T original;

        RemoveAllAttachedColliders(config.target);

        if (config.useExisting)
        {
            original = config.source.GetComponent<T>();
        }
        else
        {
            RemoveAllAttachedColliders(config.source);
            original = config.source.AddComponent<T>();
        }
        var attached = config.target.AddComponent<T>();

        copy(original, attached);

        if (!config.useExisting)
        {
            DestroyImmediate(original);
        }

        return attached;
    }

    public void RemoveAllAttachedColliders(GameObject target)
    {
        var all = target.GetComponents<Collider2D>();

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

    [Serializable]
    public struct SpriteColliderConfig
    {
        [Tooltip("The collider source GameObject that contains a sprite collider in its hierarchy.")]
        public GameObject source;

        [Tooltip("The target GameObject that the script will attach a collider to.")]
        public GameObject target;

        [Tooltip("The type of collider to assign")]
        public ColliderType type;

        [Tooltip("Set this to true to use the existing collider data on 'other', rather than creating one from scratch.")]
        public bool useExisting;

        [Tooltip("Set this to true to use the source offset for the generated collider. Otherwise, script will use offset of zero.")]
        public bool useSourceOffset;
    }
}

#endif