using Cysharp.Threading.Tasks;
using MSKim.Manager;
using System;
using System.Collections.Generic;
using TMPro;
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
            [SerializeField] private TextMeshProUGUI text = null;
            [SerializeField] private Utils.ShopTabType type = Utils.ShopTabType.Ingredient;
            
            public Utils.ShopTabType Type => type;


            public void OnClickEvent(UnityAction<Utils.ShopTabType> action)
            {
                button.onClick.AddListener(() =>
                {
                    action?.Invoke(type);
                });
            }

            public void ChangeColor(Color buttonColor, Color textColor)
            {
                button.image.color = buttonColor;
                text.color = textColor;
            }
        }

        [SerializeField] private CommonButton exitButton;
        [SerializeField] private List<TabButton> tabButtonList = new();
        [SerializeField] private TextMeshProUGUI currencyText;
        [SerializeField] private Transform itemBoxRoot = null;
        [SerializeField] private ScrollRect scrollRect = null;
        [SerializeField] private TextMeshProUGUI titleBarText;

        [Header("Tab Button Color Settings")]
        [SerializeField] private Color activeButtonColor;
        [SerializeField] private Color unactiveButtonColor;
        [SerializeField] private Color activeTextColor;
        [SerializeField] private Color unactiveTextColor;

        private ShopPopup controller;

        public Transform ItemBoxRoot => itemBoxRoot;

        public void Initialize(ShopPopup controller)
        {
            this.controller = controller;

            SetCurrencyText(Managers.UserData.GetCurrencyAmount(Utils.CurrencyType.Gold));

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

        public async void ChangeTabButton(Utils.ShopTabType shopTabType)
        {
            foreach(var tabButton in tabButtonList)
            {
                if(tabButton.Type == shopTabType)
                {
                    titleBarText.text = shopTabType.ToString().ToUpper();
                    tabButton.ChangeColor(activeButtonColor, activeTextColor);

                    await UniTask.NextFrame(PlayerLoopTiming.PostLateUpdate);

                    scrollRect.verticalNormalizedPosition = 1f;
                    LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
                }
                else
                {
                    tabButton.ChangeColor(unactiveButtonColor, unactiveTextColor);
                }
            }
        }

        public void SetCurrencyText(int currencyAmount)
        {
            currencyText.CountingTo(currencyAmount, "{0:#,0}");
        }
    }
}