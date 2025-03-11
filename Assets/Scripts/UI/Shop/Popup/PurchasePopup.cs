using MSKim.Manager;
using System;
using UnityEngine;

namespace MSKim.UI
{
    public class PurchasePopup : PoolAble
    {
        [Header("Buy Popup View")]
        [SerializeField] private PurchasePopupView view;

        public Data.ShopItemData TargetData { get; private set; }

        public void Initailize(Data.ShopItemData itemData)
        {
            TargetData = itemData;

            view.Initialize(this);

            Managers.UserData.OnChangeCurrencyDataEvent -= HandleChangeCurrency;
            Managers.UserData.OnChangeCurrencyDataEvent += HandleChangeCurrency;
        }

        public void OnExit()
        {
            Release();
        }

        public void OnPurchase()
        {
            Managers.UserData.Payment(Utils.CurrencyType.Gold, TargetData);
            view.SetData();
        }

        private void HandleChangeCurrency(Utils.CurrencyType currencyType, int currencyAmount)
        {
            view.SetCurrencyText(currencyAmount);
        }
    }
}