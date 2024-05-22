using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
public class GeospatialDataParser
{
    [MenuItem("DataParsers/Parse GeospatialData")]
    static void Parse()
    {
        string[] guids = AssetDatabase.FindAssets("GeospatialData", new string[] { "Assets/Database/" });

        if (guids.Length > 0)
        {
            TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(AssetDatabase.GUIDToAssetPath(guids[0]));
            string data = textAsset.ToString();
            string tagOpen = "<coordinates>";
            string tagClose = "</coordinates>";

            CreateDataAsset(data, tagOpen, tagClose);
        }
    }

    static void CreateDataAsset(string data, string tagOpen, string tagClose)
    {
        string[] words = data.Split(new char[] { ' ', '\n', '\r', '\t' });

        List<GeospatialData.Pose> poses = new List<GeospatialData.Pose>();
        foreach (var item in words)
        {
            if (item.StartsWith(tagOpen))
            {
                string[] filtered = item.Substring(tagOpen.Length, item.Length - tagOpen.Length - tagClose.Length).Split(',');
                poses.Add(new GeospatialData.Pose { longitude = double.Parse(filtered[0]), latitude = double.Parse(filtered[1]), altitude = double.Parse(filtered[2]) });
            }
        }

        GeospatialData dataAsset = ScriptableObject.CreateInstance<GeospatialData>();
        dataAsset.poses = poses;
        AssetDatabase.CreateAsset(dataAsset, $"Assets/Resources/GeospatialData/{nameof(GeospatialData)}.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Debug.Log("GeospatialDataParser : Created data asset.");
    }
}
#endif