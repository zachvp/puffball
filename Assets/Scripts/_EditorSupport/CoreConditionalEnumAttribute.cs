using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class CoreConditionalEnumAttribute : PropertyAttribute
{
    public readonly string sourcePropertyName;
    public readonly Enum checkValue;

    public CoreConditionalEnumAttribute(string sourceName, object value)
    {
        sourcePropertyName = sourceName;
        checkValue = value as Enum;
    }
}
