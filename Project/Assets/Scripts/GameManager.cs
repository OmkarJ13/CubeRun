using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [Header("Game Over Settings")]
    [SerializeField] private float gameOverDelay = 1.0f;
    
    [Header("Dependencies")]
    [SerializeField] private GameObject gameOverWidget;
    [SerializeField] private GameObject hud;
    [SerializeField] private LevelGenerator levelGenerator;
    [SerializeField] private Player player;

    public event Action PlayerDied;

    private void Awake()
    {
        #if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
        #else
        Debug.unityLogger.logEnabled = false;
        #endif
    }

    public void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    public void LoadNextLevel()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextScene == SceneManager.sceneCountInBuildSettings)
            nextScene = 0;

        SceneManager.LoadScene(nextScene);
    }

    public IEnumerator GameOver()
    {
        PlayerDied?.Invoke();
        
        PlayerPrefs.SetInt("HighScore", player.HighScore);
        
        levelGenerator.enabled = false;
        player.enabled = false;
        
        hud.SetActive(false);
        
        Time.timeScale = 0.1f;

        yield return new WaitForSecondsRealtime(gameOverDelay);
        
        Time.timeScale = 1.0f;
        
        gameOverWidget.SetActive(true);
    }
}
