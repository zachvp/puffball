#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

/// <summary>
/// 
/// </summary>
[Tooltip("Use to generate a limb hierarchy from a source limb GameObject root.")]
public class AssignLimb : MonoBehaviour
{
    [Tooltip("The limb's source GameObject. The hierarchy and components of this source will be copied." +
        "Thart copy will then be manipulated")]
    public GameObject source;

    public void Generate()
    {
        Debug.Log($"generating limb from source: {source}, destroy current children");

        // reset source
        source.SetActive(true);

        // clean current hierarchy
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        // create limb from source, assign as child
        var limb = Instantiate(source, transform);
        limb.name = source.name;

        // set this Transform's current position to be the fill offset,
        // then zero out the instantiated Transform hiarchy positions
        var newPos = Vector3.zero;

        for (var i = 0; i < limb.transform.childCount; i++)
        {
            var child = limb.transform.GetChild(i);

            // the 'fill' child is the source of truth for position
            if (child.name.StartsWith(CoreConstants.NAME_OBJECT_FILL))
            {
                newPos = child.localPosition;
            }

            child.localPosition = Vector3.zero;
        }

        transform.localPosition = newPos;
        source.SetActive(false);
    }
}

#endif