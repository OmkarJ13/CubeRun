using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PowerUpWheelCooldown : MonoBehaviour
{
    [Header("Cooldown Settings")]
    [SerializeField] private Image cooldownTimer;
    [SerializeField] private float cooldownTime = 10.0f;
    [SerializeField] private UnityEvent onCooldownComplete;

    private void OnEnable()
    {
        StartCoroutine(StartPowerUpWheelCoolDown());
    }

    private IEnumerator StartPowerUpWheelCoolDown()
    {
        float remainingTime = cooldownTime;
        while (remainingTime > 0.0f)
        {
            remainingTime -= Time.deltaTime;
            cooldownTimer.fillAmount = remainingTime / cooldownTime;

            yield return null;
        }
        
        onCooldownComplete.Invoke();
    }
}
