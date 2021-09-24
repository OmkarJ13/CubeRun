using System.Collections;
using UnityEngine;

public class SwipeLeftTrigger : MonoBehaviour
{
    [SerializeField] private GameObject swipeLeftWidget;
    private SwipeManager swipeManager;

    private bool swipedLeft;

    private void Awake()
    { ;
        swipeManager = GameObject.FindGameObjectWithTag("SwipeManager").GetComponent<SwipeManager>();
    }

    private void OnDisable()
    {
        swipeManager.SwipedLeft -= OnSwipedLeft;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            swipeManager.SwipedLeft += OnSwipedLeft;
            StartCoroutine(SwipeLeft());
        }
    }

    private void OnSwipedLeft()
    {
        swipedLeft = true;
    }

    private IEnumerator SwipeLeft()
    {
        swipeLeftWidget.SetActive(true);
        swipeManager.disableAllControls = true;
        swipeManager.disableSwipeLeft = false;
        Time.timeScale = 0.0f;
        
        while (!swipedLeft)
        {
            yield return null;
        }
        swipedLeft = false;
        
        Time.timeScale = 1.0f;
        swipeManager.disableAllControls = true;
        swipeLeftWidget.SetActive(false);
    }
}
