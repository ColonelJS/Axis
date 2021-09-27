using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.Events;
public class Ad
{
    public Ad(string _adId, EventHandler<Reward> _onUserEarnedRewardEvent)
    {
        adId = _adId;
        onUserEarnedReward = _onUserEarnedRewardEvent;
        CreateAndLoadRewardedAd();
    }
    public RewardedAd rewardedAd = null;
    private EventHandler<Reward> onUserEarnedReward;
    private string adId;
    private void CreateAndLoadRewardedAd()
    {
        rewardedAd = new RewardedAd(adId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        rewardedAd.LoadAd(request);
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        rewardedAd.OnUserEarnedReward += onUserEarnedReward;
    }
    private void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        CreateAndLoadRewardedAd();
    }
}

public class AdManager : MonoBehaviour
{
    public Ad reviveAd = null;
    public Ad doubleCoinsAd = null;

    public UnityEvent onUserEarnedReviveReward = new UnityEvent();
    public UnityEvent onUserEarnedDoubleCoinsReward = new UnityEvent();

    public static AdManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            //ga : ca-app-pub-8647762808123656/2122069293
            reviveAd = new Ad("ca-app-pub-4315812392007026/5627715407", OnUserEarnedReviveReward);
            doubleCoinsAd = new Ad("ca-app-pub-4315812392007026/5627715407", OnUserEarnedDoubleCoinsReward);
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
		{
            Destroy(gameObject);
		}
    }

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnUserEarnedReviveReward(object sender, Reward args)
    {
        onUserEarnedReviveReward.Invoke();
    }
    private void OnUserEarnedDoubleCoinsReward(object sender, Reward args)
    {
        onUserEarnedDoubleCoinsReward.Invoke();
        GameManager.instance.SetGameState(GameManager.GameState.SCORE);
    }

    public void UserChoseToWatchAd(Ad ad)
    {
        if (ad.rewardedAd.IsLoaded())
            ad.rewardedAd.Show();
        else
            GameManager.instance.SetGameState(GameManager.GameState.SCORE);
    }

    public bool GetIsAdReviveLoaded()
	{
        if (reviveAd.rewardedAd.IsLoaded())
            return true;
        else
            return false;
	}

    public bool GetIsAdMoneyLoaded()
    {
        if (doubleCoinsAd.rewardedAd.IsLoaded())
            return true;
        else
            return false;
    }
}
