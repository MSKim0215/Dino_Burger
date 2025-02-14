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
        [SerializeField] private GameObject nextGroup = null;

        public void Initialize(PurchasePopup controller)
        {
            this.controller = controller;

            SetData();
            BindEvent();
        }

        public void SetData()
        {
            SetCurrencyText(Managers.UserData.GetCurrencyAmount(Utils.CurrencyType.Gold));

            var currentLevel = Managers.UserData.GetUpgradeLevel((Utils.ShopItemIndex)controller.TargetData.Index);

            itemIcon.sprite = Managers.GameData.GetShopItemIcon(controller.TargetData.Index).Icon;
            levelText.text = $"Lv.{currentLevel} / {controller.TargetData.MaximumLevel}";
            infoText.text = controller.TargetData.Name;
            beforeText.text = (controller.TargetData.UpgradeAmount * currentLevel).ToString();
            nextGroup.SetActive(currentLevel < controller.TargetData.MaximumLevel);

            bool isMaxLevel = currentLevel >= controller.TargetData.MaximumLevel;
            if (!isMaxLevel)
            {
                var nextLevel = currentLevel + 1;
                afterText.text = (controller.TargetData.UpgradeAmount * nextLevel).ToString();
            }

            priceText.text = isMaxLevel ? "MAX" : string.Format("{0:#,0}", this.controller.TargetData.Price);

            bool isActiveButton = 
                (Managers.UserData.GetCurrencyAmount(Utils.CurrencyType.Gold) >= controller.TargetData.Price) &&
                (currentLevel < controller.TargetData.MaximumLevel);
            purchaseButton.interactable = isActiveButton;
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