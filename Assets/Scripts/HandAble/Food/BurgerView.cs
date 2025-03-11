using MSKim.Manager;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MSKim.HandAble.UI
{
    [Serializable]
    public class BurgerView 
    {
        [Serializable]
        public class InputBox
        {
            [SerializeField] private Utils.CrateType type;
            [SerializeField] private GameObject box = null;
            [SerializeField] private Image icon = null;
            [SerializeField] private TextMeshProUGUI countText = null;
            private int count = 0;

            public Utils.CrateType Type => type;

            public void Initialize(Utils.CrateType type)
            {
                this.type = type;
                icon.sprite = Managers.GameData.GetIngredientIconData(this.type).CookIcon;
                AddCount();
                box.SetActive(true);
            }

            public void Release()
            {
                ClearCount();
                box.SetActive(false);
            }

            private void ClearCount() => count = 0;

            private void AddCount()
            {
                count++;
                SetCount(count);
            }

            private void SetCount(int count) => countText.text = $"x {count}";
        }

        private BurgerFoodController controller = null;

        [SerializeField] private List<InputBox> inputBoxList = new();

        public void Initialize(BurgerFoodController controller)
        {
            this.controller = controller;

            this.controller.OnStackIngredientEvent -= Stack;
            this.controller.OnStackIngredientEvent += Stack;
        }

        private void Stack(IngredientController inputIngredient)
        {
            var inputBox = inputBoxList.Find(box => box.Type == inputIngredient.IngredientType);
            if (inputBox == null) return;

            inputBox.Initialize(inputIngredient.IngredientType);
        }

        public void Release()
        {
            for(int i = 0; i < inputBoxList.Count; i++)
            {
                inputBoxList[i].Release();
            }
        }
    }
}