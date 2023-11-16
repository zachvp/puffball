using ZCore;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AssignVisible))]
public class AssignVisibleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Generate and Assign"))
        {
            var response = EditorUtility.DisplayDialog(
                "This is a destructive Action, continue?",
                $"The GameObjects named {Constants.NAME_OBJECT_VIS} from the existing targets will be removed.",
                "Confirm",
                "Cancel");

            if (response)
            {
                var component = (AssignVisible) target;

                component.GenerateAll();
            }
            else
            {
                Debug.Log("User Canceled Action");
            }
        }

        base.OnInspectorGUI();
    }
}
