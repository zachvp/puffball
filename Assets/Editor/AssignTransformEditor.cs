using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AssignTransform))]
public class AssignTransformEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Assign"))
        {
            var component = (AssignTransform) target;

            component.Assign();

            EditorUtility.DisplayDialog("Assignment complete", "Copied all transforms.", "Close");
        }

        base.OnInspectorGUI();
    }
}
