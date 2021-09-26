using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject shop;
    
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
