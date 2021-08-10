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

    void Start()
    {
        ClosePopUp();
    }

    void Update()
    {
        if(AdManager.instance.GetReviveIndex() == 1)
		{
            txtReviveCost.text = "(1000$)";
		}
        else if(AdManager.instance.GetReviveIndex() == 2)
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
        AdManager.instance.UserChoseToWatchAd(AdManager.instance.doubleCoinsAd);
    }
    public void WatchReviveAd()
    {
        if (AdManager.instance.GetReviveIndex() >= 1)
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
}
