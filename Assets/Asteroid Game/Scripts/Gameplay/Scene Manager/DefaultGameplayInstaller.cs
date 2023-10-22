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

    private void Awake()
    {
        var playerPool = gameObject.AddComponent<ObjectPool>();
        playerPool.settings = playerPoolSettings;

        var asteroidPool = gameObject.AddComponent<EnemyPool>();
        asteroidPool.settings = asteroidPoolSettings;
        asteroidPool.enemySettings = asteroidSettings;

        var shipPool = gameObject.AddComponent<ObjectPool>();
        shipPool.settings = shipEnemyPoolSettings;

        //

        playerPool.GetObjectFromPool();
        asteroidPool.InstantiateEnemies(startAsteroids.Value);
    }
}