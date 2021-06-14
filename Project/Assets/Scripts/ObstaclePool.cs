using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstaclePool : MonoBehaviour
{
    [SerializeField] private List<ObstacleTile> obstacleTilePrefabs;
    private readonly List<ObstacleTile> _pool = new List<ObstacleTile>();

    public ObstacleTile GetObstacle()
    {
        Array types = Enum.GetValues(typeof(ObstacleType));
        ObstacleType randomType = (ObstacleType) Random.Range(0, types.Length);
            
        ObstacleTile obstacle = _pool.Find(x => x && x.Type == randomType && !x.gameObject.activeSelf);
        if (!obstacle)
        {
            obstacle = obstacleTilePrefabs.Find(x => x.Type == randomType);
            obstacle = Instantiate(obstacle);
            obstacle.gameObject.SetActive(false);
            _pool.Add(obstacle);
        }

        return obstacle;
    }
}
