using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MSKim.UI
{
    [Serializable]
    public class OrderTicketView
    {
        private OrderTicket controller;

        [SerializeField] private TextMeshProUGUI tableNumberText = null;
        [SerializeField] private Slider timeSlider = null;
        [SerializeField] private HorizontalLayoutGroup divideGroup = null;
        [SerializeField] private GameObject stewGroup = null;

        public async void Initialize(OrderTicket controller)
        {
            this.controller = controller;

            tableNumberText.text = this.controller.TableNumber.ToString();
            stewGroup.SetActive(this.controller.IsOrderStew);

            await UniTask.Yield(PlayerLoopTiming.Update);

            var timerRect = (timeSlider.transform as RectTransform).rect;
            divideGroup.spacing = timerRect.width / 3f;
        }

        public void SetTimer(float value) => timeSlider.value = value;
    }
}