using UnityEngine;

[Tooltip("Attach to a GameObject to mutate its Transform hierarchy on game start.")]
public class InitHierarchy : MonoBehaviour
{

    [Tooltip("The other objects to assign to this component's Transform hierarchy.")]
    public Transform[] others = new Transform[1];

    [Tooltip("The type of assignment. For example, type CHILD will assign the other Transform as this Transform's child.")]
    public Relationship relationship = Relationship.CHILD;

    public Transform offset;

    public void Start()
    {
        foreach (var o in others)
        {
            Debug.Assert(o != null, $"'None' entry in {nameof(Transform)}[]:{nameof(others)}");
            Mutate(o);
        }

        if (offset)
        {
            var newPos = Vector3.zero;

            // find the 'fill' child
            for (var i = 0; i < offset.childCount; i++)
            {
                var child = offset.GetChild(i);

                if (child.name.StartsWith("fill"))
                {
                    Debug.Log("use fill child position as offset");
                    newPos = child.position;
                }

                child.localPosition = Vector3.zero;
            }

            transform.position = newPos;
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
