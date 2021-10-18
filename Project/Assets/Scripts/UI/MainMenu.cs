using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image loadingProgress;

    public void Play()
    {
        StartCoroutine(LoadGame());
    }

    private IEnumerator LoadGame()
    {
        loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        while (!operation.isDone)
        {
            loadingProgress.fillAmount = operation.progress;
            yield return null;
        }

        loadingScreen.SetActive(false);
    }

    public void Shop()
    {
        gameObject.SetActive(false);
        shop.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
