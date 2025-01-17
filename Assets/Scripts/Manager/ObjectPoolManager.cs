using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace MSKim.Manager
{
    public class ObjectPoolManager : MonoBehaviour
    {
        [Serializable]
        private class ObjectInfo
        {
            public string createObjectName;
            public GameObject createPrefab;
            public int createCount;
            public int maxCount;
        }

        public static ObjectPoolManager instance;

        [Header("Pool Settings")]
        [SerializeField] private List<ObjectInfo> poolObjectList = new();

        private Dictionary<string, IObjectPool<GameObject>> poolObjectDict = new();
        private Dictionary<string, GameObject> createDict = new();
        private string createObjectName;

        private void Awake()
        {
            if(instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            Initialize();
        }

        private void Initialize()
        {
            for(int i = 0; i < poolObjectList.Count; i++)
            {
                var pool = new ObjectPool<GameObject>(CreatePoolObject, OnTakeFromPool, OnReturnToPool, OnDestroyPoolObject, true, poolObjectList[i].createCount, poolObjectList[i].maxCount);

                if (createDict.ContainsKey(poolObjectList[i].createObjectName))
                {
                    Debug.LogWarning($"{poolObjectList[i].createObjectName} => 이미 등록된 오브젝트입니다.");
                    return;
                }

                createDict.Add(poolObjectList[i].createObjectName, poolObjectList[i].createPrefab);
                poolObjectDict.Add(poolObjectList[i].createObjectName, pool);
                
                for(int j = 0; j < poolObjectList[i].createCount; j++)
                {
                    createObjectName = poolObjectList[i].createObjectName;
                    var create = CreatePoolObject().GetComponent<PoolAble>();
                    create.Pool.Release(create.gameObject);
                }
            }
        }

        private GameObject CreatePoolObject()
        {
            var createObj = Instantiate(createDict[createObjectName]);
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
        }

        private void OnDestroyPoolObject(GameObject poolObject)
        {
            Destroy(poolObject);
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