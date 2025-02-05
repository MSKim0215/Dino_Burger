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

        protected TableController controller;

        [SerializeField] protected GaugeCanvas gaugeCanvas = null;

        public virtual void Initialize(TableController controller)
        {
            this.controller = controller;
        }
    }
}