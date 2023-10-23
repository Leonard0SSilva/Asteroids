using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private Transform cam;
    [SerializeField]
    private float shakeDuration = 1.0f, shakeMagnitude = 0.7f, dampingSpeed = 1.0f;

    private Vector3 initialPosition;

    private void Start()
    {
        if (cam == null)
        {
            cam = Camera.main.transform;
        }
        initialPosition = cam.localPosition;
    }

    public void ShakeCamera()
    {
        if (this)
        {
            StartCoroutine(PerformShake());
        }
    }

    private IEnumerator PerformShake()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            Vector3 randomPoint = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            cam.localPosition = Vector3.Lerp(cam.localPosition, randomPoint, Time.deltaTime * dampingSpeed);

            elapsed += Time.deltaTime;

            yield return null;
        }

        cam.localPosition = initialPosition;
    }
}