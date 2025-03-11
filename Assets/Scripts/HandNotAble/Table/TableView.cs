using System;
using UnityEngine;
using UnityEngine.UI;

namespace MSKim.HandNotAble.UI
{
    [Serializable]
    public class TableView
    {
        [Serializable]
        public class CanvasGroup
        {
            [SerializeField] protected Canvas canvas = null;

            protected virtual void SetActiveRoot(bool isActive)
            {
                if (canvas == null) return;
                if (canvas.gameObject.activeSelf == isActive) return;

                canvas.gameObject.SetActive(isActive);
            }
        }

        [Serializable]
        public class GaugeCanvas : CanvasGroup
        {
            [SerializeField] private Slider slider = null;
            [SerializeField] private Image fill = null;
            [SerializeField] private Color originColor = Color.white;
            [SerializeField] private Color changeColor = Color.white;

            protected override void SetActiveRoot(bool isActive)
            {
                if (canvas == null) return;
                if (canvas.gameObject.activeSelf == isActive) return;

                canvas.gameObject.SetActive(isActive);

                SetSliderValue(0f);
            }

            public void SetOriginActiveRoot(bool isActive)
            {
                SetActiveRoot(isActive);
                SetSliderColor(originColor);
            }

            public void SetChangeActiveRoot(bool isActive)
            {
                SetActiveRoot(isActive);
                SetSliderColor(changeColor);
            }

            public void SetSliderValue(float value)
            {
                slider.value = value;
            }

            private void SetSliderColor(Color color)
            {
                if(fill.color == color) return;

                fill.color = color;
            }
        }

        protected TableControllerUseUI controller;

        [SerializeField] protected GaugeCanvas gaugeCanvas = null;

        public virtual void Initialize<T>(T controller) where T : TableControllerUseUI
        {
            this.controller = controller as T;
            if (this.controller == null) return;

            this.controller.OnSetupOriginActiveEvent(gaugeCanvas.SetOriginActiveRoot);
            this.controller.OnSetupChangeActiveEvent(gaugeCanvas.SetChangeActiveRoot);
            this.controller.OnSetupValueEvent(gaugeCanvas.SetSliderValue);
        }
    }
}