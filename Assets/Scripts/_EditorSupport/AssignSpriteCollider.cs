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
                attachedCollider = ApplyCircle(config);
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

    public Collider2D ApplyCircle(SpriteColliderConfig config)
    {
        var shape = ApplyShape<CircleCollider2D>(config, CoreUtilities.CopyCollider);

        return shape;
    }
    public Collider2D ApplyPolygon(SpriteColliderConfig config)
    {
        var shape = ApplyShape<PolygonCollider2D>(config, CoreUtilities.CopyCollider);

        return shape;
    }

    public Collider2D ApplyCapsule(SpriteColliderConfig config)
    {
        var shape = ApplyShape<CapsuleCollider2D>(config, CoreUtilities.CopyCollider);

        return shape;
    }

    // Takes delegate param to the implementation to copy the appropriately-typed collider.
    public T ApplyShape<T>(SpriteColliderConfig config, Action<T, T> copy) where T : Collider2D
    {
        T original;

        // find the implied child object in the target - default to 'fill'
        var implicitSource = CoreUtilities.FindChild(config.source, CoreConstants.NAME_FILL_PREFIX);

        // clean up any existing colliders on the target to start fresh
        RemoveAllAttachedColliders(config.target);

        if (config.useExisting)
        {
            original = implicitSource.GetComponent<T>();
        }
        else
        {
            RemoveAllAttachedColliders(config.source);
            original = implicitSource.AddComponent<T>();
        }
        var attached = config.target.AddComponent<T>();

        // delegate collider class-specific implementation to the caller
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

    public bool IsAnyColliderAttached()
    {
        foreach (var c in config)
        {
            //var implicitTarget = CoreUtilities.FindChild(config.target, CoreConstants.NAME_FILL_PREFIX)
        }

        return false;
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