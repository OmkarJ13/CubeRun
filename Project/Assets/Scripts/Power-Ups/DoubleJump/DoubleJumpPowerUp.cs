using System.Collections;
using UnityEngine;

public class DoubleJumpPowerUp : PowerUp
{
    protected override void Awake()
    {
        base.Awake();

        if (SaveSystem.GetData(name) is PowerUpData data)
        {
            uptime = data.uptime;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(ActivateDoubleJump());
        audioManager.PlayClip("powerUp");
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
