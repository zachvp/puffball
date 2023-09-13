using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SelectComponents))]
public class SelectComponentsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Select"))
        {
            var component = (SelectComponents)target;

            component.Select();
        }

        base.OnInspectorGUI();
    }
}
