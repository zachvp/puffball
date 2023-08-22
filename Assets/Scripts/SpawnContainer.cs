using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnContainer : MonoBehaviour
{
    [Tooltip("The object templates to spawn")]
    public GameObject[] spawnObjects = new GameObject[1];

    public void Start()
    {
        PlayerInputManager.instance.onPlayerJoined += HandlePlayerJoined;
    }

    public void HandlePlayerJoined(PlayerInput playerInput)
    {
        foreach (var spawnObject in spawnObjects)
        {
            Debug.Assert(spawnObject != null, "spawn object item is empty");

            Instantiate(spawnObject);
        }
    }
}
