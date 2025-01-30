using MSKim.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MSKim.UI
{
    [Serializable]
    public class ShopView
    {
        [Serializable]
        private class CommonButton
        {
            [SerializeField] protected Button button = null;

            public void OnClickEvent(UnityAction action)
            {
                button.onClick.AddListener(() =>
                {
                    action?.Invoke();
                });
            }
        }

        [Serializable]
        private class TabButton : CommonButton
        {
            [SerializeField] private Utils.ShopTabType type = Utils.ShopTabType.Ingredient;

            public Utils.ShopTabType Type => type;

            public void OnClickEvent(UnityAction<Utils.ShopTabType> action)
            {
                button.onClick.AddListener(() =>
                {
                    action?.Invoke(type);
                });
            }

            public void ChangeColor(bool isActive)
            {
                if(isActive)
                {
                    button.image.color = Color.yellow;
                }
                else
                {
                    button.image.color = Color.white;
                }
            }
        }

        [SerializeField] private CommonButton exitButton;
        [SerializeField] private List<TabButton> tabButtonList = new();
        [SerializeField] private Transform shopItemBoxRoot;
        [SerializeField] private Text currencyText;

        private ShopPopup controller;

        public Transform ShopItemBoxRoot => shopItemBoxRoot;

        public void Initialize(ShopPopup controller)
        {
            this.controller = controller;
            BindEvent();
        }

        private void BindEvent()
        {
            exitButton.OnClickEvent(controller.OnExitEvent);

            foreach(var tabButton in tabButtonList)
            {
                tabButton.OnClickEvent(controller.OnTabEvent);
            }
        }

        public void ChangeTabButton(Utils.ShopTabType shopTabType)
        {
            foreach(var tabButton in tabButtonList)
            {
                tabButton.ChangeColor(tabButton.Type == shopTabType);
            }
        }

        public void SetCurrencyText(int currencyAmount)
        {
            currencyText.text = string.Format("{0:#,0}", currencyAmount);
        }
    }
}