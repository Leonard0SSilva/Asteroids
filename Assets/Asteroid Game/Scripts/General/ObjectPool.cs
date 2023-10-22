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
        public List<GameObject> objectPool = new List<GameObject>();
        public Transform poolParent;
    }

    public Settings settings;
    public View view = new();

    public void InitializePool(Settings settings)
    {
        this.settings = settings;
#if UNITY_EDITOR
        view.poolParent = new GameObject("Object Pool").transform;
        view.poolParent.SetParent(gameObject.transform);
#endif

        view.objectPool = new List<GameObject>();
        for (int i = 0; i < settings.initialPoolSize; i++)
        {
            GameObject obj = Instantiate(settings.prefab);
#if UNITY_EDITOR
            obj.transform.SetParent(view.poolParent, true);
#endif
            obj.SetActive(false);
            obj.AddComponent<Disposable>().onDestroy += RemoveObjectFromPool;
            view.objectPool.Add(obj);
        }
    }

    public virtual void InitializeGameObject(GameObject obj)
    {

    }

    public GameObject GetObjectFromPool()
    {
        foreach (GameObject obj in view.objectPool)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                InitializeGameObject(obj);
                return obj;
            }
        }

        // If all objects are in use, create a new one
        GameObject newObj = Instantiate(settings.prefab);
#if UNITY_EDITOR
        newObj.transform.SetParent(view.poolParent, true);
#endif
        view.objectPool.Add(newObj);
        InitializeGameObject(newObj);
        return newObj;
    }

    public List<GameObject> GetObjectsFromPool(int size)
    {
        var gameObjects = new List<GameObject>();
        for (int i = 0; i < size; i++)
        {
            gameObjects.Add(GetObjectFromPool());
        }
        return gameObjects;
    }

    public void RemoveObjectFromPool(GameObject obj)
    {
        if (!view.objectPool.Contains(obj))
        {
            view.objectPool.Remove(obj);
        }
    }
}