using System;
using UnityEngine;

namespace MSKim.Manager
{
    [Serializable]
    public class TitleManager : BaseManager
    {
        [Header("Game Manager List")]
        [SerializeField] private TitleCarManager titleCarManager = new();

        public TitleGuestManager TitleGuest { get; private set; } = new();
        public TitleCarManager TitleCar => titleCarManager;

        public override void Initialize()
        {
            base.Initialize();

            TitleGuest.Initialize();
            TitleCar.Initialize();
        }

        public override void OnUpdate()
        {
            if (!IsInit) return;

            TitleGuest.OnUpdate();
            TitleCar.OnUpdate();
        }
    }
}