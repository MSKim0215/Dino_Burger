using MSKim.Manager;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MSKim.HandNotAble.UI
{
    [Serializable]
    public class PotTableView : TableView
    {
        [Serializable]
        public class CompleteCanvas : CanvasGroup
        {
            public void Active()
            {
                SetActiveRoot(true);
            }

            public void UnActive()
            {
                SetActiveRoot(false);
            }
        }

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
        public class CountCanvas : CanvasGroup
        {
            [SerializeField] private TextMeshProUGUI countText;

            public void SetCountText(int amount)
            {
                SetActiveRoot(amount > 0);
                countText.text = amount.ToString();
            }
        }

        [Serializable]
        public class InputBox
        {
            [SerializeField] private GameObject frame = null;
            [SerializeField] private Image icon = null;

            public bool IsInput { get; private set; } = false;

            public void Input(Utils.CrateType ingredientType)
            {
                IsInput = true;
                frame.SetActive(IsInput);
                icon.sprite = Managers.GameData.GetIngredientIconData(ingredientType).UnCookIcon;
            }

            public void Output()
            {
                IsInput = false;
                frame.SetActive(IsInput);
            }
        }

        [SerializeField] private SpeechCanvas speechCanvas = null;
        [SerializeField] private CompleteCanvas completeCanvas = null;
        [SerializeField] private CountCanvas countCanvas = null;

        public override void Initialize<T>(T controller)
        {
            base.Initialize(controller);

            this.controller.OnSetupInputIngredientEvent(speechCanvas.Input);
            this.controller.OnSetupOutputIngredientEvent(speechCanvas.Output);

            this.controller.OnSetupValueCompleteEvent(completeCanvas.Active);
            this.controller.OnSetupOutputIngredientEvent(completeCanvas.UnActive);
        }

        public void SetCountText(int amount) => countCanvas.SetCountText(amount);
    }
}