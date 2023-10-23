using UnityEngine;

public class ShipInstaller : EnemyInstaller
{
    [SerializeField]
    public GameObjectReference player;
    [SerializeField]
    private Vector3Reference shootDirection;
    [SerializeField]
    private BoolReference shoot, canShoot;
    [SerializeField]
    private BulletShooter.Settings bulletShooterSettings;
    [SerializeField]
    private ObjectPool.Settings bulletPoolSettings;

    protected override void Awake()
    {
        base.Awake();

        var bulletPool = gameObject.AddComponent<ObjectPool>();
        bulletPool.onCreateObjects += OnCreateBullet;
        bulletPool.onResetObjects += OnResetBullet;
        bulletPool.InitializePool(bulletPoolSettings);
        bulletShooterSettings.bulletPool = bulletPool;
        bulletShooterSettings.canShoot = canShoot;
        bulletShooterSettings.shoot = shoot;
        gameObject.AddComponent<BulletShooter>().settings = bulletShooterSettings;
        gameObject.AddComponent<LightFlicker>();
    }
    protected override void OnEnable()
    {
        float raondomVelocity = Random.Range(minVelocity, maxVelocity);
        movetSettings.moveSpeed.Set(raondomVelocity);
        movetSettings.maxSpeed.Set(raondomVelocity);
        movetSettings.decelerationSpeed.Set(0);
        direction.Set(new Vector3((minDirection.x == 0 && maxDirection.x == 0) ? 0 : Mathf.Sign(Random.Range(minDirection.x, maxDirection.x))
        , (minDirection.y == 0 && maxDirection.y == 0) ? 0 : Mathf.Sign(Random.Range(minDirection.y, maxDirection.y)), 0));
    }

    private void OnCreateBullet(GameObject gameObject)
    {
        var bulletInstaller = gameObject.GetComponent<BulletInstaller>();
        bulletInstaller.Install();
    }

    private void OnResetBullet(GameObject gameObject)
    {
        var bulletInstaller = gameObject.GetComponent<BulletInstaller>();
        bulletInstaller.shootDirection.Set(shootDirection.Value);
    }

    private void Update()
    {
        if (player.Value.activeSelf)
        {
            shootDirection.Set(player.Value.transform.position - gameObject.transform.position);
        }
    }
}