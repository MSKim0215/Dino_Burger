using MSKim.Manager;
using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public SaveDataForDict<Utils.CurrencyType, int> UserCurrencyData = new();     // 재화 데이터
    public SaveDataForDict<Utils.ShopItemIndex, int> UserUpgradeData = new();     // 강화 데이터

    public void Initialize()
    {
        for (int i = 0; i < Enum.GetValues(typeof(Utils.CurrencyType)).Length; i++)
        {
            if (UserCurrencyData.ContainsKey((Utils.CurrencyType)i)) continue;

            UserCurrencyData.Add((Utils.CurrencyType)i, 0);
        }

        for (int i = 0; i < Enum.GetValues(typeof(Utils.ShopItemIndex)).Length - 1; i++)
        {
            if (UserUpgradeData.ContainsKey((Utils.ShopItemIndex)i)) continue;

            UserUpgradeData.Add((Utils.ShopItemIndex)i, Managers.GameData.GetShopItemData(i).BaseLevel);
        }
    }
}

[Serializable]
public class SaveDataPair<TKey, TValue>
{
    public TKey Key;
    public TValue Value;
}

[Serializable]
public class SaveDataForDict<TKey, TValue>
{
    public List<SaveDataPair<TKey, TValue>> Data = new();

    public void Add(TKey key, TValue value)
    {
        Data.Add(new SaveDataPair<TKey, TValue> { Key = key, Value = value });
    }

    public bool ContainsKey(TKey key)
    {
        return Data.Exists(pair => EqualityComparer<TKey>.Default.Equals(pair.Key, key));
    }

    public TValue this[TKey key]
    {
        get => Data.Find(pair => EqualityComparer<TKey>.Default.Equals(pair.Key, key)).Value;
        set
        {
            var pair = Data.Find(p => EqualityComparer<TKey>.Default.Equals(p.Key, key));
            if (pair != null)
            {
                pair.Value = value;
            }
        }
    }
}