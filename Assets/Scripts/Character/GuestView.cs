using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MSKim.NonPlayer.UI
{
    [Serializable]
    public class GuestView : CharacterView
    {
        [Serializable]
        private class BaseCanvas
        {
            [SerializeField] protected GameObject canvas;

            public void SetActiveCanvas(bool isActive) => canvas.SetActive(isActive);
        }

        [Serializable]
        private class WaitingCanvas : BaseCanvas
        {
            [SerializeField] private TextMeshProUGUI numberText;

            public void SetNumber(int number) => numberText.text = number.ToString();
        }

        [Serializable]
        private class OrderCanvas : BaseCanvas
        {
            [SerializeField] private TextMeshProUGUI numberText;
            [SerializeField] private Slider timer;

            public void SetNumber(int number) => numberText.text = number.ToString();

            public void SetSliderValue(float value) => timer.value = value;
        }

        [Header("Canvas Settings")]
        [SerializeField] private WaitingCanvas waiting = null;
        [SerializeField] private OrderCanvas order = null;

        private GuestController controller = null;

        public void Initialize(GuestController controller)
        {
            this.controller = controller;

            BindEvent();
        }

        private void BindEvent()
        {
            this.controller.OnChangeWaitingNumber -= waiting.SetNumber;
            this.controller.OnChangeWaitingNumber += waiting.SetNumber;

            this.controller.OnChangeOrderTableNumber -= order.SetNumber;
            this.controller.OnChangeOrderTableNumber += order.SetNumber;

            this.controller.OnDelayOrderEvent -= order.SetSliderValue;
            this.controller.OnDelayOrderEvent += order.SetSliderValue;
        }

        public void StartWait()
        {
            waiting.SetActiveCanvas(true);
        }

        public void StartOrder()
        {
            waiting.SetActiveCanvas(false);
            order.SetActiveCanvas(true);
        }

        public void Release()
        {
            waiting.SetActiveCanvas(false);
            order.SetActiveCanvas(false);
        }
    }
}