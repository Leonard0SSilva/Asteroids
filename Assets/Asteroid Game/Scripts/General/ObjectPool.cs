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
        public List<GameObject> objectPool = new();
        public Transform poolParent;
    }

    public Settings settings;
    public View view = new();

    public Action<GameObject> onCreateObjects, onResetObjects, onDisableObjects;

    public void InitializePool(Settings settings)
    {
        this.settings = settings;
#if UNITY_EDITOR
        view.poolParent = new GameObject($"Pool: {settings.prefab.name}").transform;
        view.poolParent.SetParent(gameObject.transform);
#endif

        view.objectPool = new List<GameObject>();
        for (int i = 0; i < settings.initialPoolSize; i++)
        {
            GameObject obj = Instantiate(settings.prefab);
            obj.SetActive(false);
            InitializeGameObject(obj);
        }
    }

    public virtual void ResetGameObject(GameObject obj)
    {
        onResetObjects?.Invoke(obj);
    }

    public virtual void InitializeGameObject(GameObject obj)
    {
#if UNITY_EDITOR
        obj.transform.SetParent(view.poolParent, true);
#endif
        var disposable = obj.AddComponent<Disposable>();
        disposable.onDisable += OnDisableObject;
        disposable.onDestroy += OnDestroyObject;
        onCreateObjects?.Invoke(obj);
        ResetGameObject(obj);
        view.objectPool.Add(obj);
    }

    public GameObject GetObjectFromPool()
    {
        foreach (GameObject obj in view.objectPool)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                ResetGameObject(obj);
                return obj;
            }
        }

        // If all objects are in use, create a new one
        GameObject newObj = Instantiate(settings.prefab);
        newObj.SetActive(true);
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

    public void OnDisableObject(GameObject obj)
    {
        if (obj)
        {
            onDisableObjects?.Invoke(obj);
        }
    }

    public void OnDestroyObject(GameObject obj)
    {
        if (view.objectPool.Contains(obj))
        {
            view.objectPool.Remove(obj);
        }
    }
}