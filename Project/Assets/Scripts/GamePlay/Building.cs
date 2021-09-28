using UnityEngine;

public class Building : MonoBehaviour
{
    public float Length { get; private set; }
    
    private MeshRenderer meshRenderer;
    private Player player;
    private LevelGenerator levelGenerator;
    private Transform playerTransform;

    private const float offset = 10.0f;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        levelGenerator = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();
        
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.receiveShadows = false;
        
        playerTransform = player.transform;
        
        CalculateLength();
    }

    private void CalculateLength()
    {
        Length = meshRenderer.bounds.size.z;
    }

    private void LateUpdate()
    {
        if (player.isDead) return;
        Vector3 distance = playerTransform.position - transform.position;

        if (distance.z >= Length + offset)
        {
            if (levelGenerator.isActiveAndEnabled)
            {
                gameObject.SetActive(false);
                levelGenerator.SpawnBuilding();
            }
        }
    }
}
