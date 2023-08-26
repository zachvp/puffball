using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkeletonBackend))]
public class SkeletonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Generate visible"))
        {
            var response = EditorUtility.DisplayDialog(
                "This is a destructive Action, continue?",
                "The limb data from the existing GameObjects will be removed.",
                "Confirm",
                "Cancel");

            if (response)
            {
                var backend = (SkeletonBackend) target;

                backend.GenerateVisible();
            }
            else
            {
                Debug.Log("User Canceled Action");
            }
        }

        if (GUILayout.Button("Generate colliders"))
        {
            var backend = (SkeletonBackend)target;

            backend.GenerateColliders();
        }

        GUILayout.EndHorizontal();
    }
}
