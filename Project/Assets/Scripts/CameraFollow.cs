using UnityEngine;

public class CameraFollow : MonoBehaviour
{   
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private void LateUpdate()
    {
        Vector3 desiredPos = target.position + offset;
        transform.position = desiredPos;
    }
}
