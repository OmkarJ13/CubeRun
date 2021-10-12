using System.Collections;
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

        Vector3 spawnPos = Vector3.forward * (player.transform.position.z + 50.0f);
        levelGenerator.SetupObstacles(spawnPos);
    }
}
