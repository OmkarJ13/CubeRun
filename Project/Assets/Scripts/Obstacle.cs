using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public ObstacleType Type => type;

    [SerializeField] private ObstacleType type;
    [SerializeField] private float spawnDistance;
    
    private Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void LateUpdate()
    {
        Vector3 direction = player.position - transform.position;
        
        if (direction.z >= spawnDistance)
        {
            if (LevelManager.Instance && LevelManager.Instance.isActiveAndEnabled)
            {
                gameObject.SetActive(false);
                LevelManager.Instance.SpawnObstacle();
            }
        }
    }
}
