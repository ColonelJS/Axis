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
    [SerializeField] private RectTransform reviveCostRect;
    [SerializeField] private Text txtCurrentMoney;
    [SerializeField] private GameObject errorMoney;
    [SerializeField] private GameObject goImgReviveCostAxius;

    int reviveCost = 2500;
    int reviveIndex = 0;

    void Start()
    {
        reviveIndex = 0;
        txtCurrentMoney.text = CustomScreen.instance.GetPlayerMoney().ToString();
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
    }

    public void OpenPopUp()
    {
        txtCurrentMoney.text = CustomScreen.instance.GetPlayerMoney().ToString();
        reviveCost = Mathf.CeilToInt(CharacterManager.instance.GetScore() / 4);
        if (reviveIndex == 1)
        {
            txtReviveCost.text = reviveCost.ToString();
            goImgReviveCostAxius.SetActive(true);
            reviveCostRect.Translate(new Vector3(-32, 0, 0), Space.Self);
        }
        else if (reviveIndex == 2)
        {
            buttonRevive.interactable = false;
        }
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
            ClosePopUp();
        });
        AdManager.instance.UserChoseToWatchAd(AdManager.instance.doubleCoinsAd);
    }
    public void WatchReviveAd()
    {
        AdManager.instance.onUserEarnedReviveReward.RemoveAllListeners();
        AdManager.instance.onUserEarnedReviveReward.AddListener(() =>
        {
            GameManager.instance.SetReviveReward(true);
            GameManager.instance.SetGameState(GameManager.GameState.REVIVE);
            GameManager.instance.DeleteAllMeteorite();

            ClosePopUp();

            reviveIndex++;
        });
        if (reviveIndex >= 1)
        {
            if (CustomScreen.instance.GetPlayerMoney() >= reviveCost)
            {
                AdManager.instance.UserChoseToWatchAd(AdManager.instance.reviveAd);
                RemoveMoney();
            }
            else
                errorMoney.GetComponent<AutoFade>().StartFade();
        }
        else
        {
            AdManager.instance.UserChoseToWatchAd(AdManager.instance.reviveAd);
        }
    }

    void RemoveMoney()
	{
        int currentMoney = CustomScreen.instance.GetPlayerMoney();
        currentMoney -= reviveCost;
        //PlayerPrefs.SetInt("money", currentMoney);
        CustomScreen.instance.SetNewMoney(currentMoney);
    }

    public void OpenScoreScreen()
    {
        GameManager.instance.SetGameState(GameManager.GameState.SCORE);
        ClosePopUp();
    }
}
