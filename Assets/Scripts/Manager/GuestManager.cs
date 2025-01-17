using System.Collections.Generic;
using UnityEngine;

namespace MSKim.Manager
{
    public class GuestManager : MonoBehaviour
    {
        [Header("Pool Settings")]
        [SerializeField] private NonPlayer.GuestController guestPrefab;
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
                var guest = Instantiate(guestPrefab, spawnPoint);
                guest.transform.position = spawnPoint.position;

                currSpawnTime = 0f;
                maxSpawnTime = Random.Range(3, 6);
            }
        }
    }
}