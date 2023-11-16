using UnityEngine;
using ZCore;

[CreateAssetMenu(fileName = nameof(TestScriptableObject), menuName = Constants.DEFAULT_MENU + nameof(TestScriptableObject))]
public class TestScriptableObject : ScriptableObject
{
    public int number;
    public string text;
    public GameObject empty;
}
