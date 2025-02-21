using MSKim.Manager;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MSKim.UI
{
    [Serializable]
    public class SettlementPopupView
    {
        private SettlementPopup controller;

        [SerializeField] private Slider percentSlider;
        [SerializeField] private TextMeshProUGUI percentText;
        [SerializeField] private TextMeshProUGUI visitText;
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private Button claimButton;
    
        public void Initialize(SettlementPopup controller)
        {
            this.controller = controller;

            var percent = (float)Managers.Game.SuccessOrderCount / Managers.Game.TotalOrderCount;
            percentSlider.value = percent;
            percentText.text = string.Format("{0:P1}", percent);
            visitText.text = $"{Managers.Game.SuccessOrderCount} <#9aa5d1>/ {Managers.Game.TotalOrderCount}";
            valueText.text = string.Format("{0:#,0}", Managers.Game.CurrentCoinAmount);

            claimButton.onClick.AddListener(this.controller.OnClaim);
        }
    }
}