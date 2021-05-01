using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public ObstacleType Type => type;

    [SerializeField] private ObstacleType type;
    [SerializeField] private Transform player;
    [SerializeField] private float spawnDistance;

    private LevelManager levelManager;

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void LateUpdate()
    {
        Vector3 direction = player.transform.position - transform.position;
        float distance = direction.z;

        if (distance >= spawnDistance)
        {
            if (levelManager && levelManager.isActiveAndEnabled)
            {
                gameObject.SetActive(false);
                levelManager.SpawnObstacle();
            }
        }
    }
}
