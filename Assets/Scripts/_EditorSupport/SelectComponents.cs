#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System;

// todo: migrate to 'ScriptableWizard'
// see: https://docs.unity3d.com/ScriptReference/Selection-objects.html
public class SelectComponents : MonoBehaviour
{
    public Config[] configs;

    public void Select()
    {
        Debug.Log("script running select");
        var config = configs[0];

        switch (config.componentType)
        {
            case ComponentType.SPRITE_RENDERER:
                Debug.Log($"selecting sprite renderers");
                var result = config.target.GetComponentsInChildren<SpriteRenderer>();
                var selection = new GameObject[result.Length];

                for (var i = 0; i < result.Length; i++)
                {
                    selection[i] = result[i].gameObject;
                }
                
                Selection.objects = selection;

                break;
        }
    }

    [Serializable]
    public struct Config
    {
        public ComponentType componentType;
        public GameObject target;
    }

    public enum ComponentType
    {
        NONE,
        SPRITE_RENDERER
    }

    public enum ActionType
    {
        NONE,
        RELATIVE,
        ABSOLUTE
    }
}

#endif