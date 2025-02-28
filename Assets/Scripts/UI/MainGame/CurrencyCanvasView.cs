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

        public GameObject MenuGroup => menuGroup;

        public void SetPriceText(int currencyAmount)
        {
            priceText.text = string.Format("{0:#,0}", currencyAmount);
        }
    }
}