using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TestDefault : MonoBehaviour
{
    public VarWatch<bool> trigger;

    public void Update()
    {
        var keyboard = Keyboard.current;

        if (keyboard.tabKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
