using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class MovementLookAt : MonoBehaviour
{
    [NonSerialized]
    new public Camera camera;
    public Transform anchor;
    public Transform root;
    public float size = 10;
    public float multiplier = 0.5f;
    public float sensitivity = 0.04f;

    public void Start()
    {
        camera = GameObject.Find("CameraStandard").GetComponent<Camera>();
    }

    public void Update()
    {
        var mousePos = Mouse.current.position;
        var target = camera.ScreenToWorldPoint(mousePos.value);
        target.z = 0;


        //transform.position = anchor.position + target;
        target += anchor.position;

        //var newPos = new Vector2(Mathf.Lerp(root.position.x, target.x, sensitivity * 8), Mathf.Lerp(root.position.y, target.y, sensitivity));
        var newPos = Vector2.Lerp(root.position, target, sensitivity);
        var toTarget = target - transform.position;

        if (toTarget.magnitude > size)
        {
            newPos = root.position + toTarget.normalized * multiplier;
        }
        transform.position = newPos;

        //Debug.DrawLine(anchor.position, target, Color.red, 5f);
    }
}
