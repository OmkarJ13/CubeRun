using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstaclePool : MonoBehaviour
{
    [SerializeField] private List<ObstacleTile> obstacleTilePrefabs;
    [SerializeField] private int amountPerPrefab = 1;
    
    private readonly List<ObstacleTile> _pool = new List<ObstacleTile>();

    private void Awake()
    {
        InitPool();
    }

    private void InitPool()
    {
        foreach (ObstacleTile obstacleTilePrefab in obstacleTilePrefabs)
        {
            for (int i = 0; i < amountPerPrefab; i++)
            {
                ObstacleTile pooledObstacle = Instantiate(obstacleTilePrefab);
                pooledObstacle.gameObject.SetActive(false);
                _pool.Add(pooledObstacle);   
            }
        }   
    }
    
    public ObstacleTile GetObstacle()
    {
        int randomIndex = Random.Range(0, _pool.Count);
        ObstacleTile obstacle = !_pool[randomIndex].gameObject.activeSelf ? _pool[randomIndex] : GetObstacle();
        return obstacle;
    }
}
