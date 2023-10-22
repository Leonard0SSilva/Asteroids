using System;
using UnityEngine;

public class Disposable : MonoBehaviour
{
    public Action<GameObject> onDisable;
    public Action<GameObject> onDestroy;

    private void OnDisable()
    {
        onDisable?.Invoke(gameObject);
    }

    private void OnDestroy()
    {
        onDestroy?.Invoke(gameObject);
    }
}