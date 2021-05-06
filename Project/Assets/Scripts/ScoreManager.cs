using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] private float updateRate;
    
    private Text scoreText;

    // ReSharper disable once MemberCanBePrivate.Global
    public float CurrentScore { get; private set; }

    protected override void Awake()
    {
        scoreText = GetComponent<Text>();
    }

    private void FixedUpdate()
    {
        CurrentScore += updateRate;
        scoreText.text = Mathf.RoundToInt(CurrentScore).ToString(); 
    }
}
