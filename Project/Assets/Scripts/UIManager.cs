using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Text Fields")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI achievementText;
    [SerializeField] private TextMeshProUGUI coinText;
    
    [Header("Dependencies")]
    [SerializeField] private Player player;
    [SerializeField] private GameObject hud;

    private void OnScoreUpdated()
    {
        scoreText.text = player.CurrentScore.ToString();
    }

    private void OnNewHighScore()
    {
        highScoreText.gameObject.SetActive(false);
        achievementText.gameObject.SetActive(true);

        player.HighScoreUnlocked -= OnNewHighScore;
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
        hud.SetActive(false);
    }

    public void EnableHUD()
    {
        hud.SetActive(true);
    }
}
