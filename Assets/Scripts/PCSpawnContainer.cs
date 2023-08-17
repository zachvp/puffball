using UnityEngine;
using UnityEngine.InputSystem;

public class PCSpawnContainer : MonoBehaviour
{
    [Tooltip("The object templates to spawn")]
    public GameObject[] spawnObjects;

    public void Start()
    {
        PlayerInputManager.instance.onPlayerJoined += HandlePlayerJoined;
    }

    public void HandlePlayerJoined(PlayerInput playerInput)
    {
        foreach (var spawnObject in spawnObjects)
        {
            Instantiate(spawnObject);
        }
    }
}
