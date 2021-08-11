using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdsPopUp : MonoBehaviour
{
    [SerializeField] private GameObject popUp;
    [SerializeField] private Button buttonRevive;
    [SerializeField] private Text txtReviveCost;
    [SerializeField] private GameObject errorMoney;

    int reviveCost = 1000;
    int reviveIndex = 0;

    void Start()
    {
        reviveIndex = 0;
        ClosePopUp();
    }

    void Update()
    {
        if(reviveIndex == 1)
		{
            txtReviveCost.text = "(1000$)";
		}
        else if(reviveIndex == 2)
		{
            buttonRevive.interactable = false;
		}
    }

    public void OpenPopUp()
    {
        popUp.SetActive(true);
    }

    public void ClosePopUp()
    {
        popUp.SetActive(false);
    }

    public void WatchDoubleCoinAd()
    {
        AdManager.instance.onUserEarnedDoubleCoinsReward.RemoveAllListeners();
        AdManager.instance.onUserEarnedDoubleCoinsReward.AddListener(() =>
        {
            ClosePopUp();
            GameManager.instance.SetDoubleCoinReward();
            Debug.Log("User earned reward");
        });
        AdManager.instance.UserChoseToWatchAd(AdManager.instance.doubleCoinsAd);
        ClosePopUp();
    }
    public void WatchReviveAd()
    {
        AdManager.instance.onUserEarnedReviveReward.RemoveAllListeners();
        AdManager.instance.onUserEarnedReviveReward.AddListener(() =>
        {
            ClosePopUp();
            GameManager.instance.SetReviveReward(true);
            GameManager.instance.SetGameState(GameManager.GameState.REVIVE);
            GameManager.instance.DeleteAllMeteorite();

            reviveIndex++;
            Debug.Log("User earned revive");
        });
        if (reviveIndex >= 1)
        {
            if (PlayerPrefs.GetInt("money") >= reviveCost)
            {
                AdManager.instance.UserChoseToWatchAd(AdManager.instance.reviveAd);
                RemoveMoney();
                ClosePopUp();
            }
            else
                errorMoney.GetComponent<AutoFade>().StartFade();
        }
        else
        {
            AdManager.instance.UserChoseToWatchAd(AdManager.instance.reviveAd);
            ClosePopUp();
        }
    }

    void RemoveMoney()
	{
        int currentMoney = PlayerPrefs.GetInt("money");
        currentMoney -= reviveCost;
        PlayerPrefs.SetInt("money", currentMoney);
    }

    public void OpenScoreScreen()
    {
        GameManager.instance.SetGameState(GameManager.GameState.SCORE);
        ClosePopUp();
    }
}
