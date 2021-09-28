using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    public float length { get; private set; }
    
    protected LevelGenerator levelGenerator;
    protected Player player;
    protected Transform playerTransform;

    protected MeshRenderer[] childRenderers;

    protected float offset = 10.0f;

    protected virtual void Awake()
    {
        InitDependencies();
        CenterParent();
        CacheChildRenderers();
        
        length = GetLength();
    }

    private void InitDependencies()
    {
        levelGenerator = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        playerTransform = player.transform;
    }

    private void CacheChildRenderers()
    {
        childRenderers = GetComponentsInChildren<MeshRenderer>(true);
    }

    public float GetLength()
    {
        Bounds bounds = new Bounds(transform.position, Vector3.zero);
        foreach (MeshRenderer meshRenderer in childRenderers)
        {
            bounds.Encapsulate(meshRenderer.bounds);
            meshRenderer.receiveShadows = false;
        }

        return bounds.size.z;
    }

    protected void CenterParent()
    {
        List<Transform> children = new List<Transform>();
        Vector3 center = Vector3.zero;
        
        for (int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i));
        }

        foreach (Transform child in children)
        {
            center += child.position;
            child.SetParent(null);
        }

        center /= children.Count;
        center.x = 0.0f;
        center.y = 0.0f;

        transform.position = center;

        foreach (Transform child in children)
        {
            child.SetParent(transform);
        }
    }

    protected virtual void LateUpdate()
    {
        if (player.isDead) return;
        Vector3 distance = playerTransform.position - transform.position;

        if (distance.z >= length + offset)
        {
            if (levelGenerator.isActiveAndEnabled)
            {
                gameObject.SetActive(false);
                levelGenerator.SpawnSegment();
            }
        }
    }
}
