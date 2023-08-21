using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TestDefault : MonoBehaviour
{
    public GameObject[] spawn;
    public VarWatch<bool> trigger;

    public void Start()
    {
        trigger.onChanged += (old, updated) =>
        {
            if (updated)
            {
                Spawn();
                trigger.value = false;
            }
        };
    }

    public void Update()
    {
        trigger.Update(trigger.value);

        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void Spawn()
    {
        foreach (var s in spawn)
        {
            var spawn = Instantiate(s, transform);
            spawn.transform.position = transform.position;
        }
    }
}
