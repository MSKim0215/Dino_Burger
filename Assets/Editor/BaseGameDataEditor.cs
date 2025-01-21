using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public abstract class BaseGameDataEditor : Editor
{
    protected abstract string GetData();

    protected abstract void OnEnable();

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (target == null) return;

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Data Load From Text"))
        {
            Load(GetData());
        }

        if (GUILayout.Button("Save Data With Text"))
        {
            Save();
        }

        EditorGUILayout.EndHorizontal();
    }

    protected virtual void Load<T>(List<T> dataList, string data) 
    {
        if (string.IsNullOrEmpty(data)) return;

        dataList.Clear();

        var jsonArray = JArray.Parse(data);
        foreach(var json in jsonArray)
        {
            var jsonData = json.ToObject<T>();
            dataList.Add(jsonData);
        }
    }

    protected virtual void Save<T>(List<T> dataList, Object target)
    {
        if (dataList == null || dataList.Count == 0) return;

        var path = AssetDatabase.GetAssetPath(target);
        path = path.Replace("ScriptableObject", "Json");
        var jsonArray = new JArray();

        foreach (var data in dataList)
        {
            jsonArray.Add(JObject.FromObject(data));
        }

        var directory = Path.GetDirectoryName(path);
        var assetName = Path.GetFileNameWithoutExtension(path);
        var jsonFilePath = Path.Combine(directory, $"{assetName}.json");

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(jsonFilePath, jsonArray.ToString());
        AssetDatabase.Refresh();

        Debug.Log($"Saved JSON file at: {jsonFilePath}");
    }

    protected abstract void Load(string data);
    protected abstract void Save();
}
