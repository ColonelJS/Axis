using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdsPopUp : MonoBehaviour
{
    [SerializeField] private GameObject popUp;
    [SerializeField] private Button buttonRevive;
    [SerializeField] private Button buttonMoney;
    [SerializeField] private Text txtReviveCost;
    [SerializeField] private GameObject errorMoney;

    int reviveCost = 2500;
    int reviveIndex = 0;

    void Start()
    {
        reviveIndex = 0;
        ClosePopUp();
    }

    void Update()
    {
        if (AdManager.instance.GetIsAdReviveLoaded())
            buttonRevive.interactable = true;
        else
            buttonRevive.interactable = false;

        if (AdManager.instance.GetIsAdMoneyLoaded())
            buttonMoney.interactable = true;
        else
            buttonMoney.interactable = false;

        if (reviveIndex == 1)
		{
            txtReviveCost.text = "("+ reviveCost.ToString()+"$)";
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
            GameManager.instance.SetDoubleCoinReward();
        });
        AdManager.instance.UserChoseToWatchAd(AdManager.instance.doubleCoinsAd);
        ClosePopUp();
    }
    public void WatchReviveAd()
    {
        AdManager.instance.onUserEarnedReviveReward.RemoveAllListeners();
        AdManager.instance.onUserEarnedReviveReward.AddListener(() =>
        {
            GameManager.instance.SetReviveReward(true);
            GameManager.instance.SetGameState(GameManager.GameState.REVIVE);
            GameManager.instance.DeleteAllMeteorite();

            reviveIndex++;
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
