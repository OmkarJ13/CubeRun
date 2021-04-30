using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private ObstaclePool obstaclePool;

    [SerializeField] private float spawnDistance;
    [SerializeField] private float minDistance, maxDistance;
    [SerializeField] private float minTransitionDistance, maxTransitionDistance;
    
    [SerializeField] private int obstaclesAtStart;
    [SerializeField] private int minObstacles, maxObstacles;
    
    private int obstacles;

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

        currentPos += Vector3.forward * Random.Range(minDistance, maxDistance);

        if (obstacles >= Random.Range(minObstacles, maxObstacles))
        {
            currentPos += Vector3.forward * Random.Range(minTransitionDistance, maxTransitionDistance);
            obstacles = 0;
        }
        else
        {
            obstacles++;
        }
    }
    
    private void OnApplicationQuit()
    {
        enabled = false;
    }
}
