using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MyCustomEditorWindow : EditorWindow
{
    GUILayoutOption[] _size100x50 = { GUILayout.Width(100f), GUILayout.Height(50f) };
    Object _target;


    [MenuItem("Window/MyCustomEditorWindow")]
    public static void ShowWindow()
    {
        MyCustomEditorWindow window = GetWindow<MyCustomEditorWindow>();
        window.Show();
    }

    private void OnGUI()
    {
        DrawTitle();
        DrawTestButton();
        DrawObjectField();
    }

    private void DrawTitle()
    {
        GUILayout.Label("My CustomEditorWindow", EditorStyles.boldLabel);
    }

    private void DrawTestButton()
    {
        if (GUILayout.Button("Do Test", _size100x50))
        {
            Debug.Log("This is a test ~!");
        }
    }

    private void DrawObjectField()
    {
        _target = EditorGUILayout.ObjectField(_target, typeof(Object), true);
    }
}
