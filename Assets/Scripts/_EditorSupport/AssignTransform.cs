#if UNITY_EDITOR

using System;
using UnityEngine;

public class AssignTransform : MonoBehaviour
{
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
        CoreUtilities.CopyTransform(c.binding.source.transform, c.binding.target.transform);



        // todo: extend to find "fill" child
    }

    [Serializable]
    public struct Config
    {
        [Tooltip("The binding definitions. The source transform values will copy over to the target.")]
        public BindingGameObject binding;
    }
}

#endif