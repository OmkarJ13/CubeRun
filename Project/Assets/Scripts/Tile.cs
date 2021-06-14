using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    public float Length { get; protected set; }

    protected LevelGenerator LevelGenerator;
    protected Transform Player;

    protected virtual void Awake()
    {
        LevelGenerator = FindObjectOfType<LevelGenerator>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;

        Length = GetComponent<MeshRenderer>().bounds.size.z;
    }

    protected virtual void LateUpdate()
    {
        Vector3 distance = Player.position - transform.position;

        if (distance.z >= Length)
        {
            if (LevelGenerator.isActiveAndEnabled)
            {
                gameObject.SetActive(false);
                LevelGenerator.SpawnSegment();
            }
        }
    }
}
