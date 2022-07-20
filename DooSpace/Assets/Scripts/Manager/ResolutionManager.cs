using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{
    public static ResolutionManager instance;

    [Header("Main screen")]
    [SerializeField] GameObject bgUp;
    [SerializeField] RectTransform bgUpRectTransform;
    [SerializeField] RectTransform ribbonRect;
    [SerializeField] RectTransform highscoreImg;
    [SerializeField] RectTransform customImg;
    [SerializeField] Transform transformPointLogo;
    [Space(8)]
    [Header("Settings")]
    [SerializeField] GameObject bgInfo;
    [SerializeField] RectTransform bgInfoRectTransform;
    [SerializeField] RectTransform bgInfoScrollViewRectTransform;
    [Space(8)]
    [Header("Screens rect")]
    [SerializeField] RectTransform mainScreenRect;
    [SerializeField] RectTransform settingsRect;
    [SerializeField] GameObject bgUpElements;
    [SerializeField] GameObject buttonStart;
    [SerializeField] GameObject settingsElements;
    [SerializeField] RectTransform highscoreRect;
    [SerializeField] GameObject highscoreElements;
    [SerializeField] RectTransform highscoreElementsRect;
    [SerializeField] RectTransform loginToGPRect;
    [SerializeField] RectTransform globalScoreRect;
    [SerializeField] RectTransform globalScoreSelfRect;
    [SerializeField] RectTransform globalScoreLoadingSelfRect;
    [SerializeField] RectTransform globalScoreLoadingRect;

    [SerializeField] GameObject newVersionElements;
    [SerializeField] GameObject connexionGPGSElements;
    [SerializeField] RectTransform FirebaseLoadingRect;
    [SerializeField] GameObject FirebaseLoadingElements;
    [SerializeField] GameObject menuPauseElements;

    [SerializeField] RectTransform customRect;
    [SerializeField] GameObject customElements;
    [SerializeField] RectTransform seasonPassRectBase;
    [SerializeField] RectTransform seasonPassRectMiddle;
    [SerializeField] RectTransform seasonPassRectUp;
    [SerializeField] RectTransform seasonPassRectDown;
    [SerializeField] RectTransform spaceRect;
    [SerializeField] RectTransform skyRect;
    [SerializeField] RectTransform earthRect;
    [SerializeField] GameObject skyElements;
    [SerializeField] GameObject characterElements;
    [SerializeField] GameObject secondCharacterElements;
    [SerializeField] Transform characterSpawnPoint;
    [SerializeField] GameObject adsPopUpElements;

    [SerializeField] Transform fuelSpawnPoint;
    [SerializeField] Transform pauseSpawnPoint;
    [SerializeField] GameObject fuelElements;
    [SerializeField] GameObject buttonPause;

    [SerializeField] Transform moneySpawnPoint;
    [SerializeField] GameObject moneyGo;

    [SerializeField] GameObject resultsElements;
    [SerializeField] GameObject chestElements;
    [SerializeField] RectTransform chestRect;
    [SerializeField] GameObject chestScreenElements;
    [SerializeField] RectTransform scoreScreenBgRect;

    [Space(8)]
    [Header("Canvas scaler")]
    [SerializeField] CanvasScaler menusCanvasScaler;
    [SerializeField] CanvasScaler backgroundCanvasScaler;
    [SerializeField] CanvasScaler playerCanvasScaler;
    [SerializeField] CanvasScaler resultsCanvasScaler;
    [SerializeField] CanvasScaler hudCanvasScaler;

    private void Awake()
	{
        if (instance == null)
            instance = this;
	}

	void Start()
    {
        SetupCanvasScaler();
        SetupScreenRect();
        SetupMainMenu();
    }

    void Update()
    {
        fuelElements.transform.position = new Vector3(fuelSpawnPoint.position.x, fuelElements.transform.position.y, fuelElements.transform.position.z);
        buttonPause.transform.position = new Vector3(pauseSpawnPoint.position.x, buttonPause.transform.position.y, buttonPause.transform.position.z);

        highscoreImg.position = new Vector3(highscoreImg.position.x, transformPointLogo.position.y, highscoreImg.position.z);
        customImg.position = new Vector3(customImg.position.x, transformPointLogo.position.y, customImg.position.z);
    }

    Vector2 GetResolution()
	{
        Vector2 res;
        res.x = Screen.width;
        res.y = Screen.height;
        return res;
	}

    void SetupCanvasScaler()
	{
        menusCanvasScaler.referenceResolution = GetResolution();
        backgroundCanvasScaler.referenceResolution = GetResolution();
        playerCanvasScaler.referenceResolution = GetResolution();
        resultsCanvasScaler.referenceResolution = GetResolution();
    }

    void SetupScreenRect()
	{
        mainScreenRect.sizeDelta = new Vector2(Screen.width, Screen.height);
        highscoreRect.sizeDelta = new Vector2(Screen.width, Screen.height);
        customRect.sizeDelta = new Vector2(Screen.width, Screen.height);
        loginToGPRect.sizeDelta = new Vector2(Screen.width, loginToGPRect.sizeDelta.y);

        seasonPassRectMiddle.sizeDelta = new Vector2(seasonPassRectBase.rect.width, seasonPassRectMiddle.sizeDelta.y);
        seasonPassRectUp.sizeDelta = new Vector2(seasonPassRectBase.rect.width, seasonPassRectUp.sizeDelta.y);
        seasonPassRectDown.sizeDelta = new Vector2(seasonPassRectBase.rect.width, seasonPassRectDown.sizeDelta.y);

        //newVersionRect.sizeDelta = new Vector2(Screen.width, Screen.height);
        //connexionDBRect.sizeDelta = new Vector2(Screen.width, Screen.height);
        FirebaseLoadingRect.sizeDelta = new Vector2(Screen.width, Screen.height);
        //menuPauseRect.sizeDelta = new Vector2(Screen.width, Screen.height);

        float newScale = GetResolution().y / 2400;
        bgUpElements.transform.localScale = new Vector3(newScale, newScale, newScale);
        settingsElements.transform.localScale = new Vector3(newScale, newScale, newScale);
        highscoreElements.transform.localScale = new Vector3(newScale, newScale, newScale);
        customElements.transform.localScale = new Vector3(newScale, newScale, newScale);

        //float scaleFactor = highscoreElements.transform.localScale.x;
        globalScoreRect.sizeDelta = new Vector2(Screen.width / newScale, globalScoreRect.sizeDelta.y);
        globalScoreSelfRect.sizeDelta = new Vector2(Screen.width / newScale, globalScoreSelfRect.sizeDelta.y);
        globalScoreLoadingRect.sizeDelta = new Vector2(Screen.width / newScale, globalScoreLoadingRect.sizeDelta.y);
        globalScoreLoadingSelfRect.sizeDelta = new Vector2(Screen.width / newScale, globalScoreLoadingSelfRect.sizeDelta.y);

        newVersionElements.transform.localScale = new Vector3(newScale, newScale, newScale);
        connexionGPGSElements.transform.localScale = new Vector3(newScale, newScale, newScale);
        connexionGPGSElements.transform.localPosition = new Vector3(0, connexionGPGSElements.transform.localPosition.y * newScale, 0);
        FirebaseLoadingElements.transform.localScale = new Vector3(newScale, newScale, newScale);
        menuPauseElements.transform.localScale = new Vector3(newScale, newScale, newScale);

        settingsRect.sizeDelta = new Vector2(settingsRect.sizeDelta.x * newScale, settingsRect.sizeDelta.y * newScale);

        spaceRect.sizeDelta = new Vector2(Screen.width, Screen.height);
        skyRect.sizeDelta = new Vector2(Screen.width, Screen.height);
        skyElements.transform.localScale = new Vector3(newScale, newScale, newScale);
        characterElements.transform.position = characterSpawnPoint.position;
        characterElements.transform.localScale = new Vector3(newScale, newScale, newScale);
        secondCharacterElements.transform.position = characterSpawnPoint.position;
        secondCharacterElements.transform.localScale = new Vector3(newScale, newScale, newScale);
        adsPopUpElements.transform.localScale = new Vector3(newScale, newScale, newScale);

        moneyGo.transform.position = new Vector3(moneySpawnPoint.position.x, moneyGo.transform.position.y, moneyGo.transform.position.z);

        chestRect.sizeDelta = new Vector2(Screen.width, Screen.height) / newScale;
        float chestScale = GetResolution().x / 1080;
        resultsElements.transform.localScale = new Vector3(newScale, newScale, newScale);
        chestElements.transform.localScale = new Vector3(chestScale, chestScale, chestScale);
        chestScreenElements.transform.localScale = new Vector3(newScale, newScale, newScale);

        scoreScreenBgRect.sizeDelta = new Vector2(Screen.width, Screen.height);
        scoreScreenBgRect.localPosition = new Vector3(-Screen.width, scoreScreenBgRect.position.y, scoreScreenBgRect.position.z);
    }

    void SetupMainMenu()
	{
        bgUp.transform.localPosition = new Vector3(0, 0, 0);
        bgUpRectTransform.sizeDelta = GetResolution();

        float newScale = GetResolution().y / 2400;
        ribbonRect.position = new Vector3(ribbonRect.position.x, ribbonRect.position.y * newScale, ribbonRect.position.z);
        ribbonRect.sizeDelta = new Vector2(Screen.width, Screen.height / 6);

        highscoreImg.transform.localScale = new Vector3(newScale, newScale, newScale);
        highscoreImg.anchoredPosition = new Vector3(highscoreRect.sizeDelta.x, highscoreImg.transform.position.y, highscoreImg.transform.position.z);
        highscoreImg.position = new Vector3(highscoreImg.position.x, transformPointLogo.position.y, highscoreImg.position.z);

        customImg.transform.localScale = new Vector3(newScale, newScale, newScale);
        customImg.transform.localPosition = new Vector3(-customRect.sizeDelta.x - (customImg.sizeDelta.x * newScale), customImg.transform.position.y, customImg.transform.position.z);
        customImg.position = new Vector3(customImg.position.x, transformPointLogo.position.y, customImg.position.z);
    }

    public float GetSettingsSizeY()
	{
        return settingsRect.sizeDelta.y;
    }
}
