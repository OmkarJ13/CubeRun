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
    [SerializeField] private GameObject gameOverWidget;
    [SerializeField] private GameObject hud;
    [SerializeField] private LevelGenerator levelGenerator;
    [SerializeField] private FollowCamera followCamera;
    [SerializeField] private Player player;

    private GameObject _shatteredPlayer;
    private static readonly int CurveStrength = Shader.PropertyToID("_CurveStrength");

    private void Awake ()
    {
        #if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
        #else
        Debug.unityLogger.logEnabled = false;
        #endif
    }

    private void Start()
    {
        Shader.SetGlobalVector(CurveStrength, new Vector4(-0.005f, -0.001f));
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

    public IEnumerator GameOver()
    {
        PlayerPrefs.SetInt("HighScore", player.HighScore);
        ReplacePlayerMesh();
        levelGenerator.enabled = false;
        hud.SetActive(false);

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
}
