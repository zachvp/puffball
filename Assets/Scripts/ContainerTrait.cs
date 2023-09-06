using UnityEngine;

public class ContainerTrait : MonoBehaviour
{
    public Trait value;

#if DEBUG
    public void Start()
    {
        Debug.Assert(Validate(), $"more than one instance of {nameof(ContainerTrait)} exists in hierchy on GameObject {name}");
    }

    public bool Validate()
    {
        var ancestor = GetComponentInParent<ContainerTrait>();
        var descendent = GetComponentInChildren<ContainerTrait>();
        var result = true;

        if (ancestor)
        {
            result &= ancestor.Equals(this);
        }
        if (descendent)
        {
            result &= descendent.Equals(this);
        }

        return result;
    }
#endif
}
