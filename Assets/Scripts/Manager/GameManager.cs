using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MSKim.Manager
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;

        public static GameManager Instance
        {
            get
            {
                if (instance == null) instance = new();
                return instance;
            }
        }

        [Header("Settings")]
        [SerializeField] private List<GameObject> waitChairList = new();
        [SerializeField] private List<GameObject> pickupTableList = new();

        [Header("Info Viewer")]
        [SerializeField] private int currentWaitNumber;
        [SerializeField] private Queue<NonPlayer.GuestController> pickupZoneGuests = new();
        [SerializeField] private Queue<NonPlayer.GuestController> waitingZoneGuests = new();
        [SerializeField] private bool[] canWaitSeats;

        public bool CanMovePickupTable => pickupZoneGuests.Count < pickupTableList.Count;

        public bool CanMoveWaitingChair => waitingZoneGuests.Count < waitChairList.Count;

        private void Awake()
        {
            if(instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            canWaitSeats = new bool[waitChairList.Count];
            
            for(int i = 0; i < waitChairList.Count; i++)
            {
                canWaitSeats[i] = true;
            }
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                Time.timeScale = Time.timeScale == 1f ? 5f : 1f;
            }
        }

        public void AddPickupZone(NonPlayer.GuestController guest)
        {
            pickupZoneGuests.Enqueue(guest);
            guest.CurrentWaypointType = (Utils.WaypointType)Enum.Parse(typeof(Utils.WaypointType), $"PickupZone_{pickupZoneGuests.Count}");
        }

        public void AddWaitingZone(NonPlayer.GuestController guest)
        {
            guest.CurrentWaypointType = GetRandomWaitingZoneType();
            waitingZoneGuests.Enqueue(guest);
            guest.WaitingNumber = waitingZoneGuests.Count;
        }

        private Utils.WaypointType GetRandomWaitingZoneType()
        {
            while(CanMoveWaitingChair)
            {
                var randIndex = UnityEngine.Random.Range(0, waitChairList.Count);

                if (canWaitSeats[randIndex])
                {
                    canWaitSeats[randIndex] = false;
                    return (Utils.WaypointType)Enum.Parse(typeof(Utils.WaypointType), $"WaitingZone_{++randIndex}");
                }
            }
            return Utils.WaypointType.Outside_L;
        }
    }
}