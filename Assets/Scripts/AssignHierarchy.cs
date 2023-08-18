using System;
using UnityEngine;

[Tooltip("Attach to a GameObject to mutate its Transform hierarchy on game start.")]
public class AssignHierarchy : MonoBehaviour
{
    [Tooltip("The other object to assign to this component's Transform hierarchy.")]
    public Transform other;

    [Tooltip("The type of assignment. For example, type CHILD will assign the other Transform as this Transform's child.")]
    public Relationship relationship = Relationship.CHILD;

    public void Start()
    {
        Mutate();
    }

    public void Mutate()
    {
        switch (relationship)
        {
            case Relationship.NONE:
                transform.SetParent(null);
                break;
            case Relationship.PARENT:
                transform.SetParent(other);
                break;
            case Relationship.CHILD:
                other.SetParent(transform);
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
