using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// <para> CoinRandomizer class spawns coins based on the spawnChance provided by the developer. Place coin GameObjects with
/// an Instance of the 'Coin' class attached to it as the children of the GameObject with this class attached on it. </para>
/// </summary>
public class CoinRandomizer : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] [Range(0.0f, 100.0f)] private float spawnChance = 30.0f;

    private readonly List<Transform> children = new List<Transform>();
    public bool shouldSpawn { get; private set; }

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i));
        }
    }

    private void OnEnable()
    {
        shouldSpawn = Random.Range(0.0f, 100.0f) < spawnChance;

        gameObject.SetActive(shouldSpawn);
        foreach (Transform child in children)
        {
            child.gameObject.SetActive(shouldSpawn);
        }
    }
}
