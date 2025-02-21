using MSKim.Manager;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MSKim.UI
{
    public class SettlementPopup : PoolAble
    {
        [Header("Settlement View")]
        [SerializeField] private SettlementPopupView view;

        public void Initialize()
        {
            Time.timeScale = 0f;
            view.Initialize(this);
        }

        public void OnClaim()
        {
            Managers.UserData.IncreaseAmount(Utils.CurrencyType.Gold, Managers.Game.CurrentCoinAmount);

            Managers.Game.CurrentCoinAmount = 0;
            Managers.Game.SuccessOrderCount = 0;
            Managers.Game.TotalOrderCount = 0;

            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }
    }
}