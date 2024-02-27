using UnityEngine;
using UnityEditor;


#if UNITY_EDITOR
[CustomEditor(typeof(Sample))]
public class SampleEditor : Editor
{
    Sample _sample;


    private void OnEnable()
    {
        _sample = (Sample)target;
    }

    public override void OnInspectorGUI()
    {
        _sample.number = EditorGUILayout.IntField("숫 ☆ 자", _sample.number);
    }
}
#endif
