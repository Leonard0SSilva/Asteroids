using System;
using UnityEngine;

public class DisableAfterTime : MonoBehaviour
{
    [Serializable]
    public class Settings
    {
        public float disableTime = 2.0f;
    }

    public Settings settings;
    private float timer = 0.0f;
    private bool isDisabled = false;

    private void OnEnable()
    {
        timer = 0.0f;
        isDisabled = false;
    }

    private void Update()
    {
        if (!isDisabled)
        {
            timer += Time.deltaTime;

            if (timer >= settings.disableTime)
            {
                gameObject.SetActive(false);
                isDisabled = true;
            }
        }
    }
}
