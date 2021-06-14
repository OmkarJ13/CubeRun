using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxVelocity = 35.0f;
    [SerializeField] private float forwardSpeed = 1250.0f;
    [SerializeField] private float sidewaysSpeed = 7.0f;
    [SerializeField] private float laneDistance = 3.0f;
    [SerializeField] private float rotationSpeed = 125.0f;
    [SerializeField] private float jumpMultiplier = 10.0f;
    [SerializeField] private float duckSpeed = 10.0f;

    [Header("Dependencies")] 
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private SwipeManager swipeManager;
    [SerializeField] private GameManager gameManager;

    public event Action ScoreUpdated;
    public event Action HighScoreUnlocked;

    public int CurrentScore { get; private set; }
    public int HighScore { get; private set; }

    private Rigidbody _rb;
    
    private Vector3 _targetPos;
    private Vector3 _currentVelocity;

    private int _desiredLane = 1;
    
    private bool _isRotating;
    private bool _isJumping;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    private void Update()
    {
        if (swipeManager.SwipeLeft)
            SetLane(true);
        else if (swipeManager.SwipeRight)
            SetLane(false);
        else if (swipeManager.SwipeUp)
            Jump();
        else if (swipeManager.SwipeDown)
            Duck();
    }

    private void FixedUpdate()
    {
        MoveForward();
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (!enabled) return;

        if (other.gameObject.CompareTag("Obstacle"))
        {
            if (_rb.freezeRotation)
                _rb.freezeRotation = false;

            StartCoroutine(cameraShake.Shake(0.05f, 0.04f));
            StartCoroutine(gameManager.GameOver());
        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            _isJumping = false;
            
            float angle = Vector3.Angle(transform.forward, Vector3.forward);
            if (!_isRotating && Math.Abs(angle % 90.0f - 0.0f) > 0.025f) // 0.025f = Tolerance
            {
                StartCoroutine(Rotate());
            }
        }
    }

    private void Jump()
    {
        if (!_isJumping)
        {
            _isJumping = true;
            _rb.AddForce(Vector3.up * jumpMultiplier, ForceMode.Impulse);
        }
    }

    private void Duck()
    {
        if (_isJumping)
        {
            _rb.AddForce(Vector3.down * duckSpeed, ForceMode.Impulse);
        }
    }

    private void SetLane(bool goingLeft)
    {
        _desiredLane += goingLeft ? -1 : 1;
        _desiredLane = Mathf.Clamp(_desiredLane, 0, 2);
        StartCoroutine(ChangeLane());
    }

    private IEnumerator ChangeLane()
    {
        do
        {
            Vector3 currentPos = _rb.position;
            
            _targetPos = Vector3.zero;
            _targetPos.x = _desiredLane switch
            {
                0 => -laneDistance,
                2 => laneDistance,
                _ => 0.0f
            };
            _targetPos.y = currentPos.y;
            _targetPos.z = currentPos.z;

            Vector3 nextPos = Vector3.MoveTowards(currentPos, _targetPos, sidewaysSpeed * Time.fixedDeltaTime);
            _rb.MovePosition(nextPos);

            yield return new WaitForFixedUpdate();
        } while (_rb.position != _targetPos);
    }

    private IEnumerator Rotate()
    {
        _isRotating = true;
        _rb.freezeRotation = true;

        Vector3 forwardAligned = AlignVector(transform.forward);
        Vector3 upAligned = AlignVector(transform.up);
        
        Quaternion targetRotation = Quaternion.LookRotation(forwardAligned, upAligned);

        while (_rb.rotation != targetRotation)
        {
            Quaternion nextRotation = Quaternion.RotateTowards(_rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            _rb.MoveRotation(nextRotation);
            
            yield return new WaitForFixedUpdate();
        }

        _rb.freezeRotation = false;
        _isRotating = false;
    }

    private Vector3 AlignVector(Vector3 v)
    {
        if (Mathf.Abs(v.x) < Mathf.Abs(v.y))
        {
            v.x = 0.0f;

            if (Mathf.Abs(v.y) < Mathf.Abs(v.z))
            {
                v.y = 0.0f;
            }
            else
            {
                v.z = 0.0f;
            }
        }
        else
        {
            v.y = 0.0f;

            if (Mathf.Abs(v.x) < Mathf.Abs(v.z))
            {
                v.x = 0.0f;
            }
            else
            {
                v.z = 0.0f;
            }
        }
        
        return v;
    }

    private void MoveForward()
    {
        _currentVelocity = _rb.velocity;
        _rb.AddForce(Vector3.forward * (forwardSpeed * Time.fixedDeltaTime));
        _currentVelocity = Vector3.ClampMagnitude(_currentVelocity, maxVelocity);
        _rb.velocity = _currentVelocity;
        
        UpdateScore();
    }

    private void UpdateScore()
    {
        CurrentScore = ((int) transform.position.z);
        if (CurrentScore > HighScore)
        {
            HighScore = CurrentScore;
            HighScoreUnlocked?.Invoke();
        }
        
        ScoreUpdated?.Invoke();
    }
}
