using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Text Fields")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI achievementText;
    
    [Header("Dependencies")]
    [SerializeField] private Player player;

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

    private void OnEnable()
    {
        player.ScoreUpdated += OnScoreUpdated;
        player.HighScoreUnlocked += OnNewHighScore;
        
        highScoreText.text = player.HighScore.ToString();
    }

    private void OnDisable()
    {
        player.ScoreUpdated -= OnScoreUpdated;
        player.HighScoreUnlocked -= OnNewHighScore;
    }
}
