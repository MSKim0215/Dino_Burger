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

            public virtual void SetActiveRoot(bool isActive)
            {
                if (canvas.gameObject.activeSelf == isActive) return;

                canvas.gameObject.SetActive(isActive);
            }
        }

        [Serializable]
        public class GaugeCanvas : CanvasGroup
        {
            [SerializeField] private Slider slider = null;

            public override void SetActiveRoot(bool isActive)
            {
                base.SetActiveRoot(isActive);

                SetSliderValue(0f);
            }

            public void SetSliderValue(float value) => slider.value = value;
        }

        private TableControllerUseUI controller;

        [SerializeField] private GaugeCanvas gaugeCanvas = null;

        public void Initialize<T>(T controller) where T : TableControllerUseUI
        {
            this.controller = controller as T;
            if (this.controller == null) return;

            this.controller.OnSetUpActiveEvent(gaugeCanvas.SetActiveRoot);
            this.controller.OnSetUpValueEvent(gaugeCanvas.SetSliderValue);
        }
    }
}