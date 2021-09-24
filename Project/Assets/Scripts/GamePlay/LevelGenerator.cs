using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private float buildingsAtStart = 5;
    
    [Header("Object Pools")]
    [SerializeField] private ObstaclePool obstaclePool;
    [SerializeField] private BackgroundBuildingsPool buildingsPool;

    [Header("Obstacle")]
    [SerializeField] private int minObstacles = 5;
    [SerializeField] private int maxObstacles = 10;
    [SerializeField] private int obstaclesAtStart = 5;
    [SerializeField] private float distBetweenObstacles = 50.0f;

    [Header("Transition")]
    [SerializeField] private float transitionDistAtStart = 40.0f;
    [SerializeField] private float minTransitionDist = 40.0f;
    [SerializeField] private float maxTransitionDist = 80.0f;

    private Vector3 _currentPos;
    private Vector3 _currentBuildingPos = new Vector3(0.0f, -1.0f, 0.0f);
    
    private float _prevObstacleLength;
    private int _continuousObstacles;

    private void Start()
    {
        SetupBuildings();
    }

    private void SetupBuildings()
    {
        for (int i = 0; i < buildingsAtStart; i++)
        {
            SpawnBuilding();
        }
    }

    public void SetupObstacles(Vector3 startingPos)
    {
        _currentPos = startingPos;
        _currentPos += Vector3.forward * transitionDistAtStart;
        
        for (int i = 0; i < obstaclesAtStart; i++)
        {
            SpawnSegment();
        }
    }

    public void SpawnSegment()
    {
        ObstacleTile obstacle = obstaclePool.GetObstacle();
        GameObject obj = obstacle.gameObject;

        _currentPos += Vector3.forward * (obstacle.length / 2.0f + distBetweenObstacles + _prevObstacleLength / 2.0f);
        bool facingForward = Random.Range(0.0f, 1.0f) > 0.5f;
        
        Quaternion targetRot = Quaternion.Euler(facingForward ? Vector3.zero : Vector3.up * 180.0f); 
        obj.transform.SetPositionAndRotation(_currentPos, targetRot);

        obj.SetActive(true);

        _continuousObstacles++;

        if (_continuousObstacles >= Random.Range(minObstacles, maxObstacles))
        {
            _continuousObstacles = 0;
            
            float transitionDist = Random.Range(minTransitionDist, maxTransitionDist);
            _currentPos += Vector3.forward * (obstacle.length / 2.0f + transitionDist);
        }

        _prevObstacleLength = obstacle.length;
    }

    public void SpawnBuilding()
    {
        Building building = buildingsPool.GetBuilding();
        GameObject obj = building.gameObject;
        
        bool facingForward = Random.Range(0.0f, 1.0f) > 0.5f;
        Quaternion targetRot = Quaternion.Euler(facingForward ? new Vector3(-90.0f, 0.0f, 0.0f) : new Vector3(-90.0f, 180.0f, 0.0f));

        obj.transform.SetPositionAndRotation(_currentBuildingPos, targetRot);
        _currentBuildingPos += Vector3.forward * building.Length;
        
        obj.SetActive(true);
    }
}
