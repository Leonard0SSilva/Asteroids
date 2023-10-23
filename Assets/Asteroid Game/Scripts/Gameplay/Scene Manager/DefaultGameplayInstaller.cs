using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DefaultGameplayInstaller : MonoBehaviour
{
    [SerializeField]
    private IntReference currentLevel;
    [SerializeField]
    private float dificultyRate;
    [SerializeField]
    private Vector3 onDestroyAsteroidOffset;
    [SerializeField]
    private EnemyPool.Settings asteroidSettings, shipEnemyPoolSettings;
    [SerializeField]
    private ObjectPool.Settings playerPoolSettings, asteroidPoolSettings
    , shipPoolSettings, mediumAsteroidSettings, smallAsteroidSettings;
    private ObjectPool mediumAsteroidPool, smallAsteroidPool;
    [SerializeField]
    private List<ObjectPool> enemyObjectPools = new();
    [SerializeField]
    private List<EnemyPool> enemyPools = new();

    private void Awake()
    {
        //Player
        var playerPool = gameObject.AddComponent<ObjectPool>();
        playerPool.InitializePool(playerPoolSettings);
        playerPool.GetObjectFromPool();

        //Default Asteroid
        var asteroidPool = gameObject.AddComponent<EnemyPool>();
        enemyPools.Add(asteroidPool);
        enemyObjectPools.Add(asteroidPool);
        asteroidPool.enemySettings = asteroidSettings;
        asteroidPool.onCreateObjects += OnCreateAsteroid;
        asteroidPool.onDisableObjects += OnDisableAsteroid;
        asteroidPool.onDisableObjects += OnDisableEnemy;
        asteroidPool.InitializePool(asteroidPoolSettings);
        asteroidPool.InstantiateEnemies((int)asteroidPool.enemySettings.poolSize);

        //Medium Asteroids
        mediumAsteroidPool = gameObject.AddComponent<ObjectPool>();
        enemyObjectPools.Add(mediumAsteroidPool);
        mediumAsteroidPool.onCreateObjects += OnCreateAsteroid;
        mediumAsteroidPool.onDisableObjects += OnDisableEnemy;
        mediumAsteroidPool.onDisableObjects += OnDisableMediumAsteroid;
        mediumAsteroidPool.InitializePool(mediumAsteroidSettings);

        //Small Asteroids
        smallAsteroidPool = gameObject.AddComponent<ObjectPool>();
        enemyObjectPools.Add(smallAsteroidPool);
        smallAsteroidPool.onDisableObjects += OnDisableEnemy;
        smallAsteroidPool.onCreateObjects += OnCreateAsteroid;
        smallAsteroidPool.InitializePool(smallAsteroidSettings);

        //Ship
        var shipPool = gameObject.AddComponent<EnemyPool>();
        enemyPools.Add(shipPool);
        enemyObjectPools.Add(shipPool);
        shipPool.enemySettings = shipEnemyPoolSettings;
        shipPool.onDisableObjects += OnDisableEnemy;
        shipPool.InitializePool(shipPoolSettings);
    }

    private void OnCreateAsteroid(GameObject enemy)
    {
        enemy.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Random.Range(0, 360));
    }

    private void OnDisableAsteroid(GameObject enemy)
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject go = mediumAsteroidPool.GetObjectFromPool();
            go.transform.position = GetRandomPosition(enemy);
        }
    }

    private void OnDisableMediumAsteroid(GameObject enemy)
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject go = smallAsteroidPool.GetObjectFromPool();
            go.transform.position = GetRandomPosition(enemy);
        }
    }

    private Vector3 GetRandomPosition(GameObject go)
    {
        // Generate random offsets within the specified range
        float randomX = Random.Range(-onDestroyAsteroidOffset.x, onDestroyAsteroidOffset.x);
        float randomY = Random.Range(-onDestroyAsteroidOffset.y, onDestroyAsteroidOffset.y);
        float randomZ = Random.Range(-onDestroyAsteroidOffset.z, onDestroyAsteroidOffset.z);
        return go.transform.position + new Vector3(randomX, randomY, randomZ);
    }

    private async void OnDisableEnemy(GameObject enemy)
    {
        //Check for next wave of enemies
        if (enemyObjectPools.TrueForAll(x => x.view.objectPool.TrueForAll(x => !x.activeSelf)))
        {
            await Task.Delay(2000);
            currentLevel.Set(currentLevel.Value + 1);
            foreach (var item in enemyPools)
            {
                item.enemySettings.poolSize *= dificultyRate;
                item.InstantiateEnemies(Mathf.RoundToInt(item.enemySettings.poolSize));
            }
        }
    }
}