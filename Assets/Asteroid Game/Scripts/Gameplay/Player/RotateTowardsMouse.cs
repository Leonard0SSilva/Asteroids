using System;
using UnityEngine;

public class RotateTowardsMouse : MonoBehaviour
{
    [Serializable]
    public class Settings
    {
        public FloatReference rotationSpeed;
    }

    public Settings settings;

    private void FixedUpdate()
    {
        Vector3 mousePosition = Input.mousePosition;

        // Convert the mouse position from screen space to world space
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, transform.position.z));

        // Calculate the direction from the object's current position to the mouse position
        Vector3 directionToMouse = mousePosition - transform.position;

        // Calculate the rotation angle based on the direction
        float angleToMouse = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;

        // Smoothly interpolate between the current rotation and the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angleToMouse), settings.rotationSpeed.Value * Time.fixedDeltaTime);
    }
}