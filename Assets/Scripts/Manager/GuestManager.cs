using System;
using System.Collections.Generic;
using UnityEngine;

namespace MSKim.Manager
{
    [Serializable]
    public class GuestManager : BaseManager
    {
        private class SpawnPointInfo
        {
            private List<Transform> points = new();

            public bool IsEmptyPoint() => points.Count <= 0;

            public void Clear() => points.Clear();

            public void AddPoint(Transform point) => points.Add(point);

            public Transform GetRandomPoint() => points[UnityEngine.Random.Range(0, points.Count)];
        }

        private readonly SpawnPointInfo spawnPoint = new();
        private List<NonPlayer.GuestController> activeGuestList = new();
        private float currSpawnTime;
        private float maxSpawnTime;

        private const int SPAWN_MIN_TIME = 3;
        private const int SPAWN_MAX_TIME = 6;

        private const int SPAWN_MIN_POSITION = 17;
        private const int SPAWN_MAX_POSITION = 20;

        public override void Initialize()
        {
            base.Initialize();

            SetSpawnPoint();

            maxSpawnTime = GetSpawnTime();
            currSpawnTime = maxSpawnTime;
        }

        private void SetSpawnPoint()
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

        public override void OnUpdate()
        {
            currSpawnTime += Time.deltaTime;

            if (currSpawnTime >= maxSpawnTime)
            {
                Spawn();
            }
        }

        private void Spawn()
        {
            var spawnObject = Managers.Pool.GetPoolObject("Guest");

            if(spawnObject.TryGetComponent<NonPlayer.GuestController>(out var guest))
            {
                guest.transform.position = GetSpawnPosition();
                guest.Initialize();
                activeGuestList.Add(guest);
            }

            maxSpawnTime = GetSpawnTime();
            currSpawnTime = 0f;
        }

        private Vector3 GetSpawnPosition()
        {
            var spawnPoint = this.spawnPoint.GetRandomPoint();
            return new(spawnPoint.position.x, spawnPoint.position.y, UnityEngine.Random.Range(SPAWN_MIN_POSITION, SPAWN_MAX_POSITION));
        }

        private int GetSpawnTime()
        {
            return UnityEngine.Random.Range(SPAWN_MIN_TIME, SPAWN_MAX_TIME);
        }

        public void Clear()
        {
            for (int i = activeGuestList.Count - 1; i >= 0; i--)
            {
                var guest = activeGuestList[i];

                if (guest == null)
                {
                    RemoveActiveGuest(guest);
                }
                else
                {
                    guest?.Release();
                }
            }
        }

        public void RemoveActiveGuest(NonPlayer.GuestController removeTarget)
        {
            if (activeGuestList.Count <= 0) return;
            if (!activeGuestList.Contains(removeTarget)) return;

            activeGuestList.Remove(removeTarget);
        }
    }
}