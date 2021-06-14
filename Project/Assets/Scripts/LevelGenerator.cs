using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [Header("Object Pools")]
    [SerializeField] private ObstaclePool obstaclePool;
    [SerializeField] private TransitionPool transitionPool;

    [Header("Obstacle")]
    [SerializeField] private int minObstacles = 5;
    [SerializeField] private int maxObstacles = 10;
    
    [Header("Transition")]
    [SerializeField] [Range(0, 6)] private int transitionsAtStart = 4;
    [SerializeField] private int minTransitions = 2;
    [SerializeField] private int maxTransitions = 4;

    private Vector3 _currentPos;
    private int _continuousObstacles;

    private void Start()
    {
        SetupTransitions();
    }

    private void SetupTransitions()
    {
        for (int i = 0; i < transitionsAtStart; i++)
        {
            SpawnTransition();
        }
    }

    public void SpawnSegment()
    {
        ObstacleTile obstacle = obstaclePool.GetObstacle();
        
        GameObject obj = obstacle.gameObject;

        obj.transform.position = _currentPos;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);
        
        _currentPos += Vector3.forward * obstacle.Length;
        
        _continuousObstacles++;

        if (_continuousObstacles >= Random.Range(minObstacles, maxObstacles))
        {
            _continuousObstacles = 0;
            
            int transitionsToSpawn = Random.Range(minTransitions, maxTransitions);
            for (int i = 0; i < transitionsToSpawn; i++)
            {
                SpawnTransition();
            }
        }
    }

    private void SpawnTransition()
    {
        TransitionTile transition = transitionPool.GetTransition();

        GameObject obj = transition.gameObject;

        obj.transform.position = _currentPos;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);
        
        _currentPos += Vector3.forward * transition.Length;
    }
}
