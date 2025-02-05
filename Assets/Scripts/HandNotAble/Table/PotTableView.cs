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

            public void Input()
            {
                var inputBox = inputBoxList.Find(box => !box.IsInput);
                if (inputBox == null) return;

                inputBox.Input();
            }
        }

        [Serializable]
        public class InputBox
        {
            [SerializeField] private GameObject box = null;
            [SerializeField] private Image icon = null;

            public bool IsInput { get; private set; } = false;

            public void Input()
            {
                IsInput = true;
                SetActiveIcon(true);
            }

            public void SetActiveIcon(bool isAcitve) => icon.gameObject.SetActive(isAcitve);

            public void SetIcon(Sprite iconSprite) => icon.sprite = iconSprite;
        }

        [SerializeField] private SpeechCanvas speechCanvas = null;

        public override void Initialize<T>(T controller)
        {
            base.Initialize(controller);

            this.controller.OnSetUpTakeIngredientEvent(speechCanvas.Input);
        }
    }
}