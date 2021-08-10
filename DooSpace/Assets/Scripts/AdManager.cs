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
            Debug.Log("Coucou");
            reviveAd = new Ad("ca-app-pub-3940256099942544/5224354917", OnUserEarnedReviveReward);
            doubleCoinsAd = new Ad("ca-app-pub-3940256099942544/5224354917", OnUserEarnedDoubleCoinsReward);
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
		{
            Destroy(gameObject);
		}
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start " + gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(gameObject.name);
    }

    private void OnUserEarnedReviveReward(object sender, Reward args)
    {
        onUserEarnedReviveReward.Invoke();
    }
    private void OnUserEarnedDoubleCoinsReward(object sender, Reward args)
    {
        Debug.Log("User earned reward " + args.Type + " ---- " + args.Amount);
        onUserEarnedDoubleCoinsReward.Invoke();
        GameManager.instance.SetGameState(GameManager.GameState.SCORE);
    }

    public void UserChoseToWatchAd(Ad ad)
    {
        if (ad.rewardedAd.IsLoaded())
        {
            Debug.Log("pop up unactive");
            ad.rewardedAd.Show();
            if (ad == doubleCoinsAd)
            {
                onUserEarnedDoubleCoinsReward.RemoveAllListeners();
                onUserEarnedDoubleCoinsReward.AddListener(() =>
                {
                    GameManager.instance.SetDoubleCoinReward();
                    Debug.Log("User earned reward");
                });
            }
            else if (ad == reviveAd)
            {
                onUserEarnedReviveReward.RemoveAllListeners();
                onUserEarnedReviveReward.AddListener(() =>
                {
                    GameManager.instance.SetReviveReward(true);
                    GameManager.instance.SetGameState(GameManager.GameState.REVIVE);
                    GameManager.instance.DeleteAllMeteorite();
                    Debug.Log("User earned revive");
                });
            }
        }
        else
            GameManager.instance.SetGameState(GameManager.GameState.SCORE);
    }

    public void OpenScoreScreen()
    {
        GameManager.instance.SetGameState(GameManager.GameState.SCORE);
    }

    /*public void WatchDoubleCoinAd()
    {
        UserChoseToWatchAd(doubleCoinsAd);
    }
    public void WatchReviveAd()
    {
        UserChoseToWatchAd(reviveAd);
    }*/
}
