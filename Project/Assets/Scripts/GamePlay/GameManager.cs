using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Tutorial")] 
    [SerializeField] private GameObject tutorialLevel;
    
    [Header("Global Curve Settings")] 
    [SerializeField] private Vector2 curveStrength = new Vector2(-0.005f, -0.001f);

    private readonly int CurveStrength = Shader.PropertyToID("_CurveStrength");
    private LevelGenerator levelGenerator;

    private void Awake ()
    {
        Application.targetFrameRate = 120;
        levelGenerator = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();
        
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

    private void CheckTutorial()
    {
        if (PlayerPrefs.GetInt("firstRun", 1) == 1)
        {
            tutorialLevel.SetActive(true);
            PlayerPrefs.SetInt("firstRun", 0);
        }
        else
        {
            if (tutorialLevel.activeSelf)
                tutorialLevel.SetActive(false);
            
            levelGenerator.SetupObstacles(Vector3.zero);
        }
    }

    private void Start()
    {
        SetGlobalCurve();
        CheckTutorial();
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
