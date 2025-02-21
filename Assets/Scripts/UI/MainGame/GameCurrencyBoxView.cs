using MSKim.Manager;
using System;
using TMPro;
using UnityEngine;

namespace MSKim.UI
{
    [Serializable]
    public class GameCurrencyBoxView
    {
        private GameCurrencyBox controller;

        [SerializeField] private TextMeshProUGUI currencyText = null;

        public void Initialize(GameCurrencyBox controller)
        {
            this.controller = controller;

            Managers.Game.OnChangeCurrencyEvent -= SetCurrencyText;
            Managers.Game.OnChangeCurrencyEvent += SetCurrencyText;
            Managers.Game.CurrentCoinAmount = 0;
        }

        private void SetCurrencyText(int currencyAmount) => currencyText.text = string.Format("{0:#,0}", currencyAmount);
    }
}