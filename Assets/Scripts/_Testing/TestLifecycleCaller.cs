using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestLifecycleCaller : MonoBehaviour
{
    public TestLifecycle target;

    public void Update()
    {
        var keyboard = Keyboard.current;

        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            target.gameObject.SetActive(!target.gameObject.activeSelf);
            Debug.Log($"{name}: set GameObject to active: {target.gameObject.activeSelf}");
        }

        if (keyboard.enterKey.wasPressedThisFrame)
        {
            target.enabled = !target.enabled;
            Debug.Log($"{name}: set enabled value: {target.enabled}");
        }
    }
}
