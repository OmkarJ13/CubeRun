using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    private float length;

    private void Awake()
    {
        length = transform.localScale.z * 10f;
    }

    private void OnBecameInvisible()
    {
        Vector3 nextPos = Vector3.forward * length * 2f;
        transform.localPosition += nextPos;
    }
}
