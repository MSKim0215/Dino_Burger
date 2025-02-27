using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MSKim.Manager
{
    [Serializable]
    public class GuestManager : Spawner
    {
        private List<NonPlayer.GuestController> pickupZoneGuestList = new();
        private Queue<NonPlayer.GuestController> waitingZoneGuestQueue = new();

        private bool[] canPickupSeats;
        private bool[] canWaitSeats;

        private int pickupCount;
        private int waitCount;

        private const int SPAWN_MIN_TIME = 3;
        private const int SPAWN_MAX_TIME = 7;

        private const int SPAWN_MIN_POSITION = 17;
        private const int SPAWN_MAX_POSITION = 22;

        public int CurrentPickupGuestCount => pickupZoneGuestList.Count;

        public int CurrentWaitingGuestCount => waitingZoneGuestQueue.Count;

        public bool IsExistWaitingGuest => waitingZoneGuestQueue.Count >= 1;

        public void Initialize(int pickupCount, int waitCount)
        {
            base.Initialize();

            this.pickupCount = pickupCount;
            this.waitCount = waitCount;

            SetSeat();
        }

        protected override void SetSpawnPoint()
        {
            if (!spawnPoint.IsEmptyPoint())
            {
                spawnPoint.Clear();
            }

            var root = GameObject.Find("GuestSpawnPoints").transform;

            for (int i = 0; i < root.childCount; i++)
            {
                spawnPoint.AddPoint(root.GetChild(i));
            }
        }

        private void SetSeat()
        {
            canPickupSeats = new bool[pickupCount];
            canWaitSeats = new bool[waitCount];

            for (int i = 0; i < pickupCount; i++)
            {
                canPickupSeats[i] = true;
            }

            for (int i = 0; i < waitCount; i++)
            {
                canWaitSeats[i] = true;
            }
        }

        protected override void Spawn()
        {
            var spawnObject = Managers.Pool.GetPoolObject("Guest");

            if(spawnObject.TryGetComponent<NonPlayer.GuestController>(out var guest))
            {
                guest.transform.position = GetSpawnPosition();
                guest.Initialize();
                activeObjectList.Add(spawnObject);
            }

            maxSpawnTime = GetSpawnTime();
            currSpawnTime = 0f;
        }

        private Vector3 GetSpawnPosition()
        {
            var spawnPoint = this.spawnPoint.GetRandomPoint();
            return new(spawnPoint.position.x, spawnPoint.position.y, UnityEngine.Random.Range(SPAWN_MIN_POSITION, SPAWN_MAX_POSITION));
        }

        protected override int GetSpawnTime()
        {
            return UnityEngine.Random.Range(SPAWN_MIN_TIME, SPAWN_MAX_TIME);
        }

        public override void Clear()
        {
            for (int i = activeObjectList.Count - 1; i >= 0; i--)
            {
                var obj = activeObjectList[i];

                if (obj == null)
                {
                    Remove(obj);
                }
                else
                {
                    if (obj.TryGetComponent<NonPlayer.GuestController>(out var guest))
                    {
                        guest?.Release();
                    }
                }
            }
        }

        public override void Remove(GameObject removeTarget)
        {
            if (activeObjectList.Count <= 0) return;
            if (!activeObjectList.Contains(removeTarget)) return;

            activeObjectList.Remove(removeTarget);
        }

        public void AddPickupZone(NonPlayer.GuestController guest)
        {
            guest.CurrentWaypointType = GetRandomPickupZoneType();
            pickupZoneGuestList.Add(guest);
            guest.OrderTableNumber = int.Parse(guest.CurrentWaypointType.ToString()[^1..]);
        }

        private Utils.WaypointType GetRandomPickupZoneType()
        {
            while (CurrentPickupGuestCount < pickupCount)
            {
                var randIndex = UnityEngine.Random.Range(0, pickupCount);

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
            pickupZoneGuestList.Remove(guest);
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
            waitingZoneGuestQueue.Enqueue(guest);
            guest.WaitingNumber = waitingZoneGuestQueue.Count;
        }

        private Utils.WaypointType GetRandomWaitingZoneType()
        {
            while (CurrentWaitingGuestCount < waitCount)
            {
                var randIndex = UnityEngine.Random.Range(0, waitCount);

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

            var nextGuest = waitingZoneGuestQueue.Dequeue();
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
            var guestsArray = waitingZoneGuestQueue.ToArray();
            waitingZoneGuestQueue.Clear();

            for (int i = 0; i < guestsArray.Length; i++)
            {
                guestsArray[i].WaitingNumber--;
                waitingZoneGuestQueue.Enqueue(guestsArray[i]);
            }
        }
    }
}