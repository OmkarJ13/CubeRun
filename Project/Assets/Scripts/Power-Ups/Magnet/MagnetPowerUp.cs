using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MagnetPowerUp : PowerUp
{
    [Header("Power-Up Settings")]
    [SerializeField] private Vector3 attractCoinsArea = new Vector3(50.0f, 50.0f, 50.0f);
    [SerializeField] private float magnetDissolveSpeed = 2.5f;
    
    private readonly int magnetAlphaValue = Shader.PropertyToID("_Alpha");
    private Material[] magnetMat;
    
    private CoroutineHandler coroutineHandler;

    private List<Collider> coinsInArea;
    private List<Collider> coinsBeingAttracted;
    
    private float coinAttractSpeed;

    protected override void Awake()
    {
        base.Awake();
        
        InitDependencies();
        SetupMagnetPowerUp();
    }

    private void OnEnable()
    {
        StartCoroutine(ActivateMagnet());
    }

    private void InitDependencies()
    {
        coroutineHandler = GameObject.FindGameObjectWithTag("CoroutineHandler").GetComponent<CoroutineHandler>();
    }

    private void SetupMagnetPowerUp()
    {
        magnetMat = GetComponent<MeshRenderer>().materials;
    }

    public IEnumerator ActivateMagnet()
    {
        float alpha = 0.0f;
        float target = 1.0f;

        while (alpha < target)
        {
            foreach (Material material in magnetMat)
            {
                material.SetFloat(magnetAlphaValue, alpha);
            }

            alpha = Mathf.MoveTowards(alpha, target, magnetDissolveSpeed * Time.deltaTime);
            yield return null;
        }

        powerUpTimer.gameObject.SetActive(true);

        StartCoroutine(AttractCoins());
        yield return new WaitForSeconds(uptime);
        
        yield return StartCoroutine(DeactivateMagnet());
    }
    private IEnumerator DeactivateMagnet()
    {
        float alpha = 1.0f;
        float target = 0.0f;

        while (alpha > target)
        {
            foreach (Material material in magnetMat)
            {
                material.SetFloat(magnetAlphaValue, alpha);
            }

            alpha = Mathf.MoveTowards(alpha, target, magnetDissolveSpeed * Time.deltaTime);
            yield return null;
        }

        powerUpWheel.activePowerUp = null;
        powerUpWheel.CheckButtons();
        
        gameObject.SetActive(false);
    }
    
    private IEnumerator AttractCoins()
    {
        coinsInArea = new List<Collider>();
        coinsBeingAttracted = new List<Collider>();

        LayerMask coinMask = LayerMask.GetMask("Coins");
        
        while (isActiveAndEnabled)
        {
            coinsInArea = Physics.OverlapBox(player.transform.position, attractCoinsArea,
                Quaternion.identity, coinMask).ToList();

            coinAttractSpeed = (player.GetForwardSpeed() * 2.0f);

            foreach (Collider coin in coinsInArea)
            {
                if (!coinsBeingAttracted.Contains(coin))
                    coroutineHandler.StartPersistingCoroutine(MoveToMagnet(coin));
            }

            yield return null;
        }
    }

    private IEnumerator MoveToMagnet(Collider coin)
    {
        coinsBeingAttracted.Add(coin);
        Transform coinTransform = coin.transform;
        
        Vector3 currentPos = coinTransform.position;
        Vector3 targetPos = transform.position + (Vector3.up * 0.75f);

        while (coin.gameObject.activeSelf && !player.isDead)
        {
            currentPos = Vector3.MoveTowards(currentPos, targetPos, coinAttractSpeed * Time.deltaTime);
            coinTransform.position = currentPos;
            
            targetPos = player.transform.position + (Vector3.up * 0.75f);
            yield return null;
        }

        coinsBeingAttracted.Remove(coin);
    }
}
