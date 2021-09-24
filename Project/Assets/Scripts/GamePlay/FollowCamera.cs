using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para> This class is responsible for all the follow-based activities, particularly it is used to control the MainCamera to follow the player. </para>
/// </summary>
public class FollowCamera : MonoBehaviour
{   
    [Header("Follow Settings")]
    public List<Transform> followTargets;
    public Vector3 offset;
    [SerializeField] [Range (0.0f, 1.0f)] private float smoothTime = 0.1f;
    [SerializeField] private float lookAtRotationSpeed = 10.0f;

    private Vector3 velocity;

    private void LateUpdate()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        Vector3 targetPosition = GetCenterPoint();
        
        Vector3 desiredPos = targetPosition + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, smoothTime);
    }

    private Vector3 GetCenterPoint()
    {
        if (followTargets.Count == 0)
        {
            Debug.LogError("Follow target is NULL!");
        }
        else if (followTargets.Count == 1)
        {
            return followTargets[0].position;
        }

        return GetEncapsulatingBounds().center;
    }

    private Bounds GetEncapsulatingBounds()
    {
        Bounds bounds = new Bounds(followTargets[0].position, Vector3.zero);

        foreach (Transform followTarget in followTargets)
        {
            bounds.Encapsulate(followTarget.position);
        }
        
        return bounds;
    }

    public IEnumerator LookAt(Transform target)
    {
        Quaternion currentRot = transform.rotation;
        Quaternion targetRot = Quaternion.LookRotation(target.position - transform.position);

        while (currentRot != targetRot)
        {
            currentRot = Quaternion.RotateTowards(currentRot, targetRot, lookAtRotationSpeed * Time.deltaTime);
            transform.rotation = currentRot;
            
            yield return null;
        }
    }

    public IEnumerator ResetLookAt()
    {
        Quaternion currentRot = transform.rotation;
        Quaternion targetRot = Quaternion.Euler(Vector3.forward);

        while (currentRot != targetRot)
        {
            currentRot = Quaternion.RotateTowards(currentRot, targetRot, lookAtRotationSpeed * Time.deltaTime);
            transform.rotation = currentRot;
            
            yield return null;
        }
    }
}
