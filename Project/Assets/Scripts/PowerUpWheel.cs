using UnityEngine;

public class PowerUpWheel : MonoBehaviour
{
    [HideInInspector] public PowerUpWheelButton selectedButton;
    
    [SerializeField] private Player player;
    [SerializeField] private CoroutineHandler coroutineHandler;
    [SerializeField] private UIManager uiManager;

    private Animator animator;
    
    private static readonly int OpenPowerUpWheel = Animator.StringToHash("OpenPowerUpWheel");
    private static readonly int ClosePowerUpWheel = Animator.StringToHash("ClosePowerUpWheel");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ItemSelected()
    {
        switch (selectedButton.type)
        {
            case PowerUpType.MAGNET:
                coroutineHandler.StartPersistingCoroutine(player.ActivateMagnet(selectedButton));
                break;
            
            case PowerUpType.SHIELD:
                coroutineHandler.StartPersistingCoroutine(player.ActivateShield(selectedButton));
                break;
            
            case PowerUpType.DOUBLE_JUMP:
                coroutineHandler.StartPersistingCoroutine(player.ActivateDoubleJump(selectedButton));
                break;
            
            case PowerUpType.ROCKET:
                coroutineHandler.StartPersistingCoroutine(player.ActivateRocket(selectedButton));
                break;
        }
    }

    private void OnEnable()
    {
        player._isPowerUpWheelActive = true;
        animator.SetTrigger(OpenPowerUpWheel);
        
        Time.timeScale = 0.0f;

        selectedButton = null;
    }

    private void OnDisable()
    {
        player._isPowerUpWheelActive = false;
        Time.timeScale = 1.0f;
        
        uiManager.EnableHUD();
    }

    public void CancelSelection()
    {
        animator.SetTrigger(ClosePowerUpWheel);
    }

    public void ConfirmSelection()
    {
        ItemSelected();
        animator.SetTrigger(ClosePowerUpWheel);
    }

    public void DisablePowerUpWheel()
    {
        gameObject.SetActive(false);
    }
}
