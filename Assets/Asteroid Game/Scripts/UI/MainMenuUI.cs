using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public AudioClipSettings music;
    public GameObject anyKeyText;
    public AsyncOperation loadSceneOp;

    private void OnEnable()
    {
        music.Play();
    }

    private async void Awake()
    {
        anyKeyText.SetActive(false);
        await Task.Delay(2000);
        anyKeyText.SetActive(true);
    }

    private void OnDisable()
    {
        music.Stop();
    }

    private void Update()
    {
        if (anyKeyText.activeSelf && Input.anyKey && loadSceneOp == null)
        {
            loadSceneOp = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        }
    }
}