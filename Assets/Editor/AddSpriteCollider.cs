using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConfigureSpriteCollider))]
public class AddSpriteCollider : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate and Assign"))
        {
            var component = (ConfigureSpriteCollider) target;

            component.GenerateAndAssignCollider();
        }
    }
}
