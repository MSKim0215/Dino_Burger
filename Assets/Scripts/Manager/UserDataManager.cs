using System;
using UnityEngine;
using UnityEngine.Events;

namespace MSKim.Manager
{
    [Serializable]
    public class UserDataManager : BaseManager
    {
        [Header("InGame Data Info")]
        [SerializeField] private int currentGoldAmount = 0;

        private PlayerData playerData = new();      // 사용자 데이터

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
            playerData = Managers.File.Load();
        }

        public override void OnUpdate()
        {
            if(Input.GetKeyDown(KeyCode.M))
            {
                IncreaseAmount(Utils.CurrencyType.Gold, 10000);
            }
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
            Managers.File.Save(playerData);
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
            Managers.File.Save(playerData);
        }

        public void UpgradeLevel(Utils.ShopItemIndex type)
        {
            if(!playerData.UserUpgradeData.ContainsKey(type)) return;

            playerData.UserUpgradeData[type]++;

            OnChangeUpgrade?.Invoke(type, playerData.UserUpgradeData[type]);
            Managers.File.Save(playerData);
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