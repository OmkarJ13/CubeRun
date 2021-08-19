using System.Collections;
using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    [Header("Swipe Settings")]
    [SerializeField] private float touchDeadzone;
    [SerializeField] private float mouseDeadzone;

    [Header("Double Tap Settings")] 
    [SerializeField] private float doubleTapTime = 0.25f;
    
    public bool SwipeLeft { get; private set; }
    public bool SwipeRight { get; private set; }
    public bool SwipeUp { get; private set; }
    public bool SwipeDown { get; private set; }
    public bool Tap { get; private set; }
    public bool DoubleTap { get; private set; }
    
    private Vector3 _start, _end, _swipeDelta;
    
    private bool _hasSwiped;

    private void Update()
    {
        SwipeLeft = SwipeRight = SwipeUp = SwipeDown = Tap = DoubleTap = false; // Reset this variables every frame to get the exact frame the player swiped

        if (!_hasSwiped)
        {
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                #region Mobile Input
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.touches[0];
                    if (touch.phase == TouchPhase.Began)
                    {
                        _start = touch.position;
                    }
                    else if (touch.phase == TouchPhase.Moved)
                    {
                        _end = touch.position; 
                        _swipeDelta = new Vector3(_end.x - _start.x, _end.y - _start.y); // Subtract the initial position from the current position to get a vector in the direction of the swipe

                        if (_swipeDelta.magnitude >= touchDeadzone)
                        {
                            _hasSwiped = true;

                            float x = _swipeDelta.x;
                            float y = _swipeDelta.y;

                            if (Mathf.Abs(x) > Mathf.Abs(y)) // If the absolute value of x co-ordinate is greater than y, then the swipe lies on the left or right side of the screen, else up or down
                            {
                                if (x > 0f)
                                {
                                    SwipeRight = true;
                                    Debug.Log("Swiped Right.");
                                }
                                else
                                {
                                    SwipeLeft = true;
                                    Debug.Log("Swiped Left.");
                                }
                            }
                            else
                            {
                                if (y > 0f)
                                {
                                    SwipeUp = true;
                                    Debug.Log("Swiped Up.");
                                }
                                else
                                {
                                    SwipeDown = true;
                                    Debug.Log("Swiped Down");
                                }
                            }
                            Reset();
                        }
                    }
                }
                #endregion
            }
            else if (SystemInfo.deviceType == DeviceType.Desktop)
            {
                #region Mouse Input
                if (Input.GetMouseButtonDown(0))
                {
                    _start = Input.mousePosition;
                }
                else if (Input.GetMouseButton(0))
                {
                    _end = Input.mousePosition;
                    _swipeDelta = new Vector3(_end.x - _start.x, _end.y - _start.y);

                    if (_swipeDelta.magnitude >= mouseDeadzone)
                    {
                        _hasSwiped = true;

                        float x = _swipeDelta.x;
                        float y = _swipeDelta.y;

                        if (Mathf.Abs(x) > Mathf.Abs(y))
                        {
                            if (x > 0f)
                            {
                                SwipeRight = true;
                                Debug.Log("Swiped Right.");
                            }
                            else
                            {
                                SwipeLeft = true;
                                Debug.Log("Swiped Left.");
                            }
                        }
                        else
                        {
                            if (y > 0f)
                            {
                                SwipeUp = true;
                                Debug.Log("Swiped Up.");
                            }
                            else
                            {
                                SwipeDown = true;
                                Debug.Log("Swiped Down");
                            }
                        }
                        Reset();
                    }
                }
                #endregion
            }
        }
        
        CheckForSwipeEnd();
    }

    private void CheckForSwipeEnd()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            if (Input.touchCount > 0) // If the player has swiped but is still touching the screen, check if the touch has ended or canceled
            {
                if (Input.touches[0].phase == TouchPhase.Ended)
                {
                    if (_hasSwiped)
                    {
                        _hasSwiped = false;
                    }
                    else
                    {
                        Tap = true;
                    }
                    StartCoroutine(CheckForDoubleTap());
                }
                else if (Input.touches[0].phase == TouchPhase.Canceled)
                {
                    _hasSwiped = false;
                }
            }
        }
        else if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (_hasSwiped)
                {
                    _hasSwiped = false;
                }
                else
                {
                    Tap = true;
                }

                StartCoroutine(CheckForDoubleTap());
            }
        }
    }

    private IEnumerator CheckForDoubleTap()
    {
        yield return null;
        
        float timePassedSinceTap = 0.0f;

        while (timePassedSinceTap <= doubleTapTime)
        {
            timePassedSinceTap += Time.deltaTime;
            if (Tap)
            {
                // Debug.Log("Double Tap!");
                DoubleTap = true;
            }

            yield return null;
        }
    }

    private void Reset()
    {
        _start = _end = _swipeDelta = Vector3.zero; // Reset all the vectors
    }
}
