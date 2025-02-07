using MSKim.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MSKim.HandNotAble.UI
{
    [Serializable]
    public class PotTableView : TableView
    {
        [Serializable]
        public class SpeechCanvas : CanvasGroup
        {
            [SerializeField] private List<InputBox> inputBoxList = new();

            public void Input(Utils.CrateType ingredientType)
            {
                var inputBox = inputBoxList.Find(box => !box.IsInput);
                if (inputBox == null) return;

                inputBox.Input(ingredientType);
            }

            public void Output()
            {
                for(int i = 0; i < inputBoxList.Count; i++)
                {
                    inputBoxList[i].Output();
                }
            }
        }

        [Serializable]
        public class InputBox
        {
            [SerializeField] private GameObject box = null;
            [SerializeField] private GameObject frame = null;
            [SerializeField] private Image icon = null;

            public bool IsInput { get; private set; } = false;

            public void Input(Utils.CrateType ingredientType)
            {
                IsInput = true;
                frame.SetActive(IsInput);
                icon.sprite = Managers.GameData.GetIngredientIconData(ingredientType).Icon;
            }

            public void Output()
            {
                IsInput = false;
                frame.SetActive(IsInput);
            }
        }

        [SerializeField] private SpeechCanvas speechCanvas = null;

        public override void Initialize<T>(T controller)
        {
            base.Initialize(controller);

            this.controller.OnSetupInputIngredientEvent(speechCanvas.Input);
            this.controller.OnSetupOutputIngredientEvent(speechCanvas.Output);
        }
    }
}