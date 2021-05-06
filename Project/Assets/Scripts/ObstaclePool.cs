using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstaclePool : MonoBehaviour
{
    [SerializeField] private List<GameObject> obstaclePrefabs;
    [SerializeField] private int amountPerPrefabs = 1;
    
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
            
        GameObject obstacle = pool.Find(x => x && x.GetComponent<Obstacle>()?.Type == randomType && !x.activeInHierarchy);
        if (!obstacle)
        {
            obstacle = obstaclePrefabs.Find(x => x.GetComponent<Obstacle>()?.Type == randomType);
            obstacle = Instantiate(obstacle);
            obstacle.SetActive(false);
            pool.Add(obstacle);
        }
        
        return obstacle;
    }
}
