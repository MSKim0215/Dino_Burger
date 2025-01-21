using MSKim.Data;
using Newtonsoft.Json.Linq;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(IngredientsData))]
public class IngredientsDataEditor : Editor
{
    private IngredientsData _target;

    private void OnEnable()
    {
        _target = (IngredientsData)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(_target != null)
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Data Load From Text"))
            {
                Load(_target.Json.text);
            }

            if (GUILayout.Button("Save Data With Text"))
            {
                Save();
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    private void Load(string data)
    {
        if (string.IsNullOrEmpty(data)) return;

        _target.IngredientDataList.Clear();

        var jsonArray = JArray.Parse(data);

        foreach(var json in jsonArray)
        {
            var jsonData = json.ToObject<IngredientData>();
            _target.IngredientDataList.Add(jsonData);
        }
    }

    private void Save()
    {
        if (_target.IngredientDataList == null || _target.IngredientDataList.Count == 0) return;

        var path = AssetDatabase.GetAssetPath(_target);
        path = path.Replace("ScriptableObject", "Json");
        var jsonArray = new JArray();

        foreach(var data in _target.IngredientDataList)
        {
            jsonArray.Add(JObject.FromObject(data));
        }

        var directory = Path.GetDirectoryName(path);
        var assetName = Path.GetFileNameWithoutExtension(path);
        var jsonFilePath = Path.Combine(directory, $"{assetName}.json");
    
        if(!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(jsonFilePath, jsonArray.ToString());
        AssetDatabase.Refresh();

        Debug.Log($"Saved JSON file at: {jsonFilePath}");
    }
}
