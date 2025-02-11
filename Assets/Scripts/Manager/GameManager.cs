using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MSKim.Manager
{
    [Serializable]
    public class GameManager : BaseManager
    {
        [Header("Settings")]
        [SerializeField] private List<GameObject> waitChairList = new();
        [SerializeField] private List<GameObject> pickupTableList = new();
        [SerializeField] private List<Utils.CrateType> allowIncredientList = new();

        [Header("Info Viewer")]
        [SerializeField] private int currentWaitNumber;
        [SerializeField] private List<NonPlayer.GuestController> pickupZoneGuests = new();
        [SerializeField] private Queue<NonPlayer.GuestController> waitingZoneGuests = new();
        [SerializeField] private bool[] canPickupSeats;
        [SerializeField] private bool[] canWaitSeats;

        public List<Utils.CrateType> AllowIncredientList { get => allowIncredientList; }

        public bool CanMovePickupTable => pickupZoneGuests.Count < pickupTableList.Count;

        public bool CanMoveWaitingChair => waitingZoneGuests.Count < waitChairList.Count;

        public bool IsExistWaitingGuest => waitingZoneGuests.Count >= 1;

        public override void Initialize()
        {
            base.Initialize();

            waitChairList.Clear();
            waitingZoneGuests.Clear();

            if(waitChairList.Count <= 0)
            {
                var waitSeatRoot = GameObject.Find("Chairs").transform;
                for (int i = 0; i < waitSeatRoot.childCount; i++)
                {
                    waitChairList.Add(waitSeatRoot.GetChild(i).gameObject);
                }
            }

            pickupTableList.Clear();
            pickupZoneGuests.Clear();

            if(pickupTableList.Count <= 0)
            {
                var pickupSeats = GameObject.FindGameObjectsWithTag("PickupTable");
                for (int i = 0; i < pickupSeats.Length; i++)
                {
                    pickupTableList.Add(pickupSeats[i]);
                }
            }

            canWaitSeats = new bool[waitChairList.Count];
            canPickupSeats = new bool[pickupTableList.Count];
            
            for(int i = 0; i < waitChairList.Count; i++)
            {
                canWaitSeats[i] = true;
            }

            for (int i = 0; i < pickupTableList.Count; i++)
            {
                canPickupSeats[i] = true;
            }
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Time.timeScale = Time.timeScale == 1f ? 5f : 1f;
            }
        }

        public void AddPickupZone(NonPlayer.GuestController guest)
        {
            guest.CurrentWaypointType = GetRandomPickupZoneType();
            pickupZoneGuests.Add(guest);
            guest.OrderTableNumber = int.Parse(guest.CurrentWaypointType.ToString()[^1..]);
        }

        private Utils.WaypointType GetRandomPickupZoneType()
        {
            while(CanMovePickupTable)
            {
                var randIndex = UnityEngine.Random.Range(0, pickupTableList.Count);

                if (canPickupSeats[randIndex])
                {
                    canPickupSeats[randIndex] = false;
                    return (Utils.WaypointType)Enum.Parse(typeof(Utils.WaypointType), $"PickupZone_{++randIndex}");
                }
            }
            return Utils.WaypointType.Outside_R;
        }

        public void RemovePickupZone(NonPlayer.GuestController guest)
        {
            pickupZoneGuests.Remove(guest);
            ResetPickupSeat(guest);
            guest.CurrentWaypointType = UnityEngine.Random.Range(0, 2) == 0 ? Utils.WaypointType.Pickup_Outside_L : Utils.WaypointType.Pickup_Outside_R;
        }

        private void ResetPickupSeat(NonPlayer.GuestController guest)
        {
            var typeIndex = int.Parse(guest.CurrentWaypointType.ToString().Last().ToString());
            canPickupSeats[typeIndex - 1] = true;
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

        public void RemoveWaitingZone()
        {
            RefreshNumberTicket();

            var nextGuest = waitingZoneGuests.Dequeue();
            ResetWaitingSeat(nextGuest);
            AddPickupZone(nextGuest);
        }

        private void ResetWaitingSeat(NonPlayer.GuestController guest)
        {
            var typeIndex = int.Parse(guest.CurrentWaypointType.ToString().Last().ToString());
            canWaitSeats[typeIndex - 1] = true;
        }

        private void RefreshNumberTicket()
        {
            var guestsArray = waitingZoneGuests.ToArray();
            waitingZoneGuests.Clear();

            for (int i = 0; i < guestsArray.Length; i++)
            {
                guestsArray[i].WaitingNumber--;
                waitingZoneGuests.Enqueue(guestsArray[i]);
            }
        }
    }
}