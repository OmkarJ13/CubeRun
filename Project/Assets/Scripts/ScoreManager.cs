using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Transform player;

    private Text scoreText;

    private void Awake()
    {
        scoreText = GetComponent<Text>();
    }

    private void LateUpdate()
    {
        scoreText.text = Mathf.RoundToInt(player.position.z).ToString();
    }
}
