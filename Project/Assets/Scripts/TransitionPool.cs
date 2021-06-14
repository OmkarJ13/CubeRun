using System.Collections.Generic;
using UnityEngine;

public class TransitionPool : MonoBehaviour
{
    [SerializeField] private TransitionTile transitionTilePrefab;
    private readonly List<TransitionTile> _pool = new List<TransitionTile>();

    public TransitionTile GetTransition()
    {
        TransitionTile transition = _pool.Find(x => x && !x.gameObject.activeSelf);
        if (!transition)
        {
            transition = Instantiate(transitionTilePrefab);
            transition.gameObject.SetActive(false);
            _pool.Add(transition);
        }

        return transition;
    }
}
