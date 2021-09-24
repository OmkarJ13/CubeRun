using System.Collections;
using UnityEngine;

public class SwipeDownTrigger : MonoBehaviour
{
    [SerializeField] private GameObject swipeDownWidget;
    private SwipeManager swipeManager;

    private bool swipedDown;
    
    private void Awake()
    {
        swipeManager = GameObject.FindGameObjectWithTag("SwipeManager").GetComponent<SwipeManager>();
    }

    private void OnDisable()
    {
        swipeManager.SwipedDown -= OnSwipedDown;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            swipeManager.SwipedDown += OnSwipedDown;
            StartCoroutine(SwipeDown());
        }
    }

    private void OnSwipedDown()
    {
        swipedDown = true;
    }

    private IEnumerator SwipeDown()
    {
        swipeDownWidget.SetActive(true);
        swipeManager.disableAllControls = true;
        swipeManager.disableSwipeDown = false;
        Time.timeScale = 0.0f;
        
        while (!swipedDown)
        {
            yield return null;
        }
        swipedDown = false;
        
        Time.timeScale = 1.0f;
        swipeManager.disableAllControls = true;
        swipeDownWidget.SetActive(false);
    }
}
