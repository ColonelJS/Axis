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
    [SerializeField] private RectTransform imgArrowRevive;
    public AnimationCurve curve;

    int reviveCost = 2500;
    int reviveIndex = 0;
    bool animXAxis = true;
    bool reviveAdSetup = false;
    bool needToClosePopUp = false;

    void Start()
    {
        txtCurrentMoney.text = CustomScreen.instance.GetPlayerMoney().ToString();
        ClosePopUp();
    }

    void Update()
    {
        if (AdManager.instance.GetIsAdReviveLoaded())
        {
            if (!reviveAdSetup)
                SetupReviveAd();
        }

        if (needToClosePopUp)
            ClosePopUp();

        if (AdManager.instance.GetIsAdMoneyLoaded())
            buttonMoney.interactable = true;
        else
            buttonMoney.interactable = false;
    }

    void SetupReviveAd()
    {
        buttonRevive.interactable = true;
        if (reviveIndex != 0)
        {
            reviveCost = Mathf.CeilToInt(CharacterManager.instance.GetScore() / 4);
            txtReviveCost.text = reviveCost.ToString();
            goImgReviveCostAxius.SetActive(true);
        }
        else
            txtReviveCost.text = "Free";

        reviveAdSetup = true;
    }

    void UpdateArrowLoadingAd()
    {
        if (animXAxis)
        {
            if (imgArrowRevive.localRotation.eulerAngles.x < 0)
            {
                imgArrowRevive.localRotation.eulerAngles.Set(0, 0, 0);
                animXAxis = !animXAxis;
            }
            else
                imgArrowRevive.Rotate(new Vector3(200 * Time.deltaTime, 0, 0));
        }
        else
        {
            if (imgArrowRevive.localRotation.eulerAngles.y < 0)
            {
                imgArrowRevive.localRotation.eulerAngles.Set(0, 0, 0);
                animXAxis = !animXAxis;
            }
            else
                imgArrowRevive.Rotate(new Vector3(0, 200 * Time.deltaTime, 0));
        }
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
            if (PlayerPrefs.GetString("language") == "fr")
                txtReviveCost.text = "Indisponible";
            else
                txtReviveCost.text = "Unavailable";
            goImgReviveCostAxius.SetActive(false);
        }

        if (!AdManager.instance.GetIsAdReviveLoaded())
        {
            buttonRevive.interactable = false;
            if (PlayerPrefs.GetString("language") == "fr")
                txtReviveCost.text = "Indisponible";
            else
                txtReviveCost.text = "Unavailable";
            goImgReviveCostAxius.SetActive(false);
        }

        reviveAdSetup = false;
        popUp.SetActive(true);
    }

    public void ClosePopUp()
    {
        popUp.SetActive(false);
        needToClosePopUp = false;
    }

    public void WatchDoubleCoinAd()
    {
        AdManager.instance.onUserEarnedDoubleCoinsReward.RemoveAllListeners();
        AdManager.instance.onUserEarnedDoubleCoinsReward.AddListener(() =>
        {
            GameManager.instance.SetDoubleCoinReward();
            needToClosePopUp = true;
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
            needToClosePopUp = true;

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
        CustomScreen.instance.SetNewMoney(currentMoney);
    }

    public void OpenScoreScreen()
    {
        GameManager.instance.SetGameState(GameManager.GameState.SCORE);
        ClosePopUp();
    }
}
