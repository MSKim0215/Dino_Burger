using System;
using UnityEngine;

namespace MSKim.Manager
{
    public class CarManager : Spawner
    {
        private const int SPAWN_MIN_TIME = 3;
        private const int SPAWN_MAX_TIME = 6;

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
                    if(obj.TryGetComponent<NonPlayer.CarController>(out var car))
                    {
                        car?.Release();
                    }
                }
            }
        }

        public override void Remove(GameObject target)
        {
            if (activeObjectList.Count <= 0) return;
            if (!activeObjectList.Contains(target)) return;

            activeObjectList.Remove(target);
        }

        protected override int GetSpawnTime() => UnityEngine.Random.Range(SPAWN_MIN_TIME, SPAWN_MAX_TIME);

        protected override void SetSpawnPoint()
        {
            if (!spawnPoint.IsEmptyPoint())
            {
                spawnPoint.Clear();
            }

            var root = GameObject.Find("CarSpawnPoints").transform;

            for (int i = 0; i < root.childCount; i++)
            {
                spawnPoint.AddPoint(root.GetChild(i));
            }
        }

        protected override void Spawn()
        {
            var spawnObject = Managers.Pool.GetPoolObject("Car");

            if (spawnObject.TryGetComponent<NonPlayer.CarController>(out var car))
            {
                car.transform.position = spawnPoint.GetRandomPoint().position;
                activeObjectList.Add(spawnObject);

                var randType = UnityEngine.Random.Range(0, Enum.GetValues(typeof(Utils.CarType)).Length);
                car.Initialize((Utils.CarType)randType);
            }

            maxSpawnTime = GetSpawnTime();
            currSpawnTime = 0f;
        }
    }
}