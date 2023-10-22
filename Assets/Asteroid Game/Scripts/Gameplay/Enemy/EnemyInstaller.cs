using UnityEngine;

public class EnemyInstaller : MonoBehaviour
{
    public int health;
    public Vector3Reference direction;
    public MovementController.Settings movetSettings;

    private void Awake()
    {
        direction.Set(new Vector3(Mathf.Sign(Random.Range(-1, 1)), Mathf.Sign(Random.Range(-1, 1)), 0));
        movetSettings.direction = direction;
        gameObject.AddComponent<MovementController>().settings = movetSettings;
        gameObject.AddComponent<WrapAroundScreen>();
    }
}