using System;
using System.Collections;
using UnityEngine;

public class RocketPowerUp : PowerUp
{
    [SerializeField] private float cameraRocketOffset = 5.0f;
    [SerializeField] private float rocketAltitude = 30.0f;
    [SerializeField] private float rocketTakeOffSpeed = 30.0f;
    [SerializeField] private float rocketDissolveSpeed = 5.5f;
    
    private readonly int rocketDissolveValue = Shader.PropertyToID("_Dissolve");
    private Material[] rocketMat;

    // Dependencies
    private FollowCamera followCamera;
    private CoroutineHandler coroutineHandler;
    
    // Events
    public event Action RocketLanding;

    protected override void Awake()
    {
        base.Awake();
        
        InitDependencies();
        SetupRocketPowerUp();
    }

    private void InitDependencies()
    {
        followCamera = GameObject.FindGameObjectWithTag("CameraHolder").GetComponent<FollowCamera>();
        coroutineHandler = GameObject.FindGameObjectWithTag("CoroutineHandler").GetComponent<CoroutineHandler>();
    }

    private void SetupRocketPowerUp()
    {
        rocketMat = GetComponent<MeshRenderer>().materials;
        
        PowerUpData data = SaveSystem.GetData("Rocket") as PowerUpData;
        if (data != null)
        {
            uptime = data.uptime;
            print(uptime);
        }

        print(uptime);
    }

    private void OnEnable()
    {
        StartCoroutine(ActivateRocket());
    }
    
    public IEnumerator ActivateRocket()
    {
        player.useGravity = false;

        followCamera.AddOffset(Vector3.up * cameraRocketOffset);
        coroutineHandler.StartPersistingCoroutine(followCamera.LookAt(player.transform));

        player.SetVerticalVelocity(rocketTakeOffSpeed);

        float value = 0.0f;
        while (value <= 1.0f)
        {
            foreach (var material in rocketMat)
            {
                material.SetFloat(rocketDissolveValue, value);
            }

            value += rocketDissolveSpeed * Time.deltaTime;
            yield return null;
        }

        while (Math.Abs(player.transform.position.y - rocketAltitude) > 0.5f)
        {
            yield return null;
        }

        player.SetVerticalVelocity(0.0f);

        powerUpTimer.gameObject.SetActive(true);
        yield return new WaitForSeconds(uptime);
        
        StartCoroutine(DeactivateRocket());
    }
    
    private IEnumerator DeactivateRocket()
    {
        float value = 1.0f;
        while (value >= 0.0f)
        {
            foreach (var material in rocketMat)
            {
                material.SetFloat(rocketDissolveValue, value);
            }
            value -= rocketDissolveSpeed * Time.deltaTime;
            yield return null;
        }
        
        followCamera.AddOffset(-Vector3.up * cameraRocketOffset);
        StartCoroutine(followCamera.ResetLookAt());
        
        player.useGravity = true;

        powerUpWheel.activePowerUp = null;
        powerUpWheel.CheckButtons();
        
        RocketLanding?.Invoke();
        gameObject.SetActive(false);
    }
}
