using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public ObstacleType Type => type;

    [SerializeField] private ObstacleType type;

    private LevelManager levelManager;

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
        
        if (levelManager&& levelManager.isActiveAndEnabled)
            levelManager.SpawnObstacle();
    }
}
