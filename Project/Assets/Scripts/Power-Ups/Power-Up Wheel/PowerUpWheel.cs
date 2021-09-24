using UnityEngine;

public class PowerUpWheel : MonoBehaviour
{
    [Header("Power-Up Wheel")]
    [HideInInspector] public PowerUpWheelButton selectedButton;
    [HideInInspector] public PowerUp activePowerUp;

    [Header("Dependencies")]
    [SerializeField] private PowerUpWheelCooldown cooldownTimer;

    // Dependencies
    private UIManager uiManager;
    private SwipeManager swipeManager;
    private Player player;

    private PowerUpWheelButton[] allButtons;

    private void Awake()
    {
        InitDependencies();
    }

    private void InitDependencies()
    {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        swipeManager = GameObject.FindGameObjectWithTag("SwipeManager").GetComponent<SwipeManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        
        allButtons = GetComponentsInChildren<PowerUpWheelButton>();
    }

    private void ItemSelected()
    {
        switch (selectedButton.type)
        {
            case PowerUpType.MAGNET:
                activePowerUp = player.magnet;
                player.magnet.gameObject.SetActive(true);
                break;
            
            case PowerUpType.SHIELD:
                activePowerUp = player.shield;
                player.shield.gameObject.SetActive(true);
                break;
            
            case PowerUpType.DOUBLE_JUMP:
                activePowerUp = player.doubleJump;
                player.doubleJump.gameObject.SetActive(true);
                break;
            
            case PowerUpType.ROCKET:
                activePowerUp = player.rocket;
                player.rocket.gameObject.SetActive(true);
                break;
        }
        
        selectedButton.gameObject.SetActive(false);
    }

    public void CheckButtons()
    {
        foreach (var powerUpWheelButton in allButtons)
        {
            if (powerUpWheelButton.gameObject.activeSelf)
                return;
        }
        
        ReactivateWheel();
    }

    private void ReactivateWheel()
    {
        cooldownTimer.gameObject.SetActive(true);
        foreach (PowerUpWheelButton powerUpWheelButton in allButtons)
        {
            powerUpWheelButton.gameObject.SetActive(true);
        }
    }

    private void OnEnable()
    {
        Time.timeScale = 0.0f;
        selectedButton = null;
        uiManager.DisableHUD();

        swipeManager.disableAllControls = true;
    }

    private void OnDisable()
    {
        Time.timeScale = 1.0f;
        uiManager.EnableHUD();

        swipeManager.disableAllControls = false;
    }

    public void ConfirmSelection()
    {
        if (selectedButton)
            ItemSelected();
    }
}
