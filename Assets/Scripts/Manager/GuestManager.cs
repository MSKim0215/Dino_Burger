using System;
using System.Collections.Generic;
using UnityEngine;

namespace MSKim.Manager
{
    [Serializable]
    public class GuestManager : BaseManager
    {
        [Header("Spawn Settings")]
        [SerializeField] private List<Transform> spawnPointList = new();
        [SerializeField] private float currSpawnTime;
        [SerializeField] private float maxSpawnTime;

        public List<NonPlayer.GuestController> guestList = new();

        public override void Initialize()
        {
            base.Initialize();

            if(spawnPointList.Count > 0)
            {
                spawnPointList.Clear();
            }

            if(spawnPointList.Count <= 0)
            {
                var spawnPointRoot = GameObject.Find("GuestSpawnPoints").transform;
                for (int i = 0; i < spawnPointRoot.childCount; i++)
                {
                    spawnPointList.Add(spawnPointRoot.GetChild(i));
                }
            }

            maxSpawnTime = UnityEngine.Random.Range(3, 6);
            currSpawnTime = maxSpawnTime;
        }

        public override void OnUpdate()
        {
            currSpawnTime += Time.deltaTime;

            if (currSpawnTime >= maxSpawnTime)
            {
                var spawnPoint = spawnPointList[UnityEngine.Random.Range(0, spawnPointList.Count)];
                var guest = Managers.Pool.GetPoolObject("Guest");
                guest.transform.position = new(spawnPoint.position.x, spawnPoint.position.y, UnityEngine.Random.Range(17, 20));

                if(guest.TryGetComponent<NonPlayer.GuestController>(out var controller))
                {
                    controller.Initialize();
                    guestList.Add(controller);
                }

                currSpawnTime = 0f;
                maxSpawnTime = UnityEngine.Random.Range(3, 6);
            }
        }
    }
}