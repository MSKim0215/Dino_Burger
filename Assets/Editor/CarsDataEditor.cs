using MSKim.Data;
using UnityEditor;

[CustomEditor(typeof(CarsData))]
public class CarsDataEditor : BaseGameDataEditor
{
    private CarsData _target;

    protected override string GetData() => _target.Json.text;

    protected override void OnEnable()
    {
        _target = (CarsData)target;
    }

    protected override void Load(string data)
    {
        Load(_target.CarDataList, data);
    }

    protected override void Save()
    {
        Save(_target.CarDataList, _target);
    }
}
