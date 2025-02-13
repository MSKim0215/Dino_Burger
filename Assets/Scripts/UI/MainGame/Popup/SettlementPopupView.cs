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
        }
    }
}