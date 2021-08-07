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

public class AdsManager : MonoBehaviour
{
    //[SerializeField] private GameObject popUp;

    public Ad reviveAd = null;
    public Ad doubleCoinsAd = null;

    public UnityEvent onUserEarnedReviveReward = new UnityEvent();
    public UnityEvent onUserEarnedDoubleCoinsReward = new UnityEvent();

    public static AdsManager instance = null;

    private void Awake()
	{
        if (instance == null)
        {
            reviveAd = new Ad("ca-app-pub-3940256099942544/5224354917", OnUserEarnedReviveReward);
            doubleCoinsAd = new Ad("ca-app-pub-3940256099942544/5224354917", OnUserEarnedDoubleCoinsReward);
            instance = this;
            //popUp.SetActive(false);
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnUserEarnedReviveReward(object sender, Reward args)
    {
        onUserEarnedReviveReward.Invoke();
    }
    private void OnUserEarnedDoubleCoinsReward(object sender, Reward args)
    {
        Debug.Log("User earned reward " + args.Type + " ---- " + args.Amount);
        onUserEarnedDoubleCoinsReward.Invoke();
    }

    public void UserChoseToWatchAd(Ad ad)
    {
        if (ad.rewardedAd.IsLoaded())
        {
            ad.rewardedAd.Show();
        }
    }

    public void OpenPopUp()
    {      
        //popUp.SetActive(true);
    }

    public void WatchDoubleCoinAd()
	{
        UserChoseToWatchAd(doubleCoinsAd);
    }
}
