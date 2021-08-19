using UnityEngine;

public class MagnetPowerUp : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10.0f;

    private void Update()
    {
        transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
    }
}
