using UnityEngine;

public class ShieldPowerUp : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100.0f;

    private void Update()
    {
        transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
    }
}
