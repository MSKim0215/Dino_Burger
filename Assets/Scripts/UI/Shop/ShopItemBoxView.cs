using MSKim.Manager;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace MSKim.UI
{
    [Serializable]
    public class ShopItemBoxView
    {
        private ShopItemBox controller;

        [SerializeField] private Image itemIcon = null;
        [SerializeField] private Text levelText = null;
        [SerializeField] private Text nameText = null;
        [SerializeField] private Text priceText = null;
        [SerializeField] private Button buyButton = null;

        public void Initialize(ShopItemBox controller)
        {
            this.controller = controller;
            SetData();
            BindEvent();
        }

        private void SetData()
        {
            controller.name = controller.Data.Name;
            itemIcon.color = UnityEngine.Random.ColorHSV();
            SetLevelText(Managers.UserData.GetUpgradeAmount((Utils.ShopItemIndex)controller.Data.Index));
            nameText.text = controller.Data.Name;
            priceText.text = string.Format("{0:#,0} 골드", controller.Data.Price);
        }

        public void SetLevelText(int currentLevel)
        {
            levelText.text = $"{currentLevel} / {controller.Data.MaximumLevel}";
        }

        private void BindEvent()
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(controller.OnUpgradeEvent);
        }
    }
}