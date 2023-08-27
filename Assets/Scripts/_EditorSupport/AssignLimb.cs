#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System;

[Tooltip("Use to generate a limb hierarchy from a source limb GameObject root.")]
// todo: rename to 'AssignVis'
public class AssignLimb : MonoBehaviour
{
    [Tooltip("The limb's binding definitions. The hierarchy and components of the sources will be copied to the corresponding targets.")]
    public LimbBinding[] bindings = new LimbBinding[1];

    public void GenerateAll()
    {
        foreach (var binding in bindings)
        {
            Generate(binding.source, binding.target);
        }
    }

    public void Generate(GameObject source, GameObject target)
    {
        Debug.Log($"generating limb from source: {source}, destroy current children");

        var existing = CoreUtilities.FindChild(target, CoreConstants.NAME_OBJECT_VIS);

        // clean any existing hierarchy
        CoreUtilities.DestroyDependents(existing);

        // create limb from source, assign as child
        var limb = Instantiate(source, target.transform);
        limb.name = CoreConstants.NAME_OBJECT_VIS;

        // set this Transform's current position to be the fill offset,
        // then zero out the instantiated Transform hiarchy positions
        var newPos = Vector3.zero;

        for (var i = 0; i < limb.transform.childCount; i++)
        {
            var child = limb.transform.GetChild(i);

            // the 'fill' child is the source of truth for position
            if (child.name.StartsWith(CoreConstants.NAME_FILL_PREFIX))
            {
                newPos = child.localPosition;
            }

            child.localPosition = Vector3.zero;
        }

        target.transform.localPosition = newPos;
    }

    [Serializable]
    public struct LimbBinding
    {
        public GameObject source;
        public GameObject target;
    }
}

#endif