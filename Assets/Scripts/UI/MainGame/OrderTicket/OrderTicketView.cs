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
        [SerializeField] private GameObject stewGroup = null;

        public void Initialize(OrderTicket controller)
        {
            this.controller = controller;

            tableNumberText.text = this.controller.TableNumber.ToString();
            stewGroup.SetActive(this.controller.IsOrderStew);
        }

        public void SetTimer(float value) => timeSlider.value = value;
    }
}