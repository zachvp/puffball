#if UNITY_EDITOR

using UnityEngine;
using ZCore;


[Tooltip("Use to generate a limb hierarchy from a source limb GameObject root.")]
public class AssignVisible : MonoBehaviour
{
    [Tooltip("The limb's binding definitions. The hierarchy and components of the sources will be copied to the corresponding targets.")]
    public BindingGameObject[] bindings = new BindingGameObject[1];

    public void GenerateAll()
    {
        foreach (var binding in bindings)
        {
            Generate(binding.source, binding.target);
        }
    }

    public void Generate(GameObject source, GameObject target)
    {
        var existing = CoreUtilities.FindChild(target.transform, Constants.NAME_OBJECT_VIS);

        // clean any existing hierarchy
        CoreUtilities.DestroyDependents(existing);

        // create limb from source, assign as child
        var limb = Instantiate(source, target.transform);
        limb.name = Constants.NAME_OBJECT_VIS;

        // set this Transform's current position to be the fill offset,
        // then zero out the instantiated Transform hiarchy positions
        var newPos = Vector3.zero;

        for (var i = 0; i < limb.transform.childCount; i++)
        {
            var child = limb.transform.GetChild(i);
            // the 'fill' child is the source of truth for position
            if (child.name.StartsWith(Constants.NAME_FILL_PREFIX))
            {
                newPos = child.localPosition;
            }
            child.localPosition = Vector3.zero;
        }

        // walk up the Transform hierarchy to apply all ancestor transform offsets
        // the offset will be the sum of all ancestor local positions.
        var walk = target.transform;
        var offset = Vector3.zero;
        while (walk.parent)
        {
            offset += walk.parent.localPosition;
            walk = walk.parent;
        }
        target.transform.localPosition = newPos - offset;
    }
}

#endif