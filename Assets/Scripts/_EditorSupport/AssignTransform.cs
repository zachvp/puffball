#if UNITY_EDITOR

using System;
using UnityEngine;
using System.Collections.Generic;

public class AssignTransform : MonoBehaviour
{
    public GameObject sourceRoot;
    public Config[] config = new Config[1];

    public void Assign()
    {
        foreach (var c in config)
        {
            Assign(c);
        }
    }

    public void Assign(Config c)
    {
        var implicitSource = CoreUtilities.FindChild(c.binding.source.transform, CoreConstants.NAME_FILL_PREFIX);

        CoreUtilities.CopyTransform(implicitSource.transform, c.binding.target.transform);
    }

    public void FindSources()
    {
        foreach (var c in config)
        {
            FindSource(c);
        }
    }

    public void FindSource(Config c)
    {
        // navigate from source root to find child with same name as target game object
        var unexplored = new Stack<Transform>();
        unexplored.Push(sourceRoot.transform);
        while (unexplored.Count > 0)
        {
            var next = unexplored.Pop();

            if (next.name.Equals(c.binding.target.name) && c.binding.source == null)
            {
                c.binding.source = next.gameObject;
                Debug.Log($"found match for target: {c.binding.target} from source: {next}");
            }

            CoreUtilities.ForeachChild(next, (child) =>
            {
                unexplored.Push(child);
            });
        }
    }

    [Serializable]
    public class Config
    {
        [Tooltip("The binding definitions. The source transform values will copy over to the target.")]
        public BindingGameObject binding;
    }
}

#endif