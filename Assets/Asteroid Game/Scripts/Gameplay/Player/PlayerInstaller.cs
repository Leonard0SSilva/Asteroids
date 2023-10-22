using UnityEngine;

public class PlayerInstaller : MonoBehaviour
{
    public Vector3Reference direction;
    public PlayerInputController.Settings inputSettings;
    public MovementController.Settings movetSettings;

    private void Awake()
    {
        inputSettings.direction = direction;
        GetComponent<PlayerInputController>().settings = inputSettings;
        movetSettings.direction = direction;
        GetComponent<MovementController>().settings = movetSettings;
    }
}