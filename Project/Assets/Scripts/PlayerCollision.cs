using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private float restartDelay = 1.0f;
    
    private PlayerMovement movement;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            movement.enabled = false;
            
            LevelManager.Instance.enabled = false;
            ScoreManager.Instance.enabled = false;

            StartCoroutine(Restart());
        }
    }

    private IEnumerator Restart()
    {
        yield return new WaitForSeconds(restartDelay);
        GameManager.Instance.ReloadLevel();
    }
}
