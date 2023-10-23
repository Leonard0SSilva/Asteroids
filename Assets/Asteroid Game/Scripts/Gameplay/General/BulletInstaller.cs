using UnityEngine;

public class BulletInstaller : MonoBehaviour
{
    [SerializeField]
    private DisableAfterTime.Settings disableBulletSettings;
    [SerializeField]
    private MovementController.Settings movementSettings;
    public Vector3Reference shootDirection;

    public void Install()
    {
        gameObject.AddComponent<DisableAfterTime>().settings = disableBulletSettings;
        gameObject.AddComponent<WrapAroundScreen>();
        gameObject.AddComponent<DisableOnCollision>();
        var movement = gameObject.AddComponent<MovementController>();
        movementSettings.direction = shootDirection;
        movement.settings = movementSettings;
    }
}