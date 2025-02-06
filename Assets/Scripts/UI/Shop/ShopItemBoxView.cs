using MSKim.Manager;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MSKim.UI
{
    [Serializable]
    public class ShopItemBoxView
    {
        private ShopItemBox controller;

        [SerializeField] private Image itemIcon = null;
        [SerializeField] private TextMeshProUGUI levelText = null;
        [SerializeField] private TextMeshProUGUI nameText = null;
        [SerializeField] private TextMeshProUGUI priceText = null;
        [SerializeField] private Button buyButton = null;

        public void Initialize(ShopItemBox controller)
        {
            this.controller = controller;
            SetData();
            BindEvent();
        }

        private void SetData()
        {
            itemIcon.color = UnityEngine.Random.ColorHSV();
            itemIcon.sprite = Managers.GameData.GetShopItemIcon(controller.Data.Index).Icon;
            SetLevelText(Managers.UserData.GetUpgradeAmount((Utils.ShopItemIndex)controller.Data.Index));
            nameText.text = controller.Data.Name;
            priceText.text = string.Format("{0:#,0}", controller.Data.Price);
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