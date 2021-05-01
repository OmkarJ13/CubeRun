using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public ObstacleType Type => type;

    [SerializeField] private ObstacleType type;
    [SerializeField] private float spawnDistance;

    private LevelManager levelManager;
    private Transform player;

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        Vector3 direction = player.position - transform.position;

        if (direction.z >= spawnDistance)
        {
            if (levelManager && levelManager.isActiveAndEnabled)
            {
                gameObject.SetActive(false);
                levelManager.SpawnObstacle();
            }
        }
    }
}
