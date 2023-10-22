using System;
using UnityEngine;
using System.Collections.Generic;

//Pool of game objects to efficiently reuse them instead of creating and destroying them frequently
public class ObjectPool : MonoBehaviour
{
    [Serializable]
    public class Settings
    {
        public IntReference initialPoolSize;
        public GameObject prefab;
    }

    [Serializable]
    public class View
    {
        public GameObjectListReference objectPool;
    }

    public Settings settings;
    public View view;

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        view.objectPool.Set(new List<GameObject>());

        for (int i = 0; i < settings.initialPoolSize; i++)
        {
            GameObject obj = Instantiate(settings.prefab);
            obj.AddComponent<Disposable>().onDestroy += RemoveObjectFromPool;
            obj.SetActive(false);
            view.objectPool.Add(obj);
        }
    }

    public GameObject GetObjectFromPool()
    {
        foreach (GameObject obj in view.objectPool.Value)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // If all objects are in use, create a new one
        GameObject newObj = Instantiate(settings.prefab);
        view.objectPool.Add(newObj);
        return newObj;
    }

    public void RemoveObjectFromPool(GameObject obj)
    {
        if (!view.objectPool.Value.Contains(obj))
        {
            view.objectPool.Value.Remove(obj);
        }
    }
}