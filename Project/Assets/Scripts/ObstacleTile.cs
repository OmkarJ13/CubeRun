using UnityEngine;

public class ObstacleTile : Tile
{
    public ObstacleType Type => type;
    [SerializeField] private ObstacleType type;
}
