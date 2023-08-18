using UnityEngine;

public class OnStartSetActive : MonoBehaviour
{
    public GameObject[] objects = new GameObject[1];
    public bool active;

    public void Start()
    {
        foreach (var o in objects)
        {
            o.SetActive(active);
        }
    }
}
