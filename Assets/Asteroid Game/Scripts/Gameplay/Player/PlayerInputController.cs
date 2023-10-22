// PlayerInputController: Handles player input for movement in different directions.
using System;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [Serializable]
    public class Settings
    {
        public KeyCodeVariable moveLeft, moveRight, moveUp, moveDown;
        public Vector3Reference direction;
    }

    public Settings settings;
    Vector2 vector3Zero = Vector2.zero;

    public void Update()
    {
        settings.direction.Set(vector3Zero);
        if (Input.GetKey(settings.moveLeft.value))
        {
            settings.direction.SetX(settings.direction.Value.x - 1);
        }
        if (Input.GetKey(settings.moveRight.value))
        {
            settings.direction.SetX(settings.direction.Value.x + 1);
        }
        if (Input.GetKey(settings.moveUp.value))
        {
            settings.direction.SetY(settings.direction.Value.y + 1);
        }
        if (Input.GetKey(settings.moveDown.value))
        {
            settings.direction.SetY(settings.direction.Value.y - 1);
        }
    }
}