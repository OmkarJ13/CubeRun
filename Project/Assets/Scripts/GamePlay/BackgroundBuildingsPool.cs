using System.Collections.Generic;
using UnityEngine;

public class BackgroundBuildingsPool : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private List<Building> buildingPrefabs;
    [SerializeField] private int amountPerPrefab = 2;

    private readonly List<Building> _pool = new List<Building>();

    private void Awake()
    {
        InitPool();
    }

    private void InitPool()
    {
        foreach (Building buildingPrefab in buildingPrefabs)
        {
            for (int i = 0; i < amountPerPrefab; i++)
            {
                Building instantiatedObject = Instantiate(buildingPrefab);
                instantiatedObject.gameObject.SetActive(false);
                
                _pool.Add(instantiatedObject);
            }
        }
    }

    public Building GetBuilding()
    {
        int randomRange = Random.Range(0, _pool.Count);
        Building building = !_pool[randomRange].gameObject.activeSelf ? _pool[randomRange] : GetBuilding();

        return building;
    }
}
