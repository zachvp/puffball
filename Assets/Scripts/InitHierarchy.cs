using UnityEngine;

[Tooltip("Attach to a GameObject to mutate its Transform hierarchy on game start.")]
public class InitHierarchy : MonoBehaviour
{

    [Tooltip("The other objects to assign to this component's Transform hierarchy.")]
    public Transform[] others = new Transform[1];

    [Tooltip("The type of assignment. For example, type CHILD will assign the other Transform as this Transform's child.")]
    public Relationship relationship = Relationship.CHILD;

    public void Start()
    {
        foreach (var o in others)
        {
            Debug.Assert(o != null, $"'None' entry in {nameof(Transform)}[]:{nameof(others)}");
            Mutate(o);
        }
    }

    public void Mutate(Transform target)
    {

        switch (relationship)
        {
            case Relationship.NONE:
                target.SetParent(null);
                break;
            case Relationship.PARENT:
                transform.SetParent(target);
                break;
            case Relationship.CHILD:
                target.SetParent(transform);
                break;
            default:
                Debug.LogError($"Unhandled type: {relationship}");
                break;
        }
    }

    public enum Relationship
    {
        NONE,
        PARENT,
        CHILD
    }
}
