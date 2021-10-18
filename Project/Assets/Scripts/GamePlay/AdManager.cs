using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour
{
    [SerializeField] private ModalWindow modalWindow;
    
    private RewardBasedVideoAd rewardBasedVideo;
    private Player player;
    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        
        MobileAds.Initialize(status => {});
        rewardBasedVideo = RewardBasedVideoAd.Instance;

        rewardBasedVideo.OnAdRewarded += OnAdRewarded;
        rewardBasedVideo.OnAdClosed += OnAdClosed;

        RequestRewardVideo();
    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

    private void RequestRewardVideo()
    {
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";
        rewardBasedVideo.LoadAd(CreateAdRequest(), adUnitId);
    }

    public void ShowRewardBasedVideo()
    {
        if (rewardBasedVideo.IsLoaded())
        {
            rewardBasedVideo.Show();
        }
        else
        {
            modalWindow.gameObject.SetActive(true);
        }
    }

    private void OnAdRewarded(object sender, Reward reward)
    {
        if (player)
        {
            player.RevivePlayer();
        }
    }

    private void OnAdClosed(object sender, EventArgs args)
    {
        RequestRewardVideo();
    }
}
