using System;
using System.Collections;
using UnityEngine;

public class ShieldPowerUp : PowerUp
{
    [Header("Power-Up")]
    [SerializeField] private float dissolveSpeed = 2.5f;
    [SerializeField] private Vector3 deathBoxSize = new Vector3(50.0f, 50.0f, 50.0f);

    [Header("Dependencies")] 
    [SerializeField] private LayerMask obstacleMask;
    
    private static readonly int _shieldDissolveValue = Shader.PropertyToID("_Dissolve");
    private Material _shieldMat;

    // Dependencies
    private CameraShake cameraShake;

    protected override void Awake()
    {
        base.Awake();
        
        SetupShieldPowerUp();
        InitDependencies();
    }

    private void InitDependencies()
    {
        cameraShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
    }

    private void OnEnable()
    {
        StartCoroutine(ActivateShield());
    }
    
    private void SetupShieldPowerUp()
    {
        _shieldMat = GetComponent<MeshRenderer>().material;
    }

    public void DestroyObstacles()
    {
        StartCoroutine(cameraShake.Shake(0.2f, 0.2f));
        StartCoroutine(DeactivateShield());

        Collider[] results = Physics.OverlapBox(transform.position, deathBoxSize, Quaternion.identity, obstacleMask);

        if (results.Length > 0)
        {
            foreach (Collider result in results)
            {
                result.gameObject.SetActive(false);
            }
        }

        timer.StopAllCoroutines();
        timer.onTimerComplete.Invoke();
    }

    public IEnumerator ActivateShield()
    {
        float value = 0.0f;
        while (value <= 1.0f)
        {
            _shieldMat.SetFloat(_shieldDissolveValue, value);
            value += dissolveSpeed * Time.deltaTime;

            yield return null;
        }

        timer.gameObject.SetActive(true);
        yield return new WaitForSeconds(uptime);

        yield return StartCoroutine(DeactivateShield());
    }
    
    private IEnumerator DeactivateShield()
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
