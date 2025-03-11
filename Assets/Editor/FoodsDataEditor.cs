using MSKim.Data;
using UnityEditor;

[CustomEditor(typeof(FoodsData))]
public class FoodsDataEditor : BaseGameDataEditor
{
    private FoodsData _target;

    protected override string GetData() => _target.Json.text;

    protected override void OnEnable()
    {
        _target = (FoodsData)target;
    }

    protected override void Load(string data)
    {
        Load(_target.FoodDataList, data);
    }

    protected override void Save()
    {
        Save(_target.FoodDataList, _target);
    }
}
