using UnityEditor;
using UnityEngine;
using System;

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

        // Only display this field if the source property equals the configured value.
        var display = false;
        switch (sourceProperty.propertyType)
        {
            case SerializedPropertyType.Boolean:
                display = sourceProperty.boolValue;
                break;
            case SerializedPropertyType.Enum:
                display = sourceProperty.enumValueIndex == Convert.ToInt32(cast.checkValue);
                break;
            default:
                Debug.LogError($"unhandled value type: {sourceProperty.propertyType}");
                break;
        }

        if (display)
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }
}
