using UnityEngine;
using UnityEngine.InputSystem;
using System;

// todo: fix
public class MovementLookAt : MonoBehaviour
{
    [NonSerialized]
    new public Camera camera;
    public Transform root;
    public float size = 10;
    public float multiplier = 0.5f;
    public float sensitivity = 0.04f;

    private Vector3 initialPos;

    public void Start()
    {
        camera = GameObject.Find("CameraStandard").GetComponent<Camera>();
        initialPos = transform.position;
    }

    public void Update()
    {
        var mousePos = Mouse.current.position;
        var target = camera.ScreenToWorldPoint(mousePos.value);
        target.z = 0;

        var newPos = Vector2.Lerp(initialPos, target, sensitivity);
        var toTarget = target - transform.position;

        if (toTarget.magnitude > size)
        {
            newPos = initialPos + toTarget.normalized * multiplier;
        }
        transform.position = newPos;
    }
}
