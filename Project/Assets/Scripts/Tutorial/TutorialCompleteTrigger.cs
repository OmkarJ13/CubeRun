using System.Collections;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class TutorialCompleteTrigger : MonoBehaviour
{
    [SerializeField] private GameObject tutorialCompleteWidget;
    
    private LevelGenerator levelGenerator;
    private Player player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        levelGenerator = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();
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

        Vector3 spawnPos = player.transform.position + new Vector3(0.0f, 0.5f, 1.0f) * 50.0f;
        levelGenerator.SetupObstacles(spawnPos);
    }
}
