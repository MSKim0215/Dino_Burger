using MSKim.Data;
using UnityEditor;

[CustomEditor(typeof(IngredientsData))]
public class IngredientsDataEditor : BaseGameDataEditor
{
    private IngredientsData _target;

    protected override string GetData() => _target.Json.text;

    protected override void OnEnable()
    {
        _target = (IngredientsData)target;
    }

    protected override void Load(string data)
    {
        Load(_target.IngredientDataList, data);
    }

    protected override void Save()
    {
        Save(_target.IngredientDataList, _target);
    }
}
