using UnityEngine;

[Tooltip("Attach to a GameObject to mutate its Transform hierarchy on game start.")]
public class AssignHierarchy : MonoBehaviour
{
    [Tooltip("The other object to assign to this component's Transform hierarchy.")]
    public Transform other;

    public Transform[] others = new Transform[1];

    [Tooltip("The type of assignment. For example, type CHILD will assign the other Transform as this Transform's child.")]
    public Relationship relationship = Relationship.CHILD;

    public RandomInstant random;

    public void Start()
    {
        if (other)
        {
            Mutate(other);
        }
        else
        {
            foreach (var o in others)
            {
                Mutate(o);
            }
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

        // Changing hierarchy implicitly activates the GameObject. Set to inactive for random.
        target.gameObject.SetActive(false);

        // todo: this is just a fun thing
        StartCoroutine(CoreUtilities.DelayedTask(random.Range(0.15f, 3.2f), () =>
        {
            target.gameObject.SetActive(true);
        }));
    }

    public enum Relationship
    {
        NONE,
        PARENT,
        CHILD
    }
}
