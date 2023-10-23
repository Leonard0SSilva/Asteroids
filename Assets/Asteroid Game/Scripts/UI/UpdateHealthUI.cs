using System.Collections.Generic;
using UnityEngine;

public class UpdateHealthUI : MonoBehaviour
{
    public GameObject prefab;
    public List<GameObject> healthUIs = new();
    public IntVariable variable;

    private void OnEnable()
    {
        variable.onValueChange += UpdateUI;
        UpdateUI(variable.value);
    }

    private void OnDisable()
    {
        variable.onValueChange -= UpdateUI;
    }

    public void UpdateUI(int value)
    {
        // If the value is greater than the number of UI elements, instantiate new ones
        while (value > healthUIs.Count)
        {
            GameObject go = Instantiate(prefab, transform);
            go.SetActive(true);
            healthUIs.Add(go);
        }

        // If the value is lower than the number of UI elements, destroy the excess ones
        while (value < healthUIs.Count)
        {
            int index = healthUIs.Count - 1;
            GameObject uiToDestroy = healthUIs[index];
            healthUIs.RemoveAt(index);
            Destroy(uiToDestroy);
        }
    }
}