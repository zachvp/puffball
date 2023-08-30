using System;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(TestScriptableObject), menuName = CoreConstants.DEFAULT_MENU + nameof(TestScriptableObject))]
public class TestScriptableObject : ScriptableObject
{
    public int number;
    public string text;
    public GameObject empty;
}
