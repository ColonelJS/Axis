using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.Events;
/*public class Ad
{
    private static bool adLoaded = false;
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
        rewardedAd.OnAdFullScreenContentClosed += HandleRewardedAdClosed
        rewardedAd = RewardedAd.Load(adId, onUserEarnedReward);
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
}*/

public enum AdType
{
    REVIVE = 0,
    DOUBLE_COIN
};

public class Ad
{
    public RewardedAd rewardedAd = null;
    private string _adId;
    private AdType _adType;



    public Ad(string adId, AdType adType)
    {
        _adId = adId;
        _adType = adType;
    }

    public string AdId => _adId;
    public AdType AdType => _adType;
}

public class AdManager : MonoBehaviour
{
    public Ad reviveAd = null;
    public Ad doubleCoinsAd = null;

    public UnityEvent onUserEarnedReviveReward = new UnityEvent();
    public UnityEvent onUserEarnedDoubleCoinsReward = new UnityEvent();

    [SerializeField] private AdsPopUp adsPopUp;

    public static AdManager instance = null;

    private void ReloadAdHandler(Ad ad)
    {
        ad.rewardedAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded Ad full screen content closed.");

            LoadRewardedAd(ad);
        };

        ad.rewardedAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " + "with error : " + error);

            LoadRewardedAd(ad);
        };
    }

    public void LoadRewardedAd(Ad ad)
    {
        // Clean up the old ad before loading a new one.
        if (ad.rewardedAd != null)
        {
            ad.rewardedAd.Destroy();
            ad.rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        RewardedAd.Load(ad.AdId, adRequest, (RewardedAd newRewardedAd, LoadAdError error) =>
        {
            // if error is not null, the load request failed.
            if (error != null || newRewardedAd == null)
            {
                Debug.LogError("Rewarded ad failed to load an ad with error : " + error);
                return;
            }

            Debug.Log("Rewarded ad loaded with response : " + newRewardedAd.GetResponseInfo());
            ad.rewardedAd = newRewardedAd;
            ReloadAdHandler(ad);
        });
    }

    private void Awake()
    {
        if (instance == null)
        {
            //ga : ca-app-pub-8647762808123656/2122069293
            //reviveAd = new Ad("ca-app-pub-8647762808123656/1144199621", OnUserEarnedReviveReward);
            //doubleCoinsAd = new Ad("ca-app-pub-8647762808123656/8447974573", OnUserEarnedDoubleCoinsReward);

            MobileAds.Initialize((InitializationStatus initStatus) =>
            {
                reviveAd = new Ad("ca-app-pub-8647762808123656/1144199621", AdType.REVIVE);
                doubleCoinsAd = new Ad("ca-app-pub-8647762808123656/8447974573", AdType.DOUBLE_COIN);                
                LoadRewardedAd(reviveAd);
                LoadRewardedAd(doubleCoinsAd);
            });

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
        onUserEarnedReviveReward.AddListener(() =>
        {
            GameManager.instance.SetReviveReward(true);
            GameManager.instance.SetGameState(GameManager.GameState.REVIVE);
            GameManager.instance.DeleteAllMeteorite();

            adsPopUp.ClosePopUp();
            adsPopUp.IncrementReviveIndex();
        });

        onUserEarnedDoubleCoinsReward.AddListener(() =>
        {
            GameManager.instance.SetDoubleCoinReward();
            adsPopUp.ClosePopUp();
        });
    }

    /*private void OnUserEarnedReviveReward(object sender, Reward args)
    {
        onUserEarnedReviveReward.Invoke();
    }
    private void OnUserEarnedDoubleCoinsReward(object sender, Reward args)
    {
        onUserEarnedDoubleCoinsReward.Invoke();
        GameManager.instance.SetGameState(GameManager.GameState.SCORE);
    }*/

    /*public void UserChoseToWatchAd(Ad ad)
    {
        if (ad.rewardedAd.IsLoaded())
            ad.rewardedAd.Show();
        else
            GameManager.instance.SetGameState(GameManager.GameState.SCORE);
    }*/

    public void ShowRewardedAd(Ad ad)
    {
        const string rewardMsg = "rewarded the user. Type: {0}, amount: {1}.";

        if (ad.rewardedAd != null && ad.rewardedAd.CanShowAd())
        {
            ad.rewardedAd.Show((Reward reward) =>
            {
                Debug.Log(string.Format(rewardMsg, reward.Type, reward.Amount));
                switch (ad.AdType)
                {
                    case AdType.REVIVE:
                        onUserEarnedReviveReward.Invoke();
                        break;
                    case AdType.DOUBLE_COIN:
                        onUserEarnedDoubleCoinsReward.Invoke();
                        break;
                    default:
                        break;
                }
            });
        }
        else
            GameManager.instance.SetGameState(GameManager.GameState.SCORE);
    }

    public bool GetIsAdReviveLoaded()
	{
        if(reviveAd.rewardedAd != null)
            return reviveAd.rewardedAd.CanShowAd();
        else
            return false;
	}

    public bool GetIsAdMoneyLoaded()
    {
        if (doubleCoinsAd.rewardedAd != null)
            return doubleCoinsAd.rewardedAd.CanShowAd();
        else
            return false;
    }
}
