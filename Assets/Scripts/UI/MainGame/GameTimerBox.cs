using Cysharp.Threading.Tasks;
using MSKim.Manager;
using System;
using UnityEngine;

namespace MSKim.UI
{
    public class GameTimerBox : MonoBehaviour
    {
        [Header("TimerBox View")]
        [SerializeField] private GameTimerBoxView view;

        private float currentTime = 0f;

        public event Action<float> OnTimerEvent;

        private const float MAXIMUM_TIME = 210;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            currentTime = MAXIMUM_TIME;

            StartTimer();
            view.Initialize(this);
        }

        private async void StartTimer()
        {
            while(true)
            {
                currentTime -= Time.deltaTime;
                OnTimerEvent?.Invoke(currentTime);

                await UniTask.Yield();

                if(currentTime <= 0f)
                {
                    Managers.Game.Guest.Clear();

                    currentTime = 0f;

                    var settlement = Managers.Pool.GetPoolObject("SettlementPopupUI");
                    settlement.transform.SetParent(GameObject.Find("MainGameCanvas").transform);
                    settlement.transform.localScale = Vector3.one;
                    settlement.transform.localPosition = Vector3.zero;

                    if(settlement.TryGetComponent<SettlementPopup>(out var popup))
                    {
                        popup.Initialize();
                    }

                    break;
                }
            }
        }
    }
}