using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class CoreConditionalAttribute : PropertyAttribute
{
    public readonly string sourcePropertyName;
    public readonly object checkValue;

    public CoreConditionalAttribute(string sourceName)
    {
        sourcePropertyName = sourceName;
    }

    public CoreConditionalAttribute(string sourceName, object valueToCompare)
        : this(sourceName)
    {
        checkValue = valueToCompare;
    }
}
