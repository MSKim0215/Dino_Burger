using MSKim.Manager;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PlayerData
{
    public SaveDataForDict<Utils.CurrencyType, int> UserCurrencyData = new();
    public SaveDataForDict<Utils.ShopItemIndex, int> UserUpgradeData = new();

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

namespace MSKim.Manager
{
    [Serializable]
    public class UserDataManager : BaseManager
    {
        [Header("InGame Data Info")]
        [SerializeField] private int currentGoldAmount = 0;

        private PlayerData playerData = new();
        private string path;
        private string fileName = "/save";
        private string keyWord = "sjahfiwpncvp!#$%*%! !#$";

        public event Action<Utils.CurrencyType, int> OnChangeCurrency;
        public event Action<Utils.ShopItemIndex, int> OnChangeUpgrade;
        public event Action<int> OnChangeInGameCurrency;

        public int CurrentGoldAmount
        {
            get => currentGoldAmount;
            set
            {
                currentGoldAmount = value;
                OnChangeInGameCurrency?.Invoke(CurrentGoldAmount);
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            playerData.Initialize();

            path = Application.persistentDataPath + fileName;

            LoadData();
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                IncreaseAmount(Utils.CurrencyType.Gold, 100);
            }
        }

        public void SaveData()
        {
            var data = JsonUtility.ToJson(playerData);
            File.WriteAllText(path, EncryptAndDecrypt(data));
        }

        public void LoadData()
        {
            if(!File.Exists(path))
            {
                SaveData();
            }

            var data = File.ReadAllText(path);
            playerData = JsonUtility.FromJson<PlayerData>(EncryptAndDecrypt(data));
        }

        private string EncryptAndDecrypt(string data)
        {
            var result = string.Empty;

            for(int i = 0; i < data.Length; i++)
            {
                result += (char)(data[i] ^ keyWord[i % keyWord.Length]);
            }

            return result;
        }

        public void Payment(Utils.CurrencyType currencyType, Data.ShopItemData paymentData)
        {
            Payment(currencyType, paymentData, OnPaymentSuccess, OnPaymentFailure);
        }

        private void Payment(Utils.CurrencyType currencyType, Data.ShopItemData paymentData, UnityAction<Utils.CurrencyType, Data.ShopItemData> success, UnityAction<string> failure)
        {
            if (!playerData.UserCurrencyData.ContainsKey(currencyType))
            {
                failure?.Invoke($"{currencyType} 재화 데이터가 없습니다.");
                return;
            }

            if (playerData.UserCurrencyData[currencyType] < paymentData.Price)
            {
                failure?.Invoke("재화가 부족합니다.");
                return;
            }

            success?.Invoke(currencyType, paymentData);
        }

        private void OnPaymentSuccess(Utils.CurrencyType currencyType, Data.ShopItemData paymentData)
        {
            DecreaseAmount(currencyType, paymentData.Price);
            UpgradeLevel((Utils.ShopItemIndex)paymentData.Index);
        }

        private void OnPaymentFailure(string failureMessage)
        {
            Debug.LogWarning($"Payment Failure: {failureMessage}");
        }

        public void IncreaseAmount(Utils.CurrencyType currencyType, int addAmount)
        {
            if(!playerData.UserCurrencyData.ContainsKey(currencyType))
            {
                Debug.LogWarning($"{currencyType} 재화 데이터가 없습니다.");
                return;
            }

            playerData.UserCurrencyData[currencyType] += addAmount;

            OnChangeCurrency?.Invoke(currencyType, playerData.UserCurrencyData[currencyType]);
            SaveData();
        }

        public void DecreaseAmount(Utils.CurrencyType currencyType, int subAmount)
        {
            if (!playerData.UserCurrencyData.ContainsKey(currencyType))
            {
                Debug.LogWarning($"{currencyType} 재화 데이터가 없습니다.");
                return;
            }

            playerData.UserCurrencyData[currencyType] -= subAmount;

            if (playerData.UserCurrencyData[currencyType] < 0)
            {
                playerData.UserCurrencyData[currencyType] = 0;
            }

            OnChangeCurrency?.Invoke(currencyType, playerData.UserCurrencyData[currencyType]);
            SaveData();
        }

        public void UpgradeLevel(Utils.ShopItemIndex type)
        {
            if(!playerData.UserUpgradeData.ContainsKey(type)) return;

            playerData.UserUpgradeData[type]++;

            OnChangeUpgrade?.Invoke(type, playerData.UserUpgradeData[type]);
            SaveData();
        }

        public int GetCurrencyAmount(Utils.CurrencyType currencyType)
        {
            if (!playerData.UserCurrencyData.ContainsKey(currencyType)) return 0;

            return playerData.UserCurrencyData[currencyType];
        }

        public int GetUpgradeLevel(Utils.ShopItemIndex type)
        {
            if (!playerData.UserUpgradeData.ContainsKey(type)) return 0;

            return playerData.UserUpgradeData[type];
        }

        public float GetUpgradeAmount(Utils.ShopItemIndex type)
        {
            if (type == Utils.ShopItemIndex.None) return 0f;

            return GetUpgradeLevel(type) * Managers.GameData.GetShopItemData((int)type).UpgradeAmount;
        }
    }
}