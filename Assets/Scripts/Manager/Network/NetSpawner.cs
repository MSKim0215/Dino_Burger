using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject spawnPrefab;

    protected float currSpawnTime;
    protected float maxSpawnTime;

    private void Start()
    {
        maxSpawnTime = 3f;
        currSpawnTime = maxSpawnTime;
    }

    private void Update()
    {
        currSpawnTime += Time.deltaTime;

        if (currSpawnTime >= maxSpawnTime)
        {
            if (IsClient)
            {
                SpawnServerRpc(NetworkManager.Singleton.LocalClientId);
            }
            else if (IsServer)
            {
                Spawn(NetworkManager.Singleton.LocalClientId);
            }

            currSpawnTime = 0f;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnServerRpc(ulong clientId)
    {
        Spawn(clientId);
    }

    private void Spawn(ulong clientId)
    {
        var spawnObj = Instantiate(spawnPrefab);
        var netObj = spawnObj.GetComponent<NetworkObject>();
        netObj.Spawn();

        SetSpawnPointClientRpc(netObj.NetworkObjectId, clientId);
    }

    [ClientRpc]
    private void SetSpawnPointClientRpc(ulong networkObjectId, ulong clientId)
    {
        if(NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(networkObjectId, out var carNetObject))
        {
            if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                carNetObject.transform.position = new(45, 1, 33);
            }
            else
            {
                carNetObject.transform.position = new(-45, 1, 33);
            }
        }
    }
}
