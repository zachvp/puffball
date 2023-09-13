#if UNITY_EDITOR

using System;
using UnityEngine;

public class SkeletonBackend : MonoBehaviour
{
    public AssignVisible configVisible;
    public AssignSpriteCollider configCollider;

    public void GenerateVisible()
    {
        Debug.Assert(configVisible != null, "config limb reference is empty");

        configVisible.GenerateAll();
    }

    public void GenerateColliders()
    {
        Debug.Assert(configCollider != null, "config collider reference is empty");

        Debug.Log($"placeholder: {nameof(GenerateColliders)}");
        configCollider.GenerateAndAssignCollider();
    }
}

#endif
