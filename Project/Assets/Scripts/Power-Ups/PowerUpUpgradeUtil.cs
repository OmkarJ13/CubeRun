using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpUpgradeUtil : MonoBehaviour
{
    [SerializeField] private string powerUpName;
    
    // UI 
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI currentUptimeText;
    [SerializeField] private Image currentLevelProgressBar;
    [SerializeField] private ModalWindow cannotAffordPrompt;

    // Default Values
    private int upgradeCost = 500;
    private float currentUptime = 5.0f;
    private float currentLevel = 0.0f;

    // Increment Factors
    private int costIncrease = 1000;
    private float uptimeIncrease = 5.0f;
    private float levelIncrease = 0.2f;
    
    // Dependencies
    private Shop shop;
    private AudioManager audioManager;

    private void Awake()
    {
        shop = GetComponentInParent<Shop>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void OnEnable()
    {
        if (SaveSystem.GetData(powerUpName) is PowerUpData data)
        {
            upgradeCost = data.cost;
            currentUptime = data.uptime;
            currentLevel = data.upgradeLevel;
            
            SetUI();
        }
        else
        {
            SetUI();
        }
    }

    public void Upgrade()
    {
        DeductCoins();
    }

    private void SetUI()
    {
        if (currentLevel == 1.0f)
        {
            costText.SetText("MAX");
            GetComponentInChildren<Button>().interactable = false;
        }
        else
        {
            costText.SetText(upgradeCost.ToString());
        }
        
        currentUptimeText.SetText(currentUptime + "s");
        currentLevelProgressBar.fillAmount = currentLevel;
    }

    private void DeductCoins()
    {
        int currentCoins = PlayerPrefs.GetInt("Coins", 0);
        if (upgradeCost > currentCoins)
        {
            cannotAffordPrompt.gameObject.SetActive(true);
            audioManager.PlayClip("buttonClick");
        }
        else
        {
            currentCoins -= upgradeCost;
            PlayerPrefs.SetInt("Coins", currentCoins);
            
            shop.UpdateCoins();
            SaveUpgradedPowerUp();
            
            audioManager.PlayClip("upgrade");
        }
    }

    private void SaveUpgradedPowerUp()
    {
        upgradeCost += costIncrease;
        currentUptime += uptimeIncrease;
        currentLevel += levelIncrease;
        
        PowerUpData toSave = new PowerUpData(upgradeCost, currentUptime, currentLevel);
        SaveSystem.SaveData(powerUpName, toSave);

        SetUI();
    }
}
