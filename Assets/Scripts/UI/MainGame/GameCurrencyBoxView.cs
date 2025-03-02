using MSKim.Manager;
using System;
using TMPro;
using UnityEngine;

namespace MSKim.UI
{
    [Serializable]
    public class GameCurrencyBoxView
    {
        [SerializeField] private TextMeshProUGUI currencyText = null;

        public void Initialize()
        {
            Managers.Game.OnChangeCurrencyEvent -= SetCurrencyText;
            Managers.Game.OnChangeCurrencyEvent += SetCurrencyText;
        }

        private void SetCurrencyText(int currencyAmount) => currencyText.CountingTo(currencyAmount, "{0:#,0}");
    }
}