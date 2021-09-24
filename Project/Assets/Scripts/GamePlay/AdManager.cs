using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsListener
{
    private GameManager gameManager;
    
    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        
        Advertisement.Initialize("4375779");
        Advertisement.AddListener(this);
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
        print("Ads Ready");
    }

    public void OnUnityAdsDidError(string message)
    {
        print("Ads Error : " + message);
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        print("Ad Started");
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId == "Rewarded_Android" && showResult == ShowResult.Finished)
        {
            // Reward Player
            gameManager.RevivePlayer();
        }
    }
}