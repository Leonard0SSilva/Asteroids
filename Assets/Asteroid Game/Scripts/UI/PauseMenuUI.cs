using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    private AsyncOperation loadMainMenuOp;
    public Button mainMenuButton, resumeButton;
    public GameObject menu;

    private void Start()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            if (loadMainMenuOp == null)
            {
                loadMainMenuOp = SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
            }
        });
        resumeButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            menu.SetActive(false);
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(true);
            Time.timeScale = 0;
        }
    }
}