using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// <para> Attach an instance of this class to your coin prefab, to give random rotation and to disable the coin when the player hits it. </para>
/// </summary>
public class Coin : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float minRotationSpeed = 80.0f;
    [SerializeField] private float maxRotationSpeed = 240.0f;

    // Dependencies
    private Player player;

    private Vector3 defaultPos;
    private float rotationSpeed;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        defaultPos = transform.localPosition;
    }

    private void OnEnable()
    {
        transform.localPosition = defaultPos;
    }

    private void Start()
    {
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
    }
}
