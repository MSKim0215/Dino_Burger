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
        [SerializeField] private GameObject completeBurger;
        [SerializeField] private GameObject completeStew;

        public void Initialize(OrderTicket controller)
        {
            this.controller = controller;

            tableNumberText.text = this.controller.TableNumber.ToString();
            stewGroup.SetActive(this.controller.IsOrderStew);

            Refresh();
        }

        public async void Refresh()
        {
            await UniTask.Yield(PlayerLoopTiming.Update);

            var timerRect = (timeSlider.transform as RectTransform).rect;
            divideGroup.spacing = timerRect.width / 3f;

            var parent = controller.transform.parent;
            if (parent.name != "OrderTickets") return;

            LayoutRebuilder.ForceRebuildLayoutImmediate(parent as RectTransform);
        }

        public void SetTimer(float value) => timeSlider.value = value;

        public void SetBurgerComplete(bool isActive)
        {
            completeBurger.SetActive(isActive);
            controller.ReleaseGroupList(Utils.FoodType.Hamburger);

            Refresh();
        }

        public void SetStewComplete(bool isActive)
        {
            completeStew.SetActive(isActive);
            controller.ReleaseGroupList(Utils.FoodType.Stew);

            Refresh();
        }
    }
}