using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AssignLimb))]
public class AssignLimbEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate hierarchy"))
        {
            var response = EditorUtility.DisplayDialog(
                "This is a destructive Action, continue?",
                "The limb data from the existing GameObjects will be removed.",
                "Confirm",
                "Cancel");

            if (response)
            {
                var component = (AssignLimb) target;

                component.Generate();
            }
            else
            {
                Debug.Log("User Canceled Action");
            }
        }
    }
}
