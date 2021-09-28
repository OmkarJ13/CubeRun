using System.Collections;
using UnityEngine;

public class DoubleJumpPowerUp : PowerUp
{
    protected override void Awake()
    {
        base.Awake();
        
        PowerUpData data = SaveSystem.GetData(name) as PowerUpData;
        if (data != null)
        {
            uptime = data.uptime;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(ActivateDoubleJump());
    }

    public IEnumerator ActivateDoubleJump()
    {
        player.canDoubleJump = true;
        
        powerUpTimer.gameObject.SetActive(true);
        yield return new WaitForSeconds(uptime);
        
        player.canDoubleJump = false;

        powerUpWheel.activePowerUp = null;
        powerUpWheel.CheckButtons();
        
        gameObject.SetActive(false);
    }
}
