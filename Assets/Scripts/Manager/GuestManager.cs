using System.Collections.Generic;
using UnityEngine;

namespace MSKim.Manager
{
    public class GuestManager : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [SerializeField] private List<Transform> spawnPointList = new();
        [SerializeField] private float currSpawnTime = 0f;
        [SerializeField] private float maxSpawnTime;

        private void Awake()
        {
            maxSpawnTime = Random.Range(3, 6);
        }

        private void Update()
        {
            currSpawnTime += Time.deltaTime;

            if(currSpawnTime >= maxSpawnTime)
            {
                var spawnPoint = spawnPointList[Random.Range(0, spawnPointList.Count)];
                var guest = ObjectPoolManager.instance.GetPoolObject("Guest");
                guest.transform.position = new(spawnPoint.position.x, spawnPoint.position.y, Random.Range(17, 20));
                guest.GetComponent<NonPlayer.GuestController>().Initialize();

                currSpawnTime = 0f;
                maxSpawnTime = Random.Range(3, 6);
            }
        }
    }
}