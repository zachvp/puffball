using UnityEngine;

public class InitTransform : MonoBehaviour
{
    public Transform source;

    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        transform.position = source.position;
        transform.rotation = source.rotation;
        transform.localScale = source.localScale;
    }
}
