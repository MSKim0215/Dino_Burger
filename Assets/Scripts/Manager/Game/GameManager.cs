using System;
using System.Collections.Generic;
using UnityEngine;

namespace MSKim.Manager
{
    [Serializable]
    public class GameManager : BaseManager
    {
        private int currentCoinAmount = 0;          // 인게임 재화

        [Header("Settings")]
        [SerializeField] private List<Utils.CrateType> allowIncredientList = new();

        public event Action<int> OnChangeCurrencyEvent;    // 인게임 재화 수치 변경 이벤트

        public int CurrentCoinAmount
        {
            get => currentCoinAmount;
            set
            {
                currentCoinAmount = value;
                OnChangeCurrencyEvent?.Invoke(currentCoinAmount);
            }
        }

        public List<Utils.CrateType> AllowIncredientList { get => allowIncredientList; }

        public bool CanMovePickupTable => Guest.CurrentPickupGuestCount < Zone.GetPickupTableCount();

        public bool CanMoveWaitingChair => Guest.CurrentWaitingGuestCount < Zone.GetWaitChairCount();

        public ZoneManager Zone { get; private set; } = new();
        public GuestManager Guest { get; private set; } = new();
        public CarManager Car { get; private set; } = new();

        public int TotalOrderCount { get; set; } = 0;   // 총 주문 횟수
        public int SuccessOrderCount { get; set; } = 0; // 성공 주문 횟수

        public override void Initialize()
        {
            base.Initialize();

            Zone.Initialize();
            Guest.Initialize(Zone.GetPickupTableCount(), Zone.GetWaitChairCount());
            Car.Initialize();
        }

        public override void OnUpdate()
        {
            Guest?.OnUpdate();
            Car?.OnUpdate();

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Time.timeScale = Time.timeScale == 1f ? 5f : 1f;
            }
        }
    }
}