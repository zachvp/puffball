using UnityEngine;

[Tooltip("Use to generate a collider on a target GameObject, then assign that collider to this GameObject.")]
public class ConfigureSpriteCollider : MonoBehaviour
{
    [Tooltip("The other GameObject that is the collider's source.")]
    public GameObject other;

    // todo: select collider type
    // todo: traverse hierarchy to find sprite collider

    public void GenerateAndAssignCollider()
    {
        Debug.Log("fired, generating collider");

        var existing = other.GetComponent<CircleCollider2D>();
        var attached = gameObject.GetComponent<CircleCollider2D>();

        if (existing)
        {
            Debug.Log($"Destroying existing component from source: {existing}");
            DestroyImmediate(existing);
        }
        if (attached)
        {
            Debug.Log($"Destroying existing attached component: {attached}");
            DestroyImmediate(attached);
        }

        var generated = other.AddComponent<CircleCollider2D>();
        attached = gameObject.AddComponent<CircleCollider2D>();

        CopyCollider(generated, ref attached);
        transform.position = generated.transform.position;
        DestroyImmediate(generated);
    }

    public void CopyCollider(CircleCollider2D source, ref CircleCollider2D copy)
    {
        Debug.Assert(source != null && copy != null,
            $"At least one NULL ref passed to {nameof(CopyCollider)}");

        copy.offset = source.offset;
        copy.radius = source.radius;
        copy.isTrigger = source.isTrigger;
        copy.sharedMaterial = source.sharedMaterial;
        copy.usedByEffector = source.usedByEffector;
    }
}
