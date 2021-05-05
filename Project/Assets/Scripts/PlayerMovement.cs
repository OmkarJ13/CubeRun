using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    [SerializeField] private float maxVelocity = 50.0f;
    [SerializeField] private float forwardSpeed = 1500.0f;
    [SerializeField] private float sidewaysSpeed = 15.0f;
    [SerializeField] private float laneDistance = 3.0f;

    private Rigidbody rb;

    private Vector3 targetPos;
    private Vector3 currentVelocity;

    private int desiredLane = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (SwipeManager.Instance.SwipeLeft)
            SetLane(true);
        else if (SwipeManager.Instance.SwipeRight)
            SetLane(false);
    }

    private void FixedUpdate()
    {
        MoveForward();
    }

    private void OnDisable()
    {
        rb.freezeRotation = false;
    }

    private void SetLane(bool goingLeft)
    {
        desiredLane += goingLeft ? -1 : 1;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);
        StartCoroutine(ChangeLane());
    }

    private IEnumerator ChangeLane()
    {
        do
        {
            Vector3 currentPos = transform.position;
            
            targetPos = Vector3.zero;
            targetPos.x = desiredLane == 0 ? -laneDistance : desiredLane == 2 ? laneDistance : 0.0f;
            targetPos.y = currentPos.y;
            targetPos.z = currentPos.z;

            transform.position = Vector3.MoveTowards(currentPos, targetPos, sidewaysSpeed * Time.deltaTime);
            yield return null;
        } while (transform.position != targetPos);
    }

    private void MoveForward()
    {
        currentVelocity = rb.velocity;
        rb.AddForce(Vector3.forward * (forwardSpeed * Time.fixedDeltaTime));
        currentVelocity = Vector3.ClampMagnitude(currentVelocity, maxVelocity);
        rb.velocity = currentVelocity;
    }
}
