using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AssignTransform))]
public class AssignTransformEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Assign"))
        {
            var choice = EditorUtility.DisplayDialog("Destructive action", "This will overwrite all target Transforms", "Continue", "Cancel");

            if (choice)
            {
                var component = (AssignTransform)target;
                component.Assign();
            }

            EditorUtility.DisplayDialog("Assignment complete", "Copied all transforms.", "Close");
        }

        if (GUILayout.Button("Find sources"))
        {
            // todo: call method
            var component = (AssignTransform)target;
            component.FindSources();
        }

        GUILayout.EndHorizontal();

        base.OnInspectorGUI();
    }
}
