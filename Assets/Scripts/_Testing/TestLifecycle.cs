using System;
using UnityEngine;

public class TestLifecycle : MonoBehaviour
{
    public void Update()
    {
        Debug.Log($"{name}: update called");
    }

    public void OnEnable()
    {
        Debug.Log($"{name}: on enable");
    }

    public void OnDisable()
    {
        Debug.Log($"{name}: on disable");
    }
}
