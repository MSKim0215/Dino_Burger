using MSKim.Manager;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MSKim.UI
{
    [Serializable]
    public class OrderIngredientView
    {
        private OrderIngredientController controller;

        [SerializeField] private Image ingredientIcon = null;
        [SerializeField] private TextMeshProUGUI countText = null;

        public void Initialize(OrderIngredientController controller)
        {
            this.controller = controller;

            ingredientIcon.sprite = Managers.GameData.GetIngredientIconData(this.controller.IngredientType).CookIcon;
        }

        public void SetCountText(int value) => countText.text = value.ToString();
    }
}