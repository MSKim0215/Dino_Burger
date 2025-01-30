using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MSKim.Manager
{
    public class UserDataManager : MonoBehaviour
    {
        private static UserDataManager instance;

        private Dictionary<Utils.CurrencyType, int> userCurrencyData = new();
        private Dictionary<Utils.ShopItemIndex, int> userUpgradeData = new();

        [Header("InGame Data Info")]
        [SerializeField] private int currentGoldAmount = 0;

        public event Action<Utils.CurrencyType, int> OnChangeCurrency;
        public event Action<Utils.ShopItemIndex, int> OnChangeUpgrade;

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

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.M))
            {
                IncreaseAmount(Utils.CurrencyType.Gold, 100);
            }
        }

        public void Payment(Utils.CurrencyType currencyType, Data.ShopItemData paymentData)
        {
            Payment(currencyType, paymentData, OnPaymentSuccess, OnPaymentFailure);
        }

        private void Payment(Utils.CurrencyType currencyType, Data.ShopItemData paymentData, UnityAction<Utils.CurrencyType, Data.ShopItemData> success, UnityAction<string> failure)
        {
            if (!userCurrencyData.ContainsKey(currencyType))
            {
                failure?.Invoke($"{currencyType} 재화 데이터가 없습니다.");
                return;
            }

            if (userCurrencyData[currencyType] < paymentData.Price)
            {
                failure?.Invoke("재화가 부족합니다.");
                return;
            }

            success?.Invoke(currencyType, paymentData);
        }

        private void OnPaymentSuccess(Utils.CurrencyType currencyType, Data.ShopItemData paymentData)
        {
            DecreaseAmount(currencyType, paymentData.Price);
            UpgradeAmount((Utils.ShopItemIndex)paymentData.Index);
        }

        private void OnPaymentFailure(string failureMessage)
        {
            Debug.LogWarning($"Payment Failure: {failureMessage}");
        }

        public void IncreaseAmount(Utils.CurrencyType currencyType, int addAmount)
        {
            if(!userCurrencyData.ContainsKey(currencyType))
            {
                Debug.LogWarning($"{currencyType} 재화 데이터가 없습니다.");
                return;
            }

            userCurrencyData[currencyType] += addAmount;

            OnChangeCurrency?.Invoke(currencyType, userCurrencyData[currencyType]);
        }

        public void DecreaseAmount(Utils.CurrencyType currencyType, int subAmount)
        {
            if (!userCurrencyData.ContainsKey(currencyType))
            {
                Debug.LogWarning($"{currencyType} 재화 데이터가 없습니다.");
                return;
            }

            userCurrencyData[currencyType] -= subAmount;

            if (userCurrencyData[currencyType] < 0)
            {
                userCurrencyData[currencyType] = 0;
            }

            OnChangeCurrency?.Invoke(currencyType, userCurrencyData[currencyType]);
        }

        public void UpgradeAmount(Utils.ShopItemIndex type)
        {
            if(!userUpgradeData.ContainsKey(type)) return;

            userUpgradeData[type]++;

            OnChangeUpgrade?.Invoke(type, userUpgradeData[type]);
        }

        public int GetUpgradeAmount(Utils.ShopItemIndex type)
        {
            if (!userUpgradeData.ContainsKey(type)) return -1;

            return userUpgradeData[type];
        }
    }
}