using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private ObstaclePool obstaclePool;
    
    [SerializeField] private int obstaclesAtStart;
    
    [SerializeField] private float spawnDistance;
    [SerializeField] private float distanceBetweenObstacles;

    private Vector3 currentPos;
    
    private void Start()
    {
        currentPos += Vector3.forward * spawnDistance;
        
        for (int i = 0; i < obstaclesAtStart; i++)
        {
            SpawnObstacle();
        }
    }

    public void SpawnObstacle()
    {
        GameObject obstacle = obstaclePool.GetObstacle();

        obstacle.transform.position = currentPos;
        obstacle.transform.rotation = Quaternion.identity;
        obstacle.SetActive(true);

        currentPos += Vector3.forward * distanceBetweenObstacles;
    }
    
    private void OnApplicationQuit()
    {
        enabled = false;
    }
}
