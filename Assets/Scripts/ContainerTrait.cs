using UnityEngine;

public class ContainerTrait : MonoBehaviour
{
    public Trait value;

#if DEBUG
    public void Start()
    {
        Debug.Assert(Validate(), $"invalid hierarchy configuration for {name}");
    }

    public bool Validate()
    {
        var ancestor = GetComponentInParent<ContainerTrait>();
        var descendent = GetComponentInChildren<ContainerTrait>();
        var root = CoreUtilities.FindRoot(gameObject);
        var result = true;

        if (ancestor)
        {
            result &= ancestor.Equals(this);
        }
        if (descendent)
        {
            result &= descendent.Equals(this);
        }
        Debug.Assert(result, $"more than one instance of {nameof(ContainerTrait)} exists in hierarchy on GameObject {name}");

        // check that this component is attached to hierarchy root
        result &= gameObject.Equals(root);
        Debug.Assert(result, $"container trait is not at the root of the hierarchy");

        return result;
    }
#endif
}
