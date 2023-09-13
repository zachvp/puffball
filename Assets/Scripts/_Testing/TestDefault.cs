using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TestDefault : CoreSingletonBehavior<TestDefault>
{
    public VarWatch<bool> trigger;
    public bool isDebug;

    public void Update()
    {
        var keyboard = Keyboard.current;

        if (keyboard.tabKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
