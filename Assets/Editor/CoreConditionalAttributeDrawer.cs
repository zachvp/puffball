using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CoreConditionalAttribute))]
public class CoreConditionalAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Debug.Assert(attribute is CoreConditionalAttribute,
                    $"wrong type for custom drawer; expected {nameof(CoreConditionalAttribute)}");

        var cast = attribute as CoreConditionalAttribute;
        var compareProperty = property.serializedObject.FindProperty(cast.property);

        if (compareProperty.boolValue)
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }
}
