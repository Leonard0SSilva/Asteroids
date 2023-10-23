using UnityEngine;

public class EnemyInstaller : MonoBehaviour
{
    public int health;
    public Vector3Reference direction;
    public MovementController.Settings movetSettings;
    public Vector2 minDirection, maxDirection;
    public float minVelocity, maxVelocity;

    protected virtual void Awake()
    {
        movetSettings.direction = direction;
        gameObject.AddComponent<MovementController>().settings = movetSettings;
        gameObject.AddComponent<WrapAroundScreen>();
        gameObject.AddComponent<DisableOnCollision>();
    }

    protected virtual void OnEnable()
    {
        float raondomVelocity = Random.Range(minVelocity, maxVelocity);
        movetSettings.moveSpeed.Set(raondomVelocity);
        movetSettings.maxSpeed.Set(raondomVelocity);
        movetSettings.decelerationSpeed.Set(0);
        direction.Set(new Vector3(Mathf.Sign(Random.Range(minDirection.x, maxDirection.x)), Mathf.Sign(Random.Range(minDirection.y, maxDirection.y)), 0));
    }
}