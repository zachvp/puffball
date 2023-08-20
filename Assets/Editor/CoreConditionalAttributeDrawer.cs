using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CoreConditionalAttribute))]
public class CoreConditionalAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Debug.Assert(attribute is CoreConditionalAttribute,
                    $"wrong type for custom drawer; expected {nameof(CoreConditionalAttribute)}");

        // attribute containing the name of the property that drives the display logic.
        var cast = attribute as CoreConditionalAttribute;
        var sourceProperty = property.serializedObject.FindProperty(cast.sourcePropertyName);

        // Only display this field if the source property is enabled.

        if (sourceProperty.boolValue)
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }
}
