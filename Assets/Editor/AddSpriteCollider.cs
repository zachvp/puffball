using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConfigureSpriteCollider))]
public class AddSpriteCollider : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Test Button"))
        {
            var component = (ConfigureSpriteCollider) target;

            component.Set();
        }
    }
}


/*
 using UnityEditor;

[CustomEditor(typeof(MyCustomComponent))]
public class MyCustomComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector fields
        DrawDefaultInspector();
        
        // Get a reference to the target component
        MyCustomComponent customComponent = (MyCustomComponent)target;

        // Render a button in the editor
        if (GUILayout.Button("Press Me"))
        {
            customComponent.ButtonPressed();
        }
    }
}
 */