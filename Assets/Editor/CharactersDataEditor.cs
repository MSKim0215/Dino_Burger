using MSKim.Data;
using UnityEditor;

[CustomEditor(typeof(GuestsData))]
public class GuestsDataEditor : BaseGameDataEditor
{
    private GuestsData _target;

    protected override string GetData() => _target.Json.text;

    protected override void OnEnable()
    {
        _target = (GuestsData)target;
    }

    protected override void Load(string data)
    {
        Load(_target.GuestDataList, data);
    }

    protected override void Save()
    {
        Save(_target.GuestDataList, _target);
    }
}

[CustomEditor(typeof(PlayersData))]
public class PlayersDataEditor : BaseGameDataEditor
{
    private PlayersData _target;

    protected override string GetData() => _target.Json.text;

    protected override void OnEnable()
    {
        _target = (PlayersData)target;
    }

    protected override void Load(string data)
    {
        Load(_target.PlayerDataList, data);
    }

    protected override void Save()
    {
        Save(_target.PlayerDataList, _target);
    }
}