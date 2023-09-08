using UnityEngine;

public class MovementRadial : MonoBehaviour
{
    public Transform anchor;
    public Transform target;

    public float range;
    public Vector2 scaleXMinMax = Vector2.one;
    public Vector2 scaleYMinMax = Vector2.one;

    public Vector3 Move(Vector2 input)
    {
        //SceneRefs.instance.uiDebug.text = Vector2.Dot(input, Vector2.right).ToString();
        //var dot = Vector2.Dot(input, Vector2.right);
        //float scale = Mathf.Lerp(0.5f, 2f, dot);
        var scale = 1f;

        //if (dot > 0.2f)
        //{
        //    scale = 1.5f;
        //}
        //else
        //{
        //    scale = 0.5f;
        //}
            
        var newPos = (Vector2) anchor.position + (input * range * scale);

        var body = target.GetComponent<Rigidbody2D>();
        if (body)
        {
            body.position = newPos;
        }
        else
        {
            target.position = newPos;
        }

        return newPos;
    }

    public Vector3 ResetState()
    {
        target.position = anchor.position;

        return anchor.position;
    }
}
