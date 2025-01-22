using MSKim.Data;
using UnityEditor;

[CustomEditor(typeof(TablesData))]
public class TablesDataEditor : BaseGameDataEditor
{
    private TablesData _target;

    protected override string GetData() => _target.Json.text;

    protected override void OnEnable()
    {
        _target = (TablesData)target;
    }

    protected override void Load(string data)
    {
        Load(_target.TableDataList, data);
    }

    protected override void Save()
    {
        Save(_target.TableDataList, _target);
    }
}
