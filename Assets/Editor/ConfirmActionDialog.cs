#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System;

public class DialogConfirmAction : EditorWindow
{
    public Action onConfirm;
    public Action onCancel;

    public static void Display(Action confirmAction, Action cancelAction)
    {
        var window = GetWindow<DialogConfirmAction>("Action confirmation");

        window.minSize = new Vector2(400, 200);
    }

    private void OnGUI()
    {
        GUILayout.Label("This is a destructive action, continue?", EditorStyles.boldLabel);
        GUILayout.Space(16);

        if (GUILayout.Button("Confirm"))
        {
            Debug.Log("button confirm");
            Emitter.Send(onConfirm);
        }

        if (GUILayout.Button("Cancel"))
        {
            Debug.Log("button cancel");
            Emitter.Send(onCancel);
        }
    }
}

#endif