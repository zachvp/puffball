using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class CoreConditionalAttribute : PropertyAttribute
{
    public readonly string sourcePropertyName;

    public CoreConditionalAttribute(string sourceName)
    {
        sourcePropertyName = sourceName;
    }
}
