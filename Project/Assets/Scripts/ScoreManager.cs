using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] private Transform player;

    private Text scoreText;
    
    // ReSharper disable once MemberCanBePrivate.Global
    public int CurrentScore { get; private set; }

    protected override void Awake()
    {
        scoreText = GetComponent<Text>();
    }

    private void LateUpdate()
    { 
        CurrentScore = Mathf.RoundToInt(player.position.z);
        scoreText.text = CurrentScore.ToString();
    }
}
