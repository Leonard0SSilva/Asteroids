using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DefaultGameplayInstaller : MonoBehaviour
{
    public float dificultyRate;
    [SerializeField]
    private ObjectPool.Settings playerPoolSettings;
    [SerializeField]
    private ObjectPool.Settings asteroidPoolSettings;
    [SerializeField]
    private EnemyPool.Settings asteroidSettings;
    [SerializeField]
    private ObjectPool.Settings shipPoolSettings;
    [SerializeField]
    private EnemyPool.Settings shipEnemyPoolSettings;

    private List<EnemyPool> enemyPools = new();

    private void Awake()
    {
        var playerPool = gameObject.AddComponent<ObjectPool>();
        playerPool.InitializePool(playerPoolSettings);
        playerPool.GetObjectFromPool();

        var asteroidPool = gameObject.AddComponent<EnemyPool>();
        enemyPools.Add(asteroidPool);
        asteroidPool.enemySettings = asteroidSettings;
        asteroidPool.onDisableObjects += OnDisableEnemy;
        asteroidPool.InitializePool(asteroidPoolSettings);
        asteroidPool.InstantiateEnemies((int)asteroidPool.enemySettings.poolSize);

        var shipPool = gameObject.AddComponent<EnemyPool>();
        enemyPools.Add(shipPool);
        shipPool.onDisableObjects += OnDisableEnemy;
        shipPool.InitializePool(shipPoolSettings);
    }

    private async void OnDisableEnemy(GameObject enemy)
    {
        //Check for next wave of enemies
        if (enemyPools.TrueForAll(x => x.view.objectPool.TrueForAll(x => !x.activeSelf)))
        {
            await Task.Delay(2000);
            foreach (var item in enemyPools)
            {
                item.enemySettings.poolSize = (int)(item.enemySettings.poolSize * 1.1f);
                item.InstantiateEnemies((int)item.enemySettings.poolSize);
            }
        }
    }
}