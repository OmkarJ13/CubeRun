using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinAmount;
    [SerializeField] private GameObject mainMenu;

    private void OnEnable()
    {
        UpdateCoins();
    }

    public void UpdateCoins()
    {
        int amount = PlayerPrefs.GetInt("Coins", 0);
        coinAmount.SetText(amount.ToString());
    }

    public void goBackToMainMenu()
    {
        gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }
}
