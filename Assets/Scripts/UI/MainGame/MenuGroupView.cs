using MSKim.Manager;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MSKim.UI
{
    [Serializable]
    public class MenuGroupView
    {
        private MenuGroup controller;

        [SerializeField] private Image menuIcon = null;
        [SerializeField] private TextMeshProUGUI priceText = null;

        public void Initialize(MenuGroup controller)
        {
            this.controller = controller;

            SetMenuIcon(Managers.GameData.GetIngredientIconData(this.controller.CurrentMenuType).UnCookIcon);

            var price = Managers.GameData.GetIngredientData(this.controller.CurrentMenuType).GuestSellPrice +
                Managers.UserData.GetUpgradeAmount(this.controller.CurrentMenuType);
            SetPriceText(price);
        }

        private void SetMenuIcon(Sprite icon)
        {
            menuIcon.sprite = icon;
        }

        private void SetPriceText(int currencyAmount)
        {
            if(currencyAmount <= 0)
            {
                priceText.text = "FREE";
                return;
            }

            priceText.text = string.Format("{0:#,0}", currencyAmount);
        }
    }
}