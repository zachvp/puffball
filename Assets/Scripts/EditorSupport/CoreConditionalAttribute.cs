using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class CoreConditionalAttribute : PropertyAttribute
{
    public readonly string property;

    public CoreConditionalAttribute(string p)
    {
        property = p;
    }
}
