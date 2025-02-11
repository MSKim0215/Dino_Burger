using MSKim.Manager;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MSKim.UI
{
    [Serializable]
    public class PurchasePopupView
    {
        private PurchasePopup controller;

        [SerializeField] private Button closeButton = null;
        [SerializeField] private TextMeshProUGUI infoText = null;
        [SerializeField] private Button cancelButton = null;
        [SerializeField] private Button purchaseButton = null;
        [SerializeField] private Image itemIcon = null;
        [SerializeField] private TextMeshProUGUI priceText = null;
        [SerializeField] private TextMeshProUGUI beforeText = null;
        [SerializeField] private TextMeshProUGUI afterText = null;
        [SerializeField] private TextMeshProUGUI currencyText = null;
        [SerializeField] private TextMeshProUGUI levelText = null;

        public void Initialize(PurchasePopup controller)
        {
            this.controller = controller;

            SetData();
            BindEvent();
        }

        public void SetData()
        {
            var currentLevel = Managers.UserData.GetUpgradeAmount((Utils.ShopItemIndex)this.controller.TargetData.Index);
            var nextLevel = currentLevel + 1;

            itemIcon.sprite = Managers.GameData.GetShopItemIcon(this.controller.TargetData.Index).Icon;
            infoText.text = this.controller.TargetData.Name;
            beforeText.text = (this.controller.TargetData.UpgradeAmount * currentLevel).ToString();
            afterText.text = (this.controller.TargetData.UpgradeAmount * nextLevel).ToString();
            priceText.text = string.Format("{0:#,0}", this.controller.TargetData.Price);
            SetCurrencyText(Managers.UserData.GetCurrencyAmount(Utils.CurrencyType.Gold));
            levelText.text = $"Lv.{currentLevel} / {this.controller.TargetData.MaximumLevel}";

            purchaseButton.interactable = Managers.UserData.GetCurrencyAmount(Utils.CurrencyType.Gold) >= controller.TargetData.Price;
        }

        private void BindEvent()
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(controller.OnExit);
            cancelButton.onClick.RemoveAllListeners();
            cancelButton.onClick.AddListener(controller.OnExit);
            purchaseButton.onClick.RemoveAllListeners();
            purchaseButton.onClick.AddListener(controller.OnPurchase);
        }

        public void SetCurrencyText(int currencyAmount)
        {
            currencyText.text = string.Format("{0:#,0}", currencyAmount);
        }
    }
}