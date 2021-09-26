using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PowerUpTimer : MonoBehaviour
{
    [Header("Timer Info")] 
    [SerializeField] private TextMeshProUGUI powerUpText;
    [SerializeField] private Image powerUpIcon;
    [SerializeField] private Image fillTimer;
    [SerializeField] public UnityEvent onTimerComplete;

    [Header("Dependencies")]
    [SerializeField] private PowerUpWheel powerUpWheel;

    private void OnEnable()
    {
        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        powerUpText.text = powerUpWheel.activePowerUp.Name;
        powerUpIcon.sprite = powerUpWheel.activePowerUp.Icon;

        float maxTime = powerUpWheel.activePowerUp.Uptime;
        float remainingTime = maxTime;
        
        while (remainingTime > 0.0f)
        {
            remainingTime -= Time.deltaTime;
            fillTimer.fillAmount = remainingTime / maxTime;
            
            yield return null;
        }
        
        onTimerComplete.Invoke();
    }
}
