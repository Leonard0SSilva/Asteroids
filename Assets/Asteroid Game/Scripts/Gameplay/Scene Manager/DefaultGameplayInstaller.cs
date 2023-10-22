using UnityEngine;

public class DefaultGameplayInstaller : MonoBehaviour
{
    public IntReference startAsteroids;
    [SerializeField]
    private ObjectPool.Settings playerPoolSettings;
    [SerializeField]
    private ObjectPool.Settings asteroidPoolSettings;
    [SerializeField]
    private EnemyPool.Settings asteroidSettings;
    [SerializeField]
    private ObjectPool.Settings shipEnemyPoolSettings;

    private ObjectPool playerPool, shipPool;
    private EnemyPool asteroidPool;

    private void Awake()
    {
        playerPool = gameObject.AddComponent<ObjectPool>();
        playerPool.InitializePool(playerPoolSettings);
        playerPool.GetObjectFromPool();

        asteroidPool = gameObject.AddComponent<EnemyPool>();
        asteroidPool.enemySettings = asteroidSettings;
        asteroidPool.InitializePool(asteroidPoolSettings);
        asteroidPool.InstantiateEnemies(startAsteroids.Value);

        shipPool = gameObject.AddComponent<ObjectPool>();
        shipPool.InitializePool(shipEnemyPoolSettings);
    }
}