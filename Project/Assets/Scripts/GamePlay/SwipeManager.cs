using System;
using System.Collections;
using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    [Header("Swipe Settings")]
    [SerializeField] private float touchDeadzone;
    [SerializeField] private float mouseDeadzone;

    [Header("Double Tap Settings")] 
    [SerializeField] private float doubleTapTime = 0.25f;
    
    public event Action SwipedLeft;
    public event Action SwipedRight;
    public event Action SwipedUp;
    public event Action SwipedDown;
    public event Action DoubleTapped;

    public bool disableAllControls
    {
        get => disableDoubleTap && disableSwipeUp && disableSwipeDown && disableSwipeLeft && disableSwipeRight;
        set => disableDoubleTap = disableSwipeUp = disableSwipeDown = disableSwipeLeft = disableSwipeRight = value;
    }
    
    public bool disableSwipeUp;
    public bool disableSwipeDown;
    public bool disableSwipeLeft;
    public bool disableSwipeRight;
    public bool disableDoubleTap;

    private Vector3 _start, _end, _swipeDelta;
    private bool hasSwiped, tapped;

    private void Update()
    {
        tapped = false;

        if (disableAllControls)
        {
            hasSwiped = false;
            return;
        }

        if (!hasSwiped)
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
                            hasSwiped = true;

                            float x = _swipeDelta.x;
                            float y = _swipeDelta.y;

                            if (Mathf.Abs(x) > Mathf.Abs(y)) // If the absolute value of x co-ordinate is greater than y, then the swipe lies on the left or right side of the screen, else up or down
                            {
                                if (x > 0f && !disableSwipeRight)
                                {
                                    SwipedRight?.Invoke();
                                }
                                else if (!disableSwipeLeft)
                                {
                                    SwipedLeft?.Invoke();
                                }
                            }
                            else
                            {
                                if (y > 0f && !disableSwipeUp)
                                {
                                    SwipedUp?.Invoke();
                                }
                                else if (!disableSwipeDown)
                                {
                                    SwipedDown?.Invoke();
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
                        hasSwiped = true;

                        float x = _swipeDelta.x;
                        float y = _swipeDelta.y;

                        if (Mathf.Abs(x) > Mathf.Abs(y))
                        {
                            if (x > 0f && !disableSwipeRight)
                            {
                                SwipedRight?.Invoke();
                            }
                            else if (x < 0f && !disableSwipeLeft)
                            {
                                SwipedLeft?.Invoke();
                            }
                        }
                        else
                        {
                            if (y > 0f && !disableSwipeUp)
                            {
                                SwipedUp?.Invoke();
                            }
                            else if (y < 0f && !disableSwipeDown)
                            {
                                SwipedDown?.Invoke();
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
                    if (hasSwiped)
                    {
                        hasSwiped = false;
                    }
                    else
                    {
                        tapped = true;
                    }
                    StartCoroutine(CheckForDoubleTap());
                }
                else if (Input.touches[0].phase == TouchPhase.Canceled)
                {
                    hasSwiped = false;
                }
            }
        }
        else if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (hasSwiped)
                {
                    hasSwiped = false;
                }
                else
                {
                    tapped = true;
                }

                StartCoroutine(CheckForDoubleTap());
            }
        }
    }

    private IEnumerator CheckForDoubleTap()
    {
        if (!disableAllControls)
        {
            yield return null;
        
            float timePassedSinceTap = 0.0f;
            while (timePassedSinceTap <= doubleTapTime)
            {
                timePassedSinceTap += Time.deltaTime;
                if (tapped && !disableDoubleTap)
                {
                    DoubleTapped?.Invoke();
                }
                
                yield return null;
            }
        }
    }

    private void Reset()
    {
        _start = _end = _swipeDelta = Vector3.zero; // Reset all the vectors
    }
}
