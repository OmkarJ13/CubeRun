using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Tutorial")] 
    [SerializeField] private GameObject tutorialLevel;
    
    [Header("Global Curve Settings")] 
    [SerializeField] private Vector2 curveStrength = new Vector2(-0.004f, -0.001f);

    private readonly int CurveStrength = Shader.PropertyToID("_CurveStrength");
    
    private LevelGenerator levelGenerator;
    private SwipeManager swipeManager;

    private void Awake ()
    {
        Application.targetFrameRate = 60;
        
        levelGenerator = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();
        swipeManager = GameObject.FindGameObjectWithTag("SwipeManager").GetComponent<SwipeManager>();
    }
    

    private void CheckTutorial()
    {
        if (PlayerPrefs.GetInt("firstRun", 1) == 1)
        {
            tutorialLevel.SetActive(true);
            swipeManager.disableAllControls = true;
            
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

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
