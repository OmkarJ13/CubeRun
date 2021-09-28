using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI coinText;

    [Header("Dependencies")]
    [SerializeField] private GameObject hudWidget;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject achievement;

    // Dependencies
    private Player player;

    private bool achievementShown;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnScoreUpdated()
    {
        scoreText.text = player.CurrentScore.ToString();
    }

    private void OnNewHighScore()
    {
        if (!achievementShown)
        {
            achievement.SetActive(true);
            achievementShown = true;
        }
        
        highScoreText.text = player.HighScore.ToString();
    }

    private void OnCoinCollected()
    {
        coinText.text = player.CollectedCoins.ToString();
    }

    private void OnEnable()
    {
        player.ScoreUpdated += OnScoreUpdated;
        player.HighScoreUnlocked += OnNewHighScore;
        player.CoinCollected += OnCoinCollected;

        highScoreText.text = player.HighScore.ToString();
        coinText.text = player.CollectedCoins.ToString();
    }

    private void OnDisable()
    {
        player.ScoreUpdated -= OnScoreUpdated;
        player.HighScoreUnlocked -= OnNewHighScore;
    }

    public void DisableHUD()
    {
        hudWidget.GetComponent<CanvasGroup>().alpha = 0.0f;
    }

    public void EnableHUD()
    {
        hudWidget.GetComponent<CanvasGroup>().alpha = 1.0f;
    }

    public void OnPaused()
    {
        if (Time.timeScale != 0.0f)
        {
            DisableHUD();
            pauseMenu.SetActive(true);
        }
    }
}
