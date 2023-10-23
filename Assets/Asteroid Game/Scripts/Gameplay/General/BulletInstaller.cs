using UnityEngine;

public class BulletInstaller : MonoBehaviour
{
    [SerializeField]
    private MovementController.Settings movementSettings;
    public Vector3Reference shootDirection;

    public void Install()
    {
        var movement = gameObject.AddComponent<MovementController>();
        movementSettings.direction = shootDirection;
        movement.settings = movementSettings;
    }
}