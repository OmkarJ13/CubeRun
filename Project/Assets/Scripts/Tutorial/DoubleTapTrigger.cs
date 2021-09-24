using System.Collections;
using UnityEngine;

public class DoubleTapTrigger : MonoBehaviour
{
    private GameObject doubleTapWidget;
    private SwipeManager swipeManager;

    private bool doubleTapped;

    private void Awake()
    {
        doubleTapWidget = GameObject.FindGameObjectWithTag("DoubleTapWidget");   
        swipeManager = GameObject.FindGameObjectWithTag("SwipeManager").GetComponent<SwipeManager>();
    }

    private void OnDisable()
    {
        swipeManager.SwipedRight -= OnDoubleTapped;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(DoubleTap());
            swipeManager.DoubleTapped += OnDoubleTapped;
        }
    }

    private void OnDoubleTapped()
    {
        doubleTapped = true;
    }

    private IEnumerator DoubleTap()
    {
        doubleTapWidget.SetActive(true);
        swipeManager.disableAllControls = true;
        swipeManager.disableDoubleTap = false;
        Time.timeScale = 0.0f;
        
        while (!doubleTapped)
        {
            yield return null;
        }
        doubleTapped = false;
        
        swipeManager.disableAllControls = false;
        doubleTapWidget.SetActive(false);
    }
}
