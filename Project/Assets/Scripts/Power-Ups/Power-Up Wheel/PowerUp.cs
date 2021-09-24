using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    public string Name => powerUpName;
    public Sprite Icon => icon;
    public float Uptime => uptime;

    [Header("Power-Up")]
    [SerializeField] protected string powerUpName;
    [SerializeField] protected Sprite icon;
    [SerializeField] protected float uptime;
    
    [Header("Dependencies")]
    [SerializeField] protected PowerUpWheel powerUpWheel;
    [SerializeField] protected PowerUpTimer timer;
    
    // Dependencies
    protected Player player;

    protected virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
}
