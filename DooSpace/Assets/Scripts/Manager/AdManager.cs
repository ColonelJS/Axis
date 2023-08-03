using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.Events;

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
    [SerializeField] private AdsPopUp adsPopUp;

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
            GameManager.instance.SetGameState(GameManager.GameState.SCORE);
            adsPopUp.ClosePopUp();
        });
    }

    public void LoadRewardedAd(Ad ad)
    {
        if (ad.rewardedAd != null)
        {
            ad.rewardedAd.Destroy();
            ad.rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        var adRequest = new AdRequest();

        RewardedAd.Load(ad.AdId, adRequest, (RewardedAd newRewardedAd, LoadAdError error) =>
        {
            if (error != null || newRewardedAd == null)
            {
                Debug.LogError("Rewarded ad failed to load an ad with error : " + error);
                return;
            }

            ad.rewardedAd = newRewardedAd;
            ReloadAdHandler(ad);
        });
    }

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

    public void ShowRewardedAd(Ad ad)
    {
        if (ad.rewardedAd != null && ad.rewardedAd.CanShowAd())
        {
            ad.rewardedAd.Show((Reward reward) =>
            {
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
