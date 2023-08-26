using System;
using UnityEngine;

public class SkeletonBackend : MonoBehaviour
{
    public AssignLimb configLimb;
    public AssignSpriteCollider configCollider;

    public void GenerateVisible()
    {
        Debug.Assert(configLimb != null, "config limb reference is empty");

        configLimb.GenerateAll();
    }

    public void GenerateColliders()
    {
        Debug.Assert(configCollider != null, "config collider reference is empty");

        Debug.Log($"placeholder: {nameof(GenerateColliders)}");
        configCollider.GenerateAndAssignCollider();
    }
}
