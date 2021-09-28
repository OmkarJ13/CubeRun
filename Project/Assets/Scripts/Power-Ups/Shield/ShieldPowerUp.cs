using System.Collections;
using UnityEngine;

public class ShieldPowerUp : PowerUp
{
    [Header("Power-Up")]
    [SerializeField] private float dissolveSpeed = 2.5f;

    private readonly int _shieldDissolveValue = Shader.PropertyToID("_Dissolve");
    private Material _shieldMat;

    protected override void Awake()
    {
        base.Awake();
        SetupShieldPowerUp();
    }

    private void OnEnable()
    {
        StartCoroutine(ActivateShield());
    }
    
    private void SetupShieldPowerUp()
    {
        _shieldMat = GetComponent<MeshRenderer>().material;
        
        PowerUpData data = SaveSystem.GetData(name) as PowerUpData;
        if (data != null)
        {
            uptime = data.uptime;
        }
    }

    private IEnumerator ActivateShield()
    {
        while (player.scalingDown)
        {
            yield return null;
        }
        
        float value = 0.0f;
        while (value <= 1.0f)
        {
            _shieldMat.SetFloat(_shieldDissolveValue, value);
            value += dissolveSpeed * Time.deltaTime;

            yield return null;
        }

        powerUpTimer.gameObject.SetActive(true);
        yield return new WaitForSeconds(uptime);

        yield return StartCoroutine(DeactivateShield());
    }
    
    public IEnumerator DeactivateShield()
    {
        float value = 1.0f;
        while (value >= 0.0f)
        {
            _shieldMat.SetFloat(_shieldDissolveValue, value);
            
            value -= dissolveSpeed * Time.deltaTime;

            yield return null;
        }

        powerUpWheel.activePowerUp = null;
        powerUpWheel.CheckButtons();
        
        gameObject.SetActive(false);
    }
}
