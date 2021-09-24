using System.Collections;
using UnityEngine;

public class TutorialCompleteTrigger : MonoBehaviour
{
    private GameObject tutorialCompleteWidget;
    private Player player;
    private GameManager gameManager;

    private void Awake()
    {
        tutorialCompleteWidget = GameObject.FindGameObjectWithTag("TutorialCompleteWidget");
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(CheckForPowerUps());
        }
    }

    private IEnumerator CheckForPowerUps()
    {
        while (player.hasAnyPowerUp)
        {
            yield return null;
        }
        
        tutorialCompleteWidget.SetActive(true);
        gameManager.StartGame();
    }
}
