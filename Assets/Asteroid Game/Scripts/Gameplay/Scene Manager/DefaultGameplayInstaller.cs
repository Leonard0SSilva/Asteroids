using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefaultGameplayInstaller : MonoBehaviour
{
    [SerializeField]
    private GameObjectReference player;
    [SerializeField]
    private IntReference currentLevel, currentScore, playerHealth;
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
    public AudioClipSettings destroyAsteroidClip, playerDeathClip, gameOverClip, startClip, reviveClip, nextLevelClip;

    private void Awake()
    {
        startClip.Play();
        currentLevel.Set(1);
        currentScore.Set(0);
        playerHealth.Set(3);

        //Player
        var playerPool = gameObject.AddComponent<ObjectPool>();
        playerPool.onDisableObjects += OnDisablePlayer;
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

    private void OnCreateAsteroid(GameObject go)
    {
        go.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Random.Range(0, 360));
    }

    private void OnDisableAsteroid(GameObject go)
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject newGO = mediumAsteroidPool.GetObjectFromPool();
            newGO.transform.position = GetRandomPosition(go);
        }
    }

    private void OnDisableMediumAsteroid(GameObject go)
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject newGO = smallAsteroidPool.GetObjectFromPool();
            newGO.transform.position = GetRandomPosition(go);
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

    private async void OnDisableEnemy(GameObject go)
    {
        destroyAsteroidClip.Play();
        currentScore.Set(currentScore + 10);
        //Check for next wave of enemies
        if (enemyObjectPools.TrueForAll(x => x.view.objectPool.TrueForAll(x => !x.activeSelf)))
        {
            nextLevelClip.Play();
            await Task.Delay(2000);
            MakePlayerImunte(player.Value);
            currentLevel.Set(currentLevel.Value + 1);
            foreach (var item in enemyPools)
            {
                item.enemySettings.poolSize *= dificultyRate;
                item.InstantiateEnemies(Mathf.RoundToInt(item.enemySettings.poolSize));
            }
        }
    }

    private async void OnDisablePlayer(GameObject go)
    {
        playerHealth.Set(playerHealth - 1);
        //GameOver
        if (playerHealth <= 0)
        {
            gameOverClip.Play();
            await Task.Delay(2000);
            SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        }
        //Revive
        else
        {
            playerDeathClip.Play();
            await Task.Delay(2000);
            reviveClip.Play();
            go.SetActive(true);
            MakePlayerImunte(go);
        }
    }

    private async void MakePlayerImunte(GameObject go)
    {
        var collider2D = go.GetComponent<Collider2D>();
        var lightFlicker = go.AddComponent<LightFlicker>();
        collider2D.enabled = false;
        await Task.Delay(2000);
        Destroy(lightFlicker);
        go.GetComponent<SpriteRenderer>().color = Color.white;
        collider2D.enabled = true;
    }
}