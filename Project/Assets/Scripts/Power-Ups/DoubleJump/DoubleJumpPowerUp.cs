using System.Collections;
using UnityEngine;

public class DoubleJumpPowerUp : PowerUp
{
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
