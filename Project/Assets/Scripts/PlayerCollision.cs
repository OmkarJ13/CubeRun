using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private float restartDelay = 1.0f;
    
    private PlayerMovement movement;
    private GameManager gameManager;
    private LevelManager levelManager;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        
        gameManager = FindObjectOfType<GameManager>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            movement.enabled = false;
            levelManager.enabled = false;
            
            StartCoroutine(Restart());
        }
    }

    private IEnumerator Restart()
    {
        yield return new WaitForSeconds(restartDelay);
        gameManager.ReloadLevel();
    }
}
