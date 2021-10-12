using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    [Header("Power-Up")]
    [SerializeField] protected string powerUpName;
    [SerializeField] protected Sprite icon;
    
    [Header("Dependencies")]
    [SerializeField] protected PowerUpWheel powerUpWheel;
    [SerializeField] protected PowerUpTimer powerUpTimer;
    
    public string Name => powerUpName;
    public Sprite Icon => icon;
    public float Uptime => uptime;

    protected float uptime;

    // Dependencies
    protected Player player;
    protected AudioManager audioManager;

    protected PowerUp()
    {
        uptime = 5.0f;
    }

    protected virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }
}
