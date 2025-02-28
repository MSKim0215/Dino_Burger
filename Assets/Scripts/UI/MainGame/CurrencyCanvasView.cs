using MSKim.Manager;
using System;
using TMPro;
using UnityEngine;

namespace MSKim.UI
{
    [Serializable]
    public class CurrencyCanvasView
    {
        [SerializeField] private GameObject menuGroup;
        [SerializeField] private TextMeshProUGUI priceText;

        private Vector3 startPosition;

        public GameObject MenuGroup => menuGroup;

        public void Initialize()
        {
            startPosition = menuGroup.transform.position;
        }

        public void SetPriceText(int currencyAmount)
        {
            priceText.text = string.Format("+{0:#,0}", currencyAmount);
        }

        public void ResetGroup()
        {
            menuGroup.transform.position = startPosition;
        }
    }
}