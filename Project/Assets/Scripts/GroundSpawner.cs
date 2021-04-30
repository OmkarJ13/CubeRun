using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    private float length = 800.0f;

    private void OnBecameInvisible()
    {
        Vector3 nextPos = Vector3.forward * length * 2.0f;
        transform.localPosition += nextPos;
    }
}
