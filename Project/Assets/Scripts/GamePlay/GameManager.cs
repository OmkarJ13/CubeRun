using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header ("Game Over Settings")]
    [SerializeField] private float gameOverDelay = 1.0f;

    [Header("Dependencies")] 
    [SerializeField] private GameObject shatteredPlayerPrefab;
    [SerializeField] private GameObject tutorialLevel;
    [SerializeField] private GameObject gameOverWidget;
    
    private UIManager uiManager;
    private LevelGenerator levelGenerator;
    private FollowCamera followCamera;
    private Player player;
    private SwipeManager swipeManager;

    private GameObject _shatteredPlayer;
    private static readonly int CurveStrength = Shader.PropertyToID("_CurveStrength");

    private void Awake ()
    {
        DisableLog();
        InitDependencies();
    }

    private void InitDependencies()
    {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        levelGenerator = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();
        followCamera = GameObject.FindGameObjectWithTag("CameraHolder").GetComponent<FollowCamera>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        swipeManager = GameObject.FindGameObjectWithTag("SwipeManager").GetComponent<SwipeManager>();
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
        TutorialCheck();
    }

    private void TutorialCheck()
    {
        if (!PlayerPrefs.HasKey("firstRun"))
        {
            PlayerPrefs.SetInt("firstRun", 1);
            swipeManager.disableAllControls = true;

            Instantiate(tutorialLevel, Vector3.forward * 100.0f, Quaternion.identity);
        }
        else
        {
            StartGame();
        }
    }

    private void SetGlobalCurve()
    {
        Shader.SetGlobalVector(CurveStrength, new Vector4(-0.005f, -0.001f));
    }

    public void StartGame()
    {
        levelGenerator.SetupObstacles(Vector3.forward * (player.transform.position.z + 50.0f));
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

    public IEnumerator GameOver()
    {
        PlayerPrefs.SetInt("HighScore", player.HighScore);

        int currentCoins = PlayerPrefs.GetInt("Coins", 0);
        PlayerPrefs.SetInt("Coins", player.CollectedCoins + currentCoins);

        ReplacePlayerMesh();
        
        levelGenerator.enabled = false;
        uiManager.DisableHUD();

        yield return new WaitForSeconds(gameOverDelay);

        gameOverWidget.SetActive(true);
    }

    private void ReplacePlayerMesh()
    {
        Transform playerTransform = player.transform;
        _shatteredPlayer = Instantiate(shatteredPlayerPrefab, playerTransform.position, playerTransform.rotation);
        
        _shatteredPlayer.transform.localScale = playerTransform.localScale;
        
        followCamera.followTargets = new List<Transform>();
        
        for (int i = 0; i < _shatteredPlayer.transform.childCount; i++)
        {
            followCamera.followTargets.Add(_shatteredPlayer.transform.GetChild(0));
        }
    }

    public void RevivePlayer()
    {
        DestroySurroundings();
        
        Destroy(_shatteredPlayer);
        player.gameObject.SetActive(true);

        followCamera.followTargets = new List<Transform> { player.transform };
        player.isDead = false;

        levelGenerator.enabled = true;
        uiManager.EnableHUD();
    }

    private void DestroySurroundings()
    {
        Collider[] results = Physics.OverlapBox(player.transform.position, new Vector3(100, 100, 100), Quaternion.identity, LayerMask.GetMask("Obstacles"));
        if (results.Length > 0)
        {
            foreach (Collider result in results)
            {
                result.gameObject.SetActive(false);
            }
        }
    }
}
