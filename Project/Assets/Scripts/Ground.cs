using UnityEngine;

public class Ground : MonoBehaviour
{
    private Player player;
    
    private Transform thisTransform;
    private Transform playerTransform;
    
    private void Start()
    {
        player = FindObjectOfType<Player>();
        
        thisTransform = transform;
        playerTransform = player.transform;
    }

    private void Update()
    {
        if (player._isDead) return;
        
        Vector3 currentPos = thisTransform.position;
        thisTransform.position = new Vector3(currentPos.x, currentPos.y, playerTransform.position.z);
    }
}
