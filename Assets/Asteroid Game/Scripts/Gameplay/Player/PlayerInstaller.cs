using UnityEngine;

public class PlayerInstaller : MonoBehaviour
{
    public Vector3Reference direction;
    public PlayerInputController.Settings inputSettings;
    public MovementController.Settings movetSettings;
    public RotateTowardsMouse.Settings rotateSettings;

    private void Awake()
    {
        inputSettings.direction = direction;
        gameObject.AddComponent<PlayerInputController>().settings = inputSettings;
        movetSettings.direction = direction;
        gameObject.AddComponent<MovementController>().settings = movetSettings;
        gameObject.AddComponent<RotateTowardsMouse>().settings = rotateSettings;
        gameObject.AddComponent<WrapAroundScreen>();
    }
}