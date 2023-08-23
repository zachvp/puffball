using UnityEditor;
using UnityEngine;
using System;

[CustomPropertyDrawer(typeof(CoreConditionalEnumAttribute))]
public class CoreConditionalEnumAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Debug.Assert(attribute is CoreConditionalEnumAttribute,
                    $"wrong type for custom drawer; expected {nameof(CoreConditionalEnumAttribute)}");

        // attribute containing the name of the property that drives the display logic.
        var cast = attribute as CoreConditionalEnumAttribute;
        var sourceProperty = property.serializedObject.FindProperty(cast.sourcePropertyName);

        // Only display this field if the source property equals the configured value.
        if (sourceProperty.enumValueIndex == Convert.ToInt32(cast.checkValue))
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }
}
