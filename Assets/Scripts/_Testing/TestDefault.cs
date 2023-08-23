using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TestDefault : MonoBehaviour
{
    public VarWatch<bool> trigger;

    public List<int> test = new List<int>();
    public int counter;
    public float t;
    public float maxT = 1;

    public void Update()
    {
        var keyboard = Keyboard.current;

        if (t < maxT)
        {
            t += Time.deltaTime;
        }
        else
        {
            test.Add(counter);
            counter++;
        }

        if (keyboard.tabKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
