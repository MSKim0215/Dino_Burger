using System;
using TMPro;
using UnityEngine;

namespace MSKim.UI
{
    [Serializable]
    public class GameTimerBoxView
    {
        private GameTimerBox controller;

        [SerializeField] private TextMeshProUGUI timerText = null;

        public void Initialize(GameTimerBox controller)
        {
            this.controller = controller;

            this.controller.OnTimerEvent -= SetTimer;
            this.controller.OnTimerEvent += SetTimer;
        }

        private void SetTimer(float value)
        {
            timerText.text = string.Format("{0:D2}:{1:D2}", (int)value / 60, (int)value % 60);
        }
    }
}