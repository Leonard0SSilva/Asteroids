using UnityEngine;

public class PlayerInstaller : MonoBehaviour
{
    [SerializeField]
    private GameObjectReference player;
    [SerializeField]
    private KeyCodeVariable shootKey;
    [SerializeField]
    private BoolReference shoot, canShoot;
    [SerializeField]
    private Vector3Reference direction, shootDirection;
    [SerializeField]
    private PlayerInputController.Settings inputSettings;
    [SerializeField]
    private MovementController.Settings movetSettings;
    [SerializeField]
    private RotateTowardsMouse.Settings rotateSettings;
    [SerializeField]
    private BulletShooter.Settings bulletShooterSettings;
    [SerializeField]
    private ObjectPool.Settings bulletPoolSettings;

    private void Awake()
    {
        player.Set(gameObject);

        inputSettings.direction = direction;
        gameObject.AddComponent<PlayerInputController>().settings = inputSettings;
        movetSettings.direction = direction;
        gameObject.AddComponent<MovementController>().settings = movetSettings;
        gameObject.AddComponent<RotateTowardsMouse>().settings = rotateSettings;
        gameObject.AddComponent<WrapAroundScreen>();

        var bulletPool = gameObject.AddComponent<ObjectPool>();
        bulletPool.onCreateObjects += OnCreateBullet;
        bulletPool.onResetObjects += OnResetBullet;
        bulletPool.InitializePool(bulletPoolSettings);
        bulletShooterSettings.bulletPool = bulletPool;
        bulletShooterSettings.canShoot = canShoot;
        bulletShooterSettings.shoot = shoot;
        gameObject.AddComponent<BulletShooter>().settings = bulletShooterSettings;

        gameObject.AddComponent<DisableOnCollision>();
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
        shoot.Set(Input.GetKey(shootKey.value));
        shootDirection.Set(gameObject.transform.right);
    }
}