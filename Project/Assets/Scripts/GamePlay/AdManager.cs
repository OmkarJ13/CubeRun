using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsListener
{
    private Player player;
    
    private void Awake()
    {
        Advertisement.Initialize("4375779");
        Advertisement.AddListener(this);
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void PlayRewarded()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show("Rewarded_Android");
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
    }

    public void OnUnityAdsDidError(string message)
    {

    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId == "Rewarded_Android" && showResult == ShowResult.Finished)
        {
            // Reward Player
            if (player)
            {
                player.RevivePlayer();
            }
        }
    }
}
