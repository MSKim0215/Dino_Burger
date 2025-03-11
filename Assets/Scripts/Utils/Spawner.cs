using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : BaseManager
{
    protected SpawnPointInfo spawnPoint = new();
    protected List<GameObject> activeObjectList = new();

    protected float currSpawnTime;
    protected float maxSpawnTime;

    public override void Initialize()
    {
        base.Initialize();

        maxSpawnTime = GetSpawnTime();
        currSpawnTime = maxSpawnTime;

        SetSpawnPoint();
    }

    public override void OnUpdate()
    {
        currSpawnTime += Time.deltaTime;

        if(currSpawnTime >= maxSpawnTime)
        {
            Spawn();
        }
    }

    protected abstract void SetSpawnPoint();
    protected abstract int GetSpawnTime();
    protected abstract void Spawn();
    public abstract void Clear();
    public abstract void Remove(GameObject target);
}