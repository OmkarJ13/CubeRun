using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;


public class Player : MonoBehaviour
{
    [Header("Movement")] 
    [Header("Speed")] 
    [SerializeField] private float forwardSpeed = 20.0f;
    [SerializeField] private float sidewaysSpeed = 10.0f;
    [SerializeField] private float turnSpeed = 10.0f;
    [SerializeField] private float jumpSpeed = 10.0f;
    [SerializeField] private float scaleSpeed = 10.0f;
    [SerializeField] private float flyingSpeed = 80.0f;

    [Header("Speed Modifiers")] 
    [SerializeField] private float speedModifier = 0.1f;

    [SerializeField] private float jumpModifier = 0.1f;
    [SerializeField] private float gravityModifier = 0.2f;
    [SerializeField] private float speedIncreaseTime = 10.0f;

    [Header("Max Speeds")] 
    [SerializeField] private float maxSpeed = 40.0f;
    [SerializeField] private float maxJumpSpeed = 20.0f;
    [SerializeField] private float maxGravity = -39.24f;

    [Header("Others")] 
    [SerializeField] private float laneDistance = 3.0f;
    [SerializeField] private float scaledDownTime = 0.75f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float gameOverDelay = 1.0f;

    [Header("Dependencies")] 
    [SerializeField] private GameObject shatteredPlayerPrefab;
    [SerializeField] private GameObject gameOverWidget;
    [SerializeField] private PowerUpWheel powerUpWheel;
    [SerializeField] private PowerUpWheelCooldown powerUpWheelCooldown;
    [SerializeField] private PowerUpTimer powerUpTimer;

    // Power-Ups
    public ShieldPowerUp shield { get; private set; }
    public MagnetPowerUp magnet { get; private set; }
    public DoubleJumpPowerUp doubleJump { get; private set; }
    public RocketPowerUp rocket { get; private set; }

    // Dependencies
    private CameraShake cameraShake;
    private CoroutineHandler coroutineHandler;
    private LevelGenerator levelGenerator;
    private FollowCamera followCamera;
    private SwipeManager swipeManager;
    private GameManager gameManager;
    private UIManager uiManager;

    // Events
    public event Action ScoreUpdated;
    public event Action HighScoreUnlocked;
    public event Action CoinCollected;

    // Player Data
    public int CurrentScore { get; private set; }
    public int HighScore { get; private set; }
    public int CollectedCoins { get; private set; }

    // Components
    private CharacterController controller;
    
    // Variables
    private GameObject shatteredPlayer;
    private float verticalVelocity;
    private float lastSpeedIncreaseTime;
    private int desiredLane = 1;

    // Flags
    [HideInInspector] public bool canDoubleJump;
    [HideInInspector] public bool useGravity;

    public bool isDead { get; private set; }
    public bool hasAnyPowerUp { get; private set; }
    public bool isGrounded { get; private set; }
    public bool scalingDown { get; private set; }

    private void Awake()
    {
        InitDependencies();
        InitPowerUps();
    }

    private void InitDependencies()
    {
        controller = GetComponent<CharacterController>();
        cameraShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
        coroutineHandler = GameObject.FindGameObjectWithTag("CoroutineHandler").GetComponent<CoroutineHandler>();
        levelGenerator = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();
        followCamera = GameObject.FindGameObjectWithTag("CameraHolder").GetComponent<FollowCamera>();
        swipeManager = GameObject.FindGameObjectWithTag("SwipeManager").GetComponent<SwipeManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
    }

    private void InitPowerUps()
    {
        shield = GetComponentInChildren<ShieldPowerUp>(true);
        rocket = GetComponentInChildren<RocketPowerUp>(true);
        magnet = GetComponentInChildren<MagnetPowerUp>(true);
        doubleJump = GetComponentInChildren<DoubleJumpPowerUp>(true);
    }

    private void OnEnable()
    {
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        swipeManager.SwipedLeft += OnSwipedLeft;
        swipeManager.SwipedRight += OnSwipedRight;
        swipeManager.SwipedUp += OnSwipedUp;
        swipeManager.SwipedDown += OnSwipedDown;
        swipeManager.DoubleTapped += OnDoubleTapped;
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void UnsubscribeEvents()
    {
        swipeManager.SwipedLeft -= OnSwipedLeft;
        swipeManager.SwipedRight -= OnSwipedRight;
        swipeManager.SwipedUp -= OnSwipedUp;
        swipeManager.SwipedDown -= OnSwipedDown;
        swipeManager.DoubleTapped -= OnDoubleTapped;
    }

    private void Start()
    {
        CorrectCenter();
        InitPlayer();
    }

    private void InitPlayer()
    {
        speedModifier = forwardSpeed * 0.1f;
        jumpModifier = jumpSpeed * 0.1f;
        gravityModifier = gravity * 0.3f;

        useGravity = true;
    }

    private void CorrectCenter()
    {
        float correctCenter = controller.center.y + controller.skinWidth;
        controller.center = Vector3.up * correctCenter;
    }

    private void Update()
    {
        GroundCheck();
        HandleMovement();
        CheckForPowerUps();
    }

    private void CheckForPowerUps()
    {
        hasAnyPowerUp = magnet.isActiveAndEnabled || shield.isActiveAndEnabled || doubleJump.isActiveAndEnabled ||
                        rocket.isActiveAndEnabled;
    }

    private void LateUpdate()
    {
        UpdateScore();
        IncreaseDifficulty();
        RotateToVelocityDirection();
    }

    public float GetForwardSpeed()
    {
        return forwardSpeed;
    }

    private void IncreaseDifficulty()
    {
        if (!isDead && Time.time - speedIncreaseTime > lastSpeedIncreaseTime)
        {
            lastSpeedIncreaseTime = Time.time;

            if (forwardSpeed < maxSpeed)
            {
                forwardSpeed += speedModifier;
                jumpSpeed += jumpModifier;
                gravity += gravityModifier;
            }
            else if (forwardSpeed > maxSpeed)
            {
                forwardSpeed = maxSpeed;
                jumpSpeed = maxJumpSpeed;
                gravity = maxGravity;
            }
        }
    }

    private void RotateToVelocityDirection()
    {
        Vector3 dir = controller.velocity;
        transform.forward = Vector3.MoveTowards(transform.forward, dir, turnSpeed * Time.deltaTime);
    }

    private void OnSwipedLeft()
    {
        SetLane(true);
    }

    private void OnSwipedRight()
    {
        SetLane(false);
    }

    private void OnSwipedUp()
    {
        if (isGrounded)
        {
            if (!rocket.isActiveAndEnabled)
            {
                Jump();
            }
        }
        else if (controller.velocity.y < 0.0f)
        {
            if (doubleJump.isActiveAndEnabled && !rocket.isActiveAndEnabled)
            {
                if (canDoubleJump)
                {
                    Jump();
                    canDoubleJump = false;
                }
            }
        }
        else
        {
            if (canDoubleJump && !rocket.isActiveAndEnabled)
            {
                Jump();
                canDoubleJump = false;
            }
        }
    }

    private void OnSwipedDown()
    {
        if (isGrounded)
        {
            if (!scalingDown)
            {
                StartCoroutine(ScaleDown());
            }
        }
        else
        {
            if (!rocket.isActiveAndEnabled && !scalingDown)
            {
                FastFall();
            }
        }
    }

    private void OnDoubleTapped()
    {
        if (!hasAnyPowerUp && !powerUpWheelCooldown.isActiveAndEnabled)
        {
            ActivatePowerUpWheel();
        }
    }

    private void ActivatePowerUpWheel()
    {
        if (!powerUpWheel.isActiveAndEnabled)
        {
            powerUpWheel.gameObject.SetActive(true);
        }
    }

    private void HandleMovement()
    {
        float currentX = transform.position.x;
        float targetX = desiredLane switch
        {
            0 => -laneDistance,
            2 => laneDistance,
            _ => 0.0f
        };

        Vector3 moveDelta = Vector3.zero;
        float nextX = Mathf.MoveTowards(currentX, targetX, sidewaysSpeed * Time.deltaTime);

        if (controller.isGrounded)
        {
            if (doubleJump.isActiveAndEnabled)
                canDoubleJump = true;
        }
        else if (controller.velocity.y < 0.0f)
        {
            verticalVelocity += useGravity ? gravity * 3.0f * Time.deltaTime : 0.0f;
        }
        else
        {
            verticalVelocity += useGravity ? gravity * 1.5f * Time.deltaTime : 0.0f;
        }

        moveDelta.x = nextX - currentX;
        moveDelta.y = verticalVelocity * Time.deltaTime;

        if (rocket.isActiveAndEnabled)
            moveDelta.z = flyingSpeed * Time.deltaTime;
        else
            moveDelta.z = forwardSpeed * Time.deltaTime;

        controller.Move(moveDelta);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Obstacle") && !isDead)
        {
            if (shield.isActiveAndEnabled)
            {
                UseShield();
            }
            else
            {
                PlayerLost();
            }
        }
    }

    private void UseShield()
    {
        if (shield.isActiveAndEnabled)
        {
            DestroySurroundings();
            coroutineHandler.StartPersistingCoroutine(cameraShake.Shake(0.2f, 0.2f));
            coroutineHandler.StartPersistingCoroutine(shield.DeactivateShield());
            powerUpTimer.onTimerComplete?.Invoke();
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

    private void PlayerLost()
    {
        isDead = true;

        coroutineHandler.StartPersistingCoroutine(cameraShake.Shake(0.2f, 0.2f));
        coroutineHandler.StartPersistingCoroutine(GameOver());

        gameObject.SetActive(false);
    }

    public IEnumerator GameOver()
    {
        PlayerPrefs.SetInt("HighScore", HighScore);

        int currentCoins = PlayerPrefs.GetInt("Coins", 0);
        PlayerPrefs.SetInt("Coins", currentCoins + CollectedCoins);

        ReplacePlayerMesh();

        levelGenerator.enabled = false;
        uiManager.DisableHUD();

        yield return new WaitForSeconds(gameOverDelay);

        gameOverWidget.SetActive(true);
    }

    private void ReplacePlayerMesh()
    {
        Transform playerTransform = transform;
        shatteredPlayer = Instantiate(shatteredPlayerPrefab, playerTransform.position, playerTransform.rotation);
        shatteredPlayer.transform.localScale = playerTransform.localScale;
        followCamera.followTargets = new List<Transform>();

        for (int i = 0; i < shatteredPlayer.transform.childCount; i++)
        {
            followCamera.followTargets.Add(shatteredPlayer.transform.GetChild(i));
        }
    }
    
    public void RevivePlayer()
    {
        DestroySurroundings();
        
        Destroy(shatteredPlayer);
        gameObject.SetActive(true);

        followCamera.followTargets = new List<Transform> { transform };
        isDead = false;

        levelGenerator.enabled = true;
        uiManager.EnableHUD();
    }
    
    private void DestroySurroundings()
    {
        Collider[] results = Physics.OverlapBox(transform.position, new Vector3(100, 100, 100), Quaternion.identity, LayerMask.GetMask("Obstacles"));
        if (results.Length > 0)
        {
            foreach (Collider result in results)
            {
                result.gameObject.SetActive(false);
            }
        }
    }

    public void Jump()
    {
        verticalVelocity = jumpSpeed;
    }

    private void FastFall()
    {
        verticalVelocity = -jumpSpeed;
    }

    public IEnumerator ScaleDown()
    {
        scalingDown = true;

        Vector3 currentScale = transform.localScale;
        Vector3 targetScale = currentScale / 2.0f;

        float initialWidth = controller.skinWidth;

        float currentWidth = initialWidth;
        float targetWidth = initialWidth / 2.0f;

        while (currentScale != targetScale)
        {
            currentScale = Vector3.MoveTowards(currentScale, targetScale, scaleSpeed * Time.deltaTime);
            transform.localScale = currentScale;

            currentWidth = Mathf.MoveTowards(currentWidth, targetWidth, scaleSpeed * Time.deltaTime);
            controller.skinWidth = currentWidth;

            if (Physics.CheckBox(transform.position, currentScale / 2.0f, transform.rotation,
                LayerMask.GetMask("Obstacles")) && !isDead)
            {
                if (shield.isActiveAndEnabled)
                    UseShield();
                else
                    PlayerLost();
            }

            yield return null;
        }

        yield return new WaitForSeconds(scaledDownTime);

        targetScale = Vector3.one;
        targetWidth = initialWidth;

        while (currentScale != targetScale)
        {
            currentScale = Vector3.MoveTowards(currentScale, targetScale, scaleSpeed * Time.deltaTime);
            transform.localScale = currentScale;

            currentWidth = Mathf.MoveTowards(currentWidth, targetWidth, scaleSpeed * Time.deltaTime);
            controller.skinWidth = currentWidth;

            if (Physics.CheckBox(transform.position, currentScale / 2.0f, transform.rotation,
                LayerMask.GetMask("Obstacles")) && !isDead)
            {
                if (shield.isActiveAndEnabled)
                    UseShield();
                else
                    PlayerLost();
            }

            yield return null;
        }

        scalingDown = false;
    }

    private void SetLane(bool goingLeft)
    {
        desiredLane += goingLeft ? -1 : 1;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);
    }

    private void UpdateScore()
    {
        if (isDead) return;

        CurrentScore = ((int)transform.position.z);
        if (CurrentScore > HighScore)
        {
            HighScore = CurrentScore;
            HighScoreUnlocked?.Invoke();
        }

        ScoreUpdated?.Invoke();
    }

    private void GroundCheck()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, controller.radius + controller.skinWidth);
    }

    public void SetVerticalVelocity(float value)
    {
        verticalVelocity = value;
    }
}