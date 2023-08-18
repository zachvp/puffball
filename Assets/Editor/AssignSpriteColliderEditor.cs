using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AssignSpriteCollider))]
public class AssignSpriteColliderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate and Assign"))
        {
            var response = EditorUtility.DisplayDialog(
                "This is a destructive Action, continue?",
                "The Colliders from the existing GameObjects will be removed.",
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
    }
}
