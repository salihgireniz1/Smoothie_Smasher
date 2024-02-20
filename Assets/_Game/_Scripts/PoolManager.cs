using PAG.Utility;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoSingleton<PoolManager>
{
    [System.Serializable]
    public class Pool
    {
        public GameObject prefab;
        public int poolSize;
    }

    public List<GameObject> poolableObjects = new();
    public int poolSize;
    private Dictionary<string, Queue<GameObject>> objectPools;

    private void Start()
    {
        objectPools = new Dictionary<string, Queue<GameObject>>();

        // Create object pools for each prefab in the pools list
        foreach (GameObject poolObj in poolableObjects)
        {
            GeneratePool(poolObj, poolSize);
        }
    }
    public void GeneratePool(GameObject poolObj, int poolSize)
    {
        string poolName = poolObj.name;
        Queue<GameObject> objectPool = new Queue<GameObject>();

        GameObject poolHolder = new GameObject(poolName + "'s");
        poolHolder.transform.parent = this.transform;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(poolObj, poolHolder.transform);
            obj.SetActive(false);
            obj.name = poolName;
            objectPool.Enqueue(obj);
        }

        objectPools.Add(poolName, objectPool);
    }

    // Retrieve an object from the pool based on its name
    public GameObject SpawnFromPool(string name, Vector3 position, Quaternion rotation)
    {
        if (!objectPools.ContainsKey(name))
        {
            Debug.LogWarning("Pool with name " + name + " doesn't exist!");
            return null;
        }

        GameObject objectToSpawn = objectPools[name].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        objectPools[name].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    // Dequeue an object from the pool if it exists, otherwise, instantiate a new one (if allowed)
    public GameObject DequeueFromPool(string name, Vector3 position, Quaternion rotation, bool instantiateIfEmpty = true)
    {
        if (!objectPools.ContainsKey(name))
        {
            Debug.LogWarning("Pool with name " + name + " doesn't exist!");
            return null;
        }

        Queue<GameObject> objectPool = objectPools[name];

        if (objectPool.Count > 0)
        {
            GameObject objectToDequeue = objectPool.Dequeue();
            objectToDequeue.SetActive(true);
            objectToDequeue.transform.position = position;
            objectToDequeue.transform.rotation = rotation;
            return objectToDequeue;
        }
        else if (instantiateIfEmpty)
        {
            GameObject pool = poolableObjects.Find(p => p.name == name);
            if (pool != null)
            {
                GameObject newObj = Instantiate(pool, position, rotation);
                newObj.name = name;
                return newObj;
            }
            else
            {
                Debug.LogWarning("Prefab not found with name " + name + " or it's set to null!");
                return null;
            }
        }
        else
        {
            return null;
        }
    }

    // Enqueue (return) an object back to its corresponding pool
    public void EnqueueToPool(GameObject obj)
    {
        if (!objectPools.ContainsKey(obj.name))
        {
            Debug.LogWarning("Pool with name " + obj.name + " doesn't exist!");
            return;
        }

        obj.SetActive(false);
        objectPools[obj.name].Enqueue(obj);
    }
}
