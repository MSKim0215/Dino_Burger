using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace MSKim.Manager
{
    [Serializable]
    public class ObjectPoolManager : BaseManager
    {
        [Serializable]
        private class ObjectInfo
        {
            public string createObjectName;
            public Utils.PoolType poolType;
            public Utils.SceneType useScene;
            public GameObject createPrefab;
            public int createCount;
            public int maxCount;
        }

        [Header("Pool Settings")]
        [SerializeField] private List<ObjectInfo> poolObjectList = new();
        [SerializeField] private List<Transform> rootList = new();

        private Dictionary<Utils.PoolType, Transform> rootDict = new();
        private Dictionary<string, IObjectPool<GameObject>> poolObjectDict = new();
        private Dictionary<string, GameObject> createDict = new();
        private string createObjectName;

        public override void Initialize()
        {
            SetPoolRoot();
            SetPoolObject();
        }

        private void SetPoolRoot()
        {
            if (rootDict.Count > 0) return;

            for (int i = 0; i < rootList.Count; i++)
            {
                rootDict.Add((Utils.PoolType)i, rootList[i]);
            }
        }

        private void SetPoolObject()
        {
            ClearPoolObject();

            for (int i = 0; i < poolObjectList.Count; i++)
            {
                if (Managers.CurrentSceneType != poolObjectList[i].useScene) continue;

                var pool = new ObjectPool<GameObject>
                    (CreatePoolObject, OnTakeFromPool, OnReturnToPool, OnDestroyPoolObject, true,
                    poolObjectList[i].createCount, poolObjectList[i].maxCount);

                if (createDict.ContainsKey(poolObjectList[i].createObjectName))
                {
                    Debug.LogWarning($"{poolObjectList[i].createObjectName} => 이미 등록된 오브젝트입니다.");
                    return;
                }

                createDict.Add(poolObjectList[i].createObjectName, poolObjectList[i].createPrefab);
                poolObjectDict.Add(poolObjectList[i].createObjectName, pool);

                CreatePoolObject(poolObjectList[i]);
            }
        }

        private void ClearPoolObject()
        {
            if (poolObjectDict.Count <= 0) return;

            foreach (var pool in poolObjectDict.Values)
            {
                pool.Clear();
            }

            createDict.Clear();
            poolObjectDict.Clear();
        }

        private void CreatePoolObject(ObjectInfo objectInfo)
        {
            for (int j = 0; j < objectInfo.createCount; j++)
            {
                createObjectName = objectInfo.createObjectName;
                var create = CreatePoolObject().GetComponent<PoolAble>();
                create.transform.SetParent(rootDict[objectInfo.poolType]);
                create.Pool.Release(create.gameObject);
            }
        }

        private GameObject CreatePoolObject()
        {
            var createObj = UnityEngine.Object.Instantiate(createDict[createObjectName]);
            createObj.name = createObjectName;
            createObj.GetComponent<PoolAble>().Pool = poolObjectDict[createObjectName];
            return createObj;
        }

        private void OnTakeFromPool(GameObject poolObject)
        {
            poolObject.SetActive(true);
        }

        private void OnReturnToPool(GameObject poolObject)
        {
            poolObject.SetActive(false);

            var target = poolObjectList.Find(targetInfo => targetInfo.createPrefab.name == poolObject.name);
            if (target == null) return;

            poolObject.transform.SetParent(rootDict[target.poolType]);
        }

        private void OnDestroyPoolObject(GameObject poolObject)
        {
            UnityEngine.Object.Destroy(poolObject);
        }

        public GameObject GetPoolObject(string objectName)
        {
            createObjectName = objectName;

            if(!createDict.ContainsKey(createObjectName))
            {
                Debug.LogWarning($"{createObjectName} => 오브젝트 풀에 등록되지 않은 오브젝트입니다.");
                return null;
            }

            return poolObjectDict[createObjectName].Get();
        }
    }
}