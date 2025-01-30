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
        }

        public void IncreaseAmount(Utils.CurrencyType type, int addAmount)
        {
            if (!userCurrencyData.ContainsKey(type))
            {
                userCurrencyData.Add(type, 0);
            }

            userCurrencyData[type] += addAmount;
        }

        public void DecreaseAmount(Utils.CurrencyType type, int subAmount)
        {
            if(!userCurrencyData.ContainsKey(type))
            {
                userCurrencyData.Add(type, 0);
            }

            userCurrencyData[type] -= subAmount;

            if (userCurrencyData[type] <= 0)
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
            if (!userUpgradeData.ContainsKey(type))
            {
                userUpgradeData.Add(type, GameDataManager.Instance.GetShopItemData((int)type).BaseLevel);
            }

            return userUpgradeData[type];
        }
    }
}