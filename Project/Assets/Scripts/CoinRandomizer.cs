using UnityEngine;

public class CoinRandomizer : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 100.0f)] private float spawnChance = 30.0f; 
    private bool shouldSpawn;
    
    private void OnEnable()
    {
        shouldSpawn = Random.Range(0.0f, 100.0f) < spawnChance;
        
        gameObject.SetActive(shouldSpawn);
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.gameObject.SetActive(shouldSpawn);
        }
    }
}
