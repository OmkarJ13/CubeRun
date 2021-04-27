using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstaclePool : MonoBehaviour
{
    [SerializeField] List<GameObject> obstaclePrefabs;
    [SerializeField] int amountPerPrefabs;
    
    private readonly List<GameObject> pool = new List<GameObject>();

    private void Start()
    {
        foreach (GameObject obstaclePrefab in obstaclePrefabs)
        {
            for (int i = 0; i < amountPerPrefabs; i++)
            {
                GameObject obstacle = Instantiate(obstaclePrefab);
                obstacle.SetActive(false);
                pool.Add(obstacle);
            }
        }
    }

    public GameObject GetObstacle()
    {
        Array types = Enum.GetValues(typeof(ObstacleType));
        ObstacleType randomType = (ObstacleType) Random.Range(0, types.Length);
            
        GameObject obstacle = pool.Find(x => x != null && x.GetComponentInChildren<Obstacle>()?.Type == randomType && !x.activeInHierarchy);
        if (!obstacle)
        {
            obstacle = obstaclePrefabs.Find(x => x.GetComponentInChildren<Obstacle>()?.Type == randomType);
            obstacle = Instantiate(obstacle);
            obstacle.SetActive(false);
            pool.Add(obstacle);
        }
        
        return obstacle;
    }
}
