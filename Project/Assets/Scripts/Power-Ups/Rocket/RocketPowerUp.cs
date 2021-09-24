using System;
using System.Collections;
using UnityEngine;

public class RocketPowerUp : PowerUp
{
    [Header("Power-Up")]
    public float rocketSpeed = 60.0f;
    [SerializeField] private float cameraRocketOffset = 3.0f;
    [SerializeField] private float rocketAltitude = 30.0f;
    [SerializeField] private float takeOffSpeed = 50.0f;

    [SerializeField] private float rocketDissolveSpeed = 2.5f;
    private static readonly int rocketDissolveValue = Shader.PropertyToID("_Dissolve");
    private Material[] rocketMat;

    // Dependencies
    private FollowCamera followCamera;
    
    // Events
    public event Action RocketEnded;

    protected override void Awake()
    {
        base.Awake();
        
        InitDependencies();
        SetupRocketPowerUp();
    }

    private void InitDependencies()
    {
        followCamera = GameObject.FindGameObjectWithTag("CameraHolder").GetComponent<FollowCamera>();
    }

    private void SetupRocketPowerUp()
    {
        rocketMat = GetComponent<MeshRenderer>().materials;
    }

    private void OnEnable()
    {
        StartCoroutine(ActivateRocket());
    }
    
    public IEnumerator ActivateRocket()
    {
        player.useGravity = false;

        followCamera.offset += Vector3.up * cameraRocketOffset;
        StartCoroutine(followCamera.LookAt(player.transform));

        player.verticalVelocity = rocketSpeed / 2.0f;

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

        while (Math.Abs(player.transform.position.y - rocketAltitude) > 0.1f)
        {
            yield return null;
        }

        player.verticalVelocity = 0.0f;

        timer.gameObject.SetActive(true);
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
        
        followCamera.offset -= Vector3.up * cameraRocketOffset;
        StartCoroutine(followCamera.ResetLookAt());
        
        player.useGravity = true;

        powerUpWheel.activePowerUp = null;
        powerUpWheel.CheckButtons();
        
        RocketEnded?.Invoke();

        gameObject.SetActive(false);
    }
}
