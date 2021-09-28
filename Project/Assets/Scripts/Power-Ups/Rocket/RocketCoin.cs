using System.Collections.Generic;
using UnityEngine;

public class RocketCoin : MonoBehaviour
{
    private Player player;
    private readonly List<Transform> children = new List<Transform>();

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        
        for (int i = 0; i < transform.childCount; i++) {
            children.Add(transform.GetChild(i));
        }
    }

    private void OnEnable()
    {
        if (player.rocket && player.rocket.isActiveAndEnabled)
        {
            player.rocket.RocketLanding += OnRocketEnded;
            children.ForEach(x => x.gameObject.SetActive(true));
        }
        else
        {
            children.ForEach(x => x.gameObject.SetActive(false));
        }
    }

    private void OnRocketEnded()
    {
        children.ForEach(x => x.gameObject.SetActive(false));
    }

    private void OnDisable()
    {
        player.rocket.RocketLanding -= OnRocketEnded;
    }
}
