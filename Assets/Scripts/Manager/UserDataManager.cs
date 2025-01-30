using System;
using System.Collections.Generic;
using UnityEngine;

namespace MSKim.Manager
{
    public class UserDataManager : MonoBehaviour
    {
        private static UserDataManager instance;

        private Dictionary<Utils.CurrencyType, int> userCurrencyData = new();
        private Dictionary<Utils.ShopItemIndex, int> userUpgradeData = new();

        [Header("InGame Data Info")]
        [SerializeField] private int currentGoldAmount = 0;

        public static UserDataManager Instance
        {
            get
            {
                if (instance == null) instance = new();
                return instance;
            }
        }

        public Dictionary<Utils.CurrencyType, int> UserCurrencyData => userCurrencyData;

        public int CurrentGoldAmount
        {
            get => currentGoldAmount;
            set
            {
                currentGoldAmount = value;
            }
        }

        private void Awake()
        {
            if(instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            Initialize();
        }

        private void Initialize()
        {
            for(int i = 0; i < Enum.GetValues(typeof(Utils.CurrencyType)).Length; i++)
            {
                if (userCurrencyData.ContainsKey((Utils.CurrencyType)i)) continue;

                userCurrencyData.Add((Utils.CurrencyType)i, 0);
            }

            for(int i = 0; i < Enum.GetValues(typeof(Utils.ShopItemIndex)).Length; i++)
            {
                if(userUpgradeData.ContainsKey((Utils.ShopItemIndex)i)) continue;

                userUpgradeData.Add((Utils.ShopItemIndex)i, GameDataManager.Instance.GetShopItemData(i).BaseLevel);
            }
        }

        public void IncreaseAmount(Utils.CurrencyType type, int addAmount)
        {
            if(!userCurrencyData.ContainsKey(type))
            {
                Debug.LogWarning($"{type} 재화 데이터가 없습니다.");
                return;
            }

            userCurrencyData[type] += addAmount;
        }

        public void DecreaseAmount(Utils.CurrencyType type, int subAmount)
        {
            if (!userCurrencyData.ContainsKey(type))
            {
                Debug.LogWarning($"{type} 재화 데이터가 없습니다.");
                return;
            }

            userCurrencyData[type] -= subAmount;

            if (userCurrencyData[type] < 0)
            {
                userCurrencyData[type] = 0;
            }
        }

        public void UpgradeAmount(Utils.ShopItemIndex type)
        {
            if(!userUpgradeData.ContainsKey(type)) return;

            userUpgradeData[type]++;
        }

        public int GetUpgradeAmount(Utils.ShopItemIndex type)
        {
            if (!userUpgradeData.ContainsKey(type)) return -1;

            return userUpgradeData[type];
        }
    }
}