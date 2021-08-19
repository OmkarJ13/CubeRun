using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
     [Header("Movement")]
     [SerializeField] private float forwardSpeed = 20.0f;
     [SerializeField] private float sidewaysSpeed = 10.0f;
     [SerializeField] private float turnSpeed = 10.0f;
     [SerializeField] private float jumpSpeed = 10.0f;
     [SerializeField] private float scaleSpeed = 10.0f;
     [SerializeField] private float speedModifier = 1f;
     [SerializeField] private float speedIncreaseTime = 10.0f;
     [SerializeField] private float maxSpeed = 40.0f;
     [SerializeField] private float laneDistance = 3.0f;
     [SerializeField] private float scaledDownTime = 0.75f;

     [Header("Power-Ups")] [Header("Power-Up Timer")] 
     [SerializeField] private GameObject powerUpTimerUI;
     
     [SerializeField] private TextMeshProUGUI powerUpText;
     [SerializeField] private Image powerUpIcon;
     [SerializeField] private Image powerUpTimer;
     
     [Header("Power-Up Wheel")]
     [SerializeField] private GameObject powerUpWheel;

     [Header("Shield")]
     [SerializeField] private GameObject shieldPrefab;
     [SerializeField] private float shieldUptime = 5.0f;
     [SerializeField] private float shieldDissolveSpeed = 2.5f;
     [SerializeField] private Vector3 deathBoxSize = new Vector3(50.0f, 50.0f, 50.0f);
     
     private static readonly int _shieldDissolveValue = Shader.PropertyToID("_Dissolve");
     private GameObject _shield;
     private Material _shieldMat;

     [Header("Magnet")] 
     [SerializeField] private GameObject magnetPrefab;
     [SerializeField] private float coinAttractSpeed = 10.0f;
     [SerializeField] private float magnetUptime = 5.0f;
     [SerializeField] private float magnetDissolveSpeed = 2.5f;
     [SerializeField] private Vector3 attractCoinsBox = new Vector3(50.0f, 50.0f, 50.0f);

     private static readonly int _magnetAlphaValue = Shader.PropertyToID("_Alpha");
     private GameObject _magnet;
     private Material[] _magnetMat;

     [Header("Double Jump")] 
     [SerializeField] private float doubleJumpUptime = 5.0f;
     private bool _canDoubleJump;

     [Header("Rocket")] 
     [SerializeField] private GameObject rocketPrefab;
     [SerializeField] private float rocketUptime = 5.0f;
     [SerializeField] private float rocketDissolveSpeed = 2.5f;
     [SerializeField] private float cameraRocketOffset = 3.0f;
     [SerializeField] private Vector3 flyAltitude = new Vector3(0.0f, 50.0f, 0.0f);

     private static readonly int _rocketDissolveValue = Shader.PropertyToID("_Dissolve");
     private GameObject _rocket;
     private Material[] _rocketMat;
     
     private bool _isAscending;

     [Header("Dependencies")]
     [SerializeField] private LayerMask obstacleLayerMask;
     [SerializeField] private LayerMask coinLayer;
     [SerializeField] private CameraShake cameraShake;
     [SerializeField] private SwipeManager swipeManager;
     [SerializeField] private GameManager gameManager;
     [SerializeField] private CoroutineHandler coroutineHandler;
     [SerializeField] private UIManager uiManager;
     [SerializeField] private FollowCamera followCamera;
     
     // Events
    public event Action ScoreUpdated;
    public event Action HighScoreUnlocked;
    public event Action CoinCollected;

    // Player Data
    public int CurrentScore { get; private set; }
    public int HighScore { get; private set; }
    
    public int CollectedCoins { get; private set; }

    // Components
    private CharacterController _controller;

    // Variables
    private float _verticalVelocity;
    private float _lastSpeedIncreaseTime;
    private int _desiredLane = 1;

    // Flags
    [HideInInspector] public bool _isPowerUpWheelActive;
    public bool _isDead { get; private set; }
    private bool _scalingDown;
    private bool _hasShield;
    private bool _hasMagnet;
    private bool _hasDoubleJump;
    private bool _hasRocket;
    private bool _useGravity = true;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
        SetupShieldPowerUp();
        SetupMagnetPowerUp();
        SetupRocketPowerUp();
    }

    private void SetupRocketPowerUp()
    {
        Transform playerTransform = transform;
        _rocket = Instantiate(rocketPrefab, playerTransform.position, Quaternion.identity, playerTransform);
        _rocketMat = _rocket.GetComponent<MeshRenderer>().materials;
        _rocket.SetActive(false);
    }

    private void SetupShieldPowerUp()
    {
        Transform playerTransform = transform;
        _shield = Instantiate(shieldPrefab, playerTransform.position, Quaternion.identity, playerTransform);
        _shieldMat = _shield.GetComponent<MeshRenderer>().material;
        _shield.SetActive(false);
    }

    private void SetupMagnetPowerUp()
    {
        Transform playerTransform = transform;
        _magnet = Instantiate(magnetPrefab, playerTransform.position + Vector3.up * 0.25f, Quaternion.identity, playerTransform);
        _magnetMat = _magnet.GetComponent<MeshRenderer>().materials;
        _magnet.SetActive(false);
    }

    private void Update()
    {
        HandleInput();
        HandleMovement();
    }

    private void LateUpdate()
    {
        UpdateScore();
        IncreaseDifficulty();
        RotateToVelocityDirection();
    }

    private void IncreaseDifficulty()
    {
        if (!_isDead && Time.time  - speedIncreaseTime > _lastSpeedIncreaseTime)
        {
            _lastSpeedIncreaseTime = Time.time;
            forwardSpeed += speedModifier;

            if (forwardSpeed > maxSpeed)
            {
                forwardSpeed -= speedModifier;
            }
        }
    }

    private void RotateToVelocityDirection()
    {
        Vector3 dir = _controller.velocity;
        transform.forward = Vector3.MoveTowards(transform.forward, dir, turnSpeed * Time.deltaTime);
    }

    private void HandleInput()
    {
        if (swipeManager.SwipeLeft)
            SetLane(true);
        else if (swipeManager.SwipeRight)
            SetLane(false);
        else if (swipeManager.DoubleTap)
            ActivatePowerUpWheel();
    }

    private void ActivatePowerUpWheel()
    {
        if (!_isPowerUpWheelActive)
        {
            uiManager.DisableHUD();
            powerUpWheel.SetActive(true);
        }
    }

    private void HandleMovement()
    {
        float currentX = transform.position.x;
        float targetX = _desiredLane switch
        {
            0 => -laneDistance,
            2 => laneDistance,
            _ => 0.0f
        };

        Vector3 moveDelta = Vector3.zero;

        float nextX = Mathf.MoveTowards(currentX, targetX, sidewaysSpeed * Time.deltaTime);
        if (_controller.isGrounded)
        {
            if (_hasDoubleJump)
                _canDoubleJump = true;
            
            if (!_hasRocket)
                _verticalVelocity = -0.1f;
            
            if (swipeManager.SwipeUp && !_hasRocket)
                Jump();
            else if (swipeManager.SwipeDown && !_scalingDown && !_hasRocket)
                StartCoroutine(ScaleDown());
        }
        else if (_controller.velocity.y < 0.0f)
        {
            _verticalVelocity += _useGravity ? Physics.gravity.y * 3.0f * Time.deltaTime : 0.0f;
            if (swipeManager.SwipeUp && _hasDoubleJump && !_hasRocket)
            {
                if (_canDoubleJump)
                {
                    Jump();
                    _canDoubleJump = false;
                }
            }
            else if (swipeManager.SwipeDown && !_hasRocket)
            {
                FastFall();
            }
        }
        else
        {
            _verticalVelocity += _useGravity ? Physics.gravity.y * 1.5f * Time.deltaTime : 0.0f;
            if (swipeManager.SwipeUp && _canDoubleJump && !_hasRocket)
            {
                Jump();
                _canDoubleJump = false;
            }
            else if (swipeManager.SwipeDown && !_hasRocket)
            {
                FastFall();
            }
        }

        moveDelta.x = nextX - currentX;
        moveDelta.y = _verticalVelocity * Time.deltaTime;

        if (_hasRocket && _isAscending)
            moveDelta.z = 0.0f;
        else
            moveDelta.z = forwardSpeed * Time.deltaTime; 
        
        _controller.Move(moveDelta);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Obstacle") && !_isDead)
        {
            if (_hasShield)
            {
                DestroyObstacles();
            }
            else
            {
                PlayerLost();
            }
        }
    }

    private void DestroyObstacles()
    {
        if (!_hasShield) return;
        
        StartCoroutine(cameraShake.Shake(0.2f, 0.2f));
        StartCoroutine(DeactivateShield());

        int layerMask = LayerMask.GetMask("Interactables", "Obstacles", "Coins");
        Collider[] results = Physics.OverlapBox(transform.position, deathBoxSize, Quaternion.identity, layerMask);

        if (results.Length > 0)
        {
            foreach (Collider result in results)
            {
                Destroy(result.gameObject);
            }
        }
    }

    private IEnumerator CountDownTimer(float upTime)
    {
        float remainingTime = upTime;
        while (remainingTime > 0.0f)
        {
            remainingTime -= Time.deltaTime;
            powerUpTimer.fillAmount = remainingTime / upTime;
            
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            CollectedCoins++;
            CoinCollected?.Invoke();
            
            other.gameObject.SetActive(false);
        }
    }

    private void SetupTimerUI(PowerUpWheelButton selectedButton)
    {
        powerUpTimerUI.SetActive(true);
        
        powerUpText.text = selectedButton.itemName;
        powerUpIcon.sprite = selectedButton.itemIcon;
        powerUpTimer.fillAmount = 1.0f;
    }

    private void RemoveTimerUI()
    {
        powerUpText.text = "";
        powerUpIcon.sprite = null;
        
        powerUpTimerUI.SetActive(false);
    }
    
    public IEnumerator ActivateRocket(PowerUpWheelButton buttonInfo)
    {
        _hasRocket = true;
        _rocket.SetActive(true);

        SetupTimerUI(buttonInfo);

        _useGravity = false;

        followCamera.offset += Vector3.up * cameraRocketOffset;
        StartCoroutine(followCamera.LookAt(transform));

        float value = 0.0f;
        while (value <= 1.0f)
        {
            foreach (var material in _rocketMat)
            {
                material.SetFloat(_rocketDissolveValue, value);
            }

            value += rocketDissolveSpeed * Time.deltaTime;

            yield return null;
        }
        
        _verticalVelocity = jumpSpeed;
        
        yield return new WaitForSeconds(1.0f);
        
        _verticalVelocity = 0.0f;
        forwardSpeed *= 2.0f;
        
        yield return StartCoroutine(CountDownTimer(rocketUptime));
        StartCoroutine(DeactivateRocket());
    }

    private IEnumerator DeactivateRocket()
    {
        RemoveTimerUI();
        forwardSpeed /= 2.0f;

        followCamera.offset -= Vector3.up * cameraRocketOffset;
        StartCoroutine(followCamera.ResetLookAt());

        float value = 1.0f;

        while (value >= 0.0f)
        {
            foreach (var material in _rocketMat)
            {
                material.SetFloat(_rocketDissolveValue, value);
            }

            value -= rocketDissolveSpeed * Time.deltaTime;

            yield return null;
        }
        
        _useGravity = true;
        _hasRocket = false;

        _rocket.SetActive(false);
    }
    
    public IEnumerator ActivateMagnet(PowerUpWheelButton buttonInfo)
    {
        _hasMagnet = true;
        _magnet.SetActive(true);
        
        SetupTimerUI(buttonInfo);
        
        StartCoroutine(AttractCoins());
        
        float alpha = 0.0f;
        float target = 1.0f;

        while (alpha < target)
        {
            foreach (Material material in _magnetMat)
            {
                material.SetFloat(_magnetAlphaValue, alpha);
            }

            alpha = Mathf.MoveTowards(alpha, target, magnetDissolveSpeed * Time.deltaTime);
            yield return null;
        }

        yield return StartCoroutine(CountDownTimer(magnetUptime));
        yield return StartCoroutine(DeactivateMagnet());
    }

    public IEnumerator ActivateDoubleJump(PowerUpWheelButton buttonInfo)
    {
        _hasDoubleJump = true;
        _canDoubleJump = true;
        
        SetupTimerUI(buttonInfo);

        yield return StartCoroutine(CountDownTimer(doubleJumpUptime));
        
        RemoveTimerUI();
        _canDoubleJump = false;
        _hasDoubleJump = false;
    }

    private IEnumerator AttractCoins()
    {
        while (_hasMagnet)
        {
            Transform magnetTransform = _magnet.transform;
            
            Collider[] coinsInArea = Physics.OverlapBox(magnetTransform.position, attractCoinsBox,
                Quaternion.identity, coinLayer);

            foreach (Collider coin in coinsInArea)
            {
                Transform coinTransform = coin.transform;
                Vector3 upOffset = Vector3.up * 0.75f;
                coinTransform.position = Vector3.MoveTowards(coinTransform.position, magnetTransform.position + upOffset, coinAttractSpeed * Time.deltaTime);
            }

            yield return null;
        }
    }

    private IEnumerator DeactivateMagnet()
    {
        RemoveTimerUI();
        
        float alpha = 1.0f;
        float target = 0.0f;

        while (alpha > target)
        {
            foreach (Material material in _magnetMat)
            {
                material.SetFloat(_magnetAlphaValue, alpha);
            }

            alpha = Mathf.MoveTowards(alpha, target, magnetDissolveSpeed * Time.deltaTime);
            yield return null;
        }
        
        _magnet.SetActive(false);
        _hasMagnet = false;
    }

    public IEnumerator ActivateShield(PowerUpWheelButton buttonInfo)
    {
        _hasShield = true;
        _shield.SetActive(true);
        
        SetupTimerUI(buttonInfo);

        float value = 0.0f;
        while (value <= 1.0f)
        {
            _shieldMat.SetFloat(_shieldDissolveValue, value);
            value += shieldDissolveSpeed * Time.deltaTime;

            yield return null;
        }

        yield return StartCoroutine(CountDownTimer(shieldUptime));
        yield return StartCoroutine(DeactivateShield());
    }

    private IEnumerator DeactivateShield()
    {
        RemoveTimerUI();
        
        float value = 1.0f;
        while (value >= 0.0f)
        {
            _shieldMat.SetFloat(_shieldDissolveValue, value);
            
            value -= shieldDissolveSpeed * Time.deltaTime;

            yield return null;
        }
        
        _hasShield = false;
        _shield.SetActive(false);
    }

    private void PlayerLost()
    {
        _isDead = true;

        coroutineHandler.StartPersistingCoroutine(cameraShake.Shake(0.2f, 0.2f));
        coroutineHandler.StartPersistingCoroutine(gameManager.GameOver());
        
        Destroy(gameObject);
    }

    private void Jump()
    {
        _verticalVelocity = jumpSpeed;
    }

    private void FastFall()
    {
        _verticalVelocity = -jumpSpeed;
    }

    private IEnumerator ScaleDown()
    {
        _scalingDown = true;
        
        Vector3 currentScale = transform.localScale;
        Vector3 targetScale = currentScale / 2.0f;

        while (currentScale != targetScale)
        {
            currentScale = Vector3.MoveTowards(currentScale, targetScale, scaleSpeed * Time.deltaTime);
            transform.localScale = currentScale;

            if (Physics.CheckBox(transform.position, currentScale / 2.0f, transform.rotation, obstacleLayerMask) && !_isDead)
            {
                if (_hasShield)
                    DestroyObstacles();
                else
                    PlayerLost();
            }

            yield return null;
        }
        
        yield return new WaitForSeconds(scaledDownTime);

        targetScale = Vector3.one;
        while (currentScale != targetScale)
        {
            currentScale = Vector3.MoveTowards(currentScale, targetScale, scaleSpeed * Time.deltaTime);
            transform.localScale = currentScale;

            if (Physics.CheckBox(transform.position, currentScale / 2.0f, transform.rotation, obstacleLayerMask) && !_isDead)
            {
                if (_hasShield)
                    DestroyObstacles();
                else
                    PlayerLost();
            }

            yield return null;
        }
        
        _scalingDown = false;
    }

    private void SetLane(bool goingLeft)
    {
        _desiredLane += goingLeft ? -1 : 1;
        _desiredLane = Mathf.Clamp(_desiredLane, 0, 2);
    }

    private void UpdateScore()
    {
        if (_isDead) return;
        
        CurrentScore = ((int) transform.position.z);
        if (CurrentScore > HighScore)
        {
            HighScore = CurrentScore;
            HighScoreUnlocked?.Invoke();
        }
        
        ScoreUpdated?.Invoke();
    }
}