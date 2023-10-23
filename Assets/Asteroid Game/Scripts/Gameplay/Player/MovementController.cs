using System;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Serializable]
    public class Settings
    {
        public FloatReference moveSpeed, maxSpeed, decelerationSpeed;
        public Vector3Reference direction;
        public Vector3Reference velocity;
    }

    public Settings settings;

    private void OnEnable()
    {
        if (settings != null)
        {
            settings.velocity.Set(Vector3.zero);
        }
    }

    private void FixedUpdate()
    {
        if (settings.direction.Value.magnitude == 0)
        {
            // Decelerate based on the current velocity
            settings.velocity.Set(Vector3.Lerp(settings.velocity.Value, Vector3.zero, settings.decelerationSpeed.Value * Time.fixedDeltaTime));
        }
        else
        {
            // Calculate the new velocity based on direction and speed
            // Ensure the velocity does not exceed the max speed
            settings.velocity.Set(Vector3.ClampMagnitude(Vector3.Lerp(settings.velocity.Value, settings.direction.Value * settings.moveSpeed.Value, settings.moveSpeed.Value * Time.fixedDeltaTime), settings.maxSpeed.Value));
        }

        // Move the object based on its velocity
        transform.position += settings.velocity.Value * Time.fixedDeltaTime;
    }
}