using System.Collections;
using UnityEngine;

public class SwipeUpTrigger : MonoBehaviour
{
    [SerializeField] private GameObject swipeUpWidget;
    private SwipeManager swipeManager;

    private bool swipedUp;

    private void Awake()
    {
        swipeManager = GameObject.FindGameObjectWithTag("SwipeManager").GetComponent<SwipeManager>();
    }

    private void OnDisable()
    {
        swipeManager.SwipedUp -= OnSwipedUp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            swipeManager.SwipedUp += OnSwipedUp;
            StartCoroutine(SwipeUp());
        }
    }

    private void OnSwipedUp()
    {
        swipedUp = true;
    }

    private IEnumerator SwipeUp()
    {
        swipeUpWidget.SetActive(true);
        swipeManager.disableAllControls = true;
        swipeManager.disableSwipeUp = false;
        Time.timeScale = 0.0f;
        
        while (!swipedUp)
        {
            yield return null;
        }
        swipedUp = false;
        
        Time.timeScale = 1.0f;
        swipeManager.disableAllControls = true;
        swipeUpWidget.SetActive(false);
    }
}
