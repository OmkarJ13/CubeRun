using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Global Curve Settings")] 
    [SerializeField] private Vector2 curveStrength = new Vector2(-0.005f, -0.001f);
    
    private readonly int CurveStrength = Shader.PropertyToID("_CurveStrength");

    private void Awake ()
    {
        DisableLog();
    }

    private void DisableLog()
    {
        #if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
        #else
        Debug.unityLogger.logEnabled = false;
        #endif
    }

    private void Start()
    {
        SetGlobalCurve();
    }

    private void SetGlobalCurve()
    {
        Shader.SetGlobalVector(CurveStrength, curveStrength);
    }

    public void ReloadLevel ()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    public void LoadNextLevel ()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextScene == SceneManager.sceneCountInBuildSettings)
            nextScene = 0;

        SceneManager.LoadScene(nextScene);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
