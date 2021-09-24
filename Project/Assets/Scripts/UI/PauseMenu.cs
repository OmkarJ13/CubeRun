using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private UIManager uIManager;
    private SwipeManager swipeManager;

    private void Awake()
    {
        uIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        swipeManager = GameObject.FindGameObjectWithTag("SwipeManager").GetComponent<SwipeManager>();
    }

    private void OnEnable()
    {
        Time.timeScale = 0.0f;
        swipeManager.disableAllControls = true;
        
        uIManager.DisableHUD();
    }

    private void OnDisable()
    {
        Time.timeScale = 1.0f;
        swipeManager.disableAllControls = false;
        
        uIManager.EnableHUD();
    }
}
