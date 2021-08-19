using UnityEngine;
using Random = UnityEngine.Random;

public class Coin : MonoBehaviour
{
    [SerializeField] private float minRotationSpeed = 80.0f;
    [SerializeField] private float maxRotationSpeed = 240.0f;

    private float rotationSpeed;

    private void Start()
    {
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Magnet"))
        {
            gameObject.SetActive(false);
        }
    }
}
