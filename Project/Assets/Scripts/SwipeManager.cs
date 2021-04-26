using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    public bool SwipeLeft => swipeLeft;
    public bool SwipeRight => swipeRight;
    public bool SwipeUp => swipeUp;
    public bool SwipeDown => swipeDown;
    
    [SerializeField] private float touchDeadzone;
    [SerializeField] private float mouseDeadzone;

    private Vector3 start, end, swipeDelta;

    private bool swipeLeft, swipeRight, swipeUp, swipeDown;
    private bool hasSwiped;

    private void Update()
    {
        swipeLeft = swipeRight = swipeUp = swipeDown = false; // Reset this variables every frame to get the exact frame the player swiped

        if (!hasSwiped)
        {
            #region Mobile Input
            if (Input.touchCount > 0)
            {
                Touch touch = Input.touches[0];
                if (touch.phase == TouchPhase.Began)
                {
                    start = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    end = touch.position; 
                    swipeDelta = new Vector3(end.x - start.x, end.y - start.y); // Subtract the initial position from the current position to get a vector in the direction of the swipe

                    if (swipeDelta.magnitude >= touchDeadzone)
                    {
                        hasSwiped = true;

                        float x = swipeDelta.x;
                        float y = swipeDelta.y;

                        if (Mathf.Abs(x) > Mathf.Abs(y)) // If the absolute value of x co-ordinate is greater than y, then the swipe lies on the left or right side of the screen, else up or down
                        {
                            if (x > 0f)
                            {
                                swipeRight = true;
                                Debug.Log("Swiped Right.");
                            }
                            else
                            {
                                swipeLeft = true;
                                Debug.Log("Swiped Left.");
                            }
                        }
                        else
                        {
                            if (y > 0f)
                            {
                                swipeUp = true;
                                Debug.Log("Swiped Up.");
                            }
                            else
                            {
                                swipeDown = true;
                                Debug.Log("Swiped Down");
                            }
                        }
                        Reset();
                    }
                }
            }
            #endregion

            #region Mouse Input
            if (Input.GetMouseButtonDown(0))
            {
                start = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                end = Input.mousePosition;
                swipeDelta = new Vector3(end.x - start.x, end.y - start.y);

                if (swipeDelta.magnitude >= mouseDeadzone)
                {
                    hasSwiped = true;

                    float x = swipeDelta.x;
                    float y = swipeDelta.y;

                    if (Mathf.Abs(x) > Mathf.Abs(y))
                    {
                        if (x > 0f)
                        {
                            swipeRight = true;
                            Debug.Log("Swiped Right.");
                        }
                        else
                        {
                            swipeLeft = true;
                            Debug.Log("Swiped Left.");
                        }
                    }
                    else
                    {
                        if (y > 0f)
                        {
                            swipeUp = true;
                            Debug.Log("Swiped Up.");
                        }
                        else
                        {
                            swipeDown = true;
                            Debug.Log("Swiped Down");
                        }
                    }
                    Reset();
                }
            }
            #endregion
        }
        else if (Input.touchCount > 0) // If the player has swiped but is still touching the screen, check if the touch has ended or canceled
        {
            if (Input.touches[0].phase == TouchPhase.Canceled || Input.touches[0].phase == TouchPhase.Ended)
            {
                hasSwiped = false;
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            hasSwiped = false;
        }
    }

    private void Reset()
    {
        start = end = swipeDelta = Vector3.zero; // Reset all the vectors
    }
}
