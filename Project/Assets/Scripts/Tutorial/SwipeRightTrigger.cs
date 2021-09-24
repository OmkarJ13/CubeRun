using System.Collections;
using UnityEngine;

public class SwipeRightTrigger : MonoBehaviour
{
    [SerializeField] private GameObject swipeRightWidget;
    private SwipeManager swipeManager;

    private bool swipedRight;

    private void Awake()
    {
        swipeManager = GameObject.FindGameObjectWithTag("SwipeManager").GetComponent<SwipeManager>();
    }

    private void OnDisable()
    {
        swipeManager.SwipedRight -= OnSwipedRight;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SwipeRight());
            swipeManager.SwipedRight += OnSwipedRight;
        }
    }

    private void OnSwipedRight()
    {
        swipedRight = true;
    }

    private IEnumerator SwipeRight()
    {
        swipeRightWidget.SetActive(true);
        swipeManager.disableAllControls = true;
        swipeManager.disableSwipeRight = false;
        Time.timeScale = 0.0f;
        
        while (!swipedRight)
        {
            yield return null;
        }
        swipedRight = false;
        
        Time.timeScale = 1.0f;
        swipeManager.disableAllControls = true;
        swipeRightWidget.SetActive(false);

    }
}
