using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AssignLimb))]
public class AssignLimbEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Generate and Assign"))
        {
            var response = EditorUtility.DisplayDialog(
                "This is a destructive Action, continue?",
                $"The GameObjects named {CoreConstants.NAME_OBJECT_VIS} from the existing targets will be removed.",
                "Confirm",
                "Cancel");

            if (response)
            {
                var component = (AssignSpriteCollider)target;

                component.GenerateAndAssignCollider();
            }
            else
            {
                Debug.Log("User Canceled Action");
            }
        }

        base.OnInspectorGUI();
    }
}
