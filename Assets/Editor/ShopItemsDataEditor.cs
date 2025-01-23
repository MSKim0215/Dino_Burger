using MSKim.Data;
using UnityEditor;

[CustomEditor(typeof(ShopItemsData))]
public class ShopItemsDataEditor : BaseGameDataEditor
{
    private ShopItemsData _target;

    protected override string GetData() => _target.Json.text;

    protected override void Load(string data)
    {
        Load(_target.ShopItemDataList, data);
    }

    protected override void OnEnable()
    {
        _target = (ShopItemsData)target;
    }

    protected override void Save()
    {
        Save(_target.ShopItemDataList, _target);
    }
}
