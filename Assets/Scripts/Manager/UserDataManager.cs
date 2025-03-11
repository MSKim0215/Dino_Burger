using System;
using UnityEngine;
using UnityEngine.Events;

namespace MSKim.Manager
{
    [Serializable]
    public class UserDataManager : BaseManager
    {
        private PlayerData playerData = new();      // 사용자 데이터

        public event Action<Utils.CurrencyType, int> OnChangeCurrencyDataEvent;     // 재화 수치 변경 이벤트
        public event Action<Utils.ShopItemIndex, int> OnChangeUpgradeDataEvent;     // 강화 수치 변경 이벤트

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

            SaveCurrencyData(currencyType);
        }

        public void DecreaseAmount(Utils.CurrencyType currencyType, int subAmount)
        {
            if (!playerData.UserCurrencyData.ContainsKey(currencyType))
            {
                Debug.LogWarning($"{currencyType} 재화 데이터가 없습니다.");
                return;
            }

            playerData.UserCurrencyData[currencyType] = Mathf.Max(0, playerData.UserCurrencyData[currencyType] - subAmount);

            SaveCurrencyData(currencyType);
        }

        private void SaveCurrencyData(Utils.CurrencyType currencyType)
        {
            OnChangeCurrencyDataEvent?.Invoke(currencyType, playerData.UserCurrencyData[currencyType]);
            Managers.File.Save(playerData);
        }

        public void UpgradeLevel(Utils.ShopItemIndex itemType)
        {
            if(!playerData.UserUpgradeData.ContainsKey(itemType)) return;

            playerData.UserUpgradeData[itemType]++;          
            SaveUpgradeData(itemType);
        }

        private void SaveUpgradeData(Utils.ShopItemIndex itemType)
        {
            OnChangeUpgradeDataEvent?.Invoke(itemType, playerData.UserUpgradeData[itemType]);
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