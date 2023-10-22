using UnityEngine;

/// <summary>
/// This script is to create a flickering effect on the SpriteRenderer's alpha channel (transparency) based on random parameters. 
/// </summary>
public class LightFlicker : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private float minIntensity, maxIntensity = 1.0f;
    [SerializeField]
    private float maxFlickerDuration = 2.0f, flickerDuration = 1.0f;

    private float originalIntensity, flickerTime, flickerEndTime;

    private void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        originalIntensity = spriteRenderer.color.a;
        flickerTime = 0.0f;
        flickerEndTime = Time.time + flickerDuration;

        RandomizeFlicker();
    }

    private void Update()
    {
        flickerTime += Time.deltaTime;

        if (Time.time >= flickerEndTime)
        {
            RandomizeFlicker();
        }

        float flickerValue = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PerlinNoise(flickerTime, flickerTime));
        Color flickerColor = spriteRenderer.color;
        flickerColor.a = originalIntensity * flickerValue;
        spriteRenderer.color = flickerColor;
    }

    private void RandomizeFlicker()
    {
        minIntensity = Random.Range(0.1f, 0.3f);
        maxIntensity = Random.Range(0.9f, 1.1f);
        flickerTime = 0.0f;
        flickerEndTime = Time.time + (flickerDuration + Random.Range(0, maxFlickerDuration));
    }
}