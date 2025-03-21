using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class CarSpawner : Spawner
{
    [Header("Mesh List")]
    [SerializeField] protected List<Mesh> meshList = new();

    public Dictionary<Utils.CarType, Mesh> MeshDict { get; protected set; } = new();

    protected const int SPAWN_MIN_TIME = 2;
    protected const int SPAWN_MAX_TIME = 4;

    public override void Initialize()
    {
        base.Initialize();

        if (MeshDict.Count <= 0)
        {
            foreach (Utils.CarType carType in Enum.GetValues(typeof(Utils.CarType)))
            {
                string meshName = "car_" + carType.ToString().ToLower();
                var meshFilter = meshList.FirstOrDefault(mf => mf.name == meshName);

                if (meshFilter != null)
                {
                    MeshDict.Add(carType, meshFilter);
                }
                else
                {
                    Debug.LogWarning($"{meshName} MeshFilter를 찾을 수 없습니다.");
                }
            }
        }
    }

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

    protected override void Spawn() { }

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
                if (obj.TryGetComponent<MSKim.NonPlayer.CarController>(out var car))
                {
                    car?.Release();
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
}

namespace MSKim.Manager
{
    [Serializable]
    public class TitleCarManager : CarSpawner
    {
        protected override void Spawn()
        {
            var spawnObject = Managers.Pool.GetPoolObject("Title_Car");

            if (spawnObject.TryGetComponent<NonPlayer.CarController>(out var car))
            {
                // 0: R생성, 1: L생성
                var randomPointIndex = spawnPoint.GetRandomIndex();
                car.transform.position = spawnPoint.GetPoint(randomPointIndex).position;
                car.transform.rotation = Quaternion.Euler(0, (randomPointIndex == 0 ? -90 : 90), 0);

                activeObjectList.Add(spawnObject);

                var randType = UnityEngine.Random.Range(0, Enum.GetValues(typeof(Utils.CarType)).Length);
                car.Initialize((Utils.CarType)randType, (Utils.CarWaypointType)randomPointIndex);
            }

            maxSpawnTime = GetSpawnTime();
            currSpawnTime = 0f;
        }
    }

    [Serializable]
    public class CarManager : CarSpawner
    {
        protected override void Spawn()
        {
            var spawnObject = Managers.Pool.GetPoolObject("Car");

            if (spawnObject.TryGetComponent<NonPlayer.CarController>(out var car))
            {
                // 0: R생성, 1: L생성
                var randomPointIndex = spawnPoint.GetRandomIndex();
                car.transform.position = spawnPoint.GetPoint(randomPointIndex).position;
                car.transform.rotation = Quaternion.Euler(0, (randomPointIndex == 0 ? -90 : 90), 0);

                activeObjectList.Add(spawnObject);

                var randType = UnityEngine.Random.Range(0, Enum.GetValues(typeof(Utils.CarType)).Length);
                car.Initialize((Utils.CarType)randType, (Utils.CarWaypointType)randomPointIndex);
            }

            maxSpawnTime = GetSpawnTime();
            currSpawnTime = 0f;
        }
    }
}