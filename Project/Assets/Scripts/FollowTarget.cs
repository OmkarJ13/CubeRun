using UnityEngine;

public class FollowTarget : MonoBehaviour
{   
    [Header("Follow Target Settings")]
    [SerializeField] private Player player;
    [SerializeField] private Vector3 offset;

    private Transform _playerTransform;

    private void Start()
    {
        _playerTransform = player.transform;
    }

    private void LateUpdate()
    {
        Vector3 desiredPos = _playerTransform.position + offset;
        transform.position = desiredPos;
    }
}
