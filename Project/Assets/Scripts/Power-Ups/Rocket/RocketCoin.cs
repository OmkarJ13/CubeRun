using System.Collections.Generic;
using UnityEngine;

public class RocketCoin : MonoBehaviour
{
    private Player player;
    private readonly List<Transform> children = new List<Transform>();

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        for (int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i));
        }
    }

    private void OnEnable()
    {
        player.rocket.RocketEnded += OnRocketEnded;
        children.ForEach(x => x.gameObject.SetActive(player.rocket.isActiveAndEnabled));
    }

    private void OnRocketEnded()
    {
        children.ForEach(x => x.gameObject.SetActive(false));
    }

    private void OnDisable()
    {
        player.rocket.RocketEnded -= OnRocketEnded;
    }
}
