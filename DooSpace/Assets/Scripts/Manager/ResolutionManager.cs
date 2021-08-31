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
    [SerializeField] RectTransform bgDownRectTransform;
    [SerializeField] RectTransform ribbonRect;
    [Space(8)]
    [Header("Settings")]
    [SerializeField] GameObject bgInfo;
    [SerializeField] RectTransform bgInfoRectTransform;
    [SerializeField] RectTransform bgInfoScrollViewRectTransform;
    [Space(6)]
    [SerializeField] GameObject bgCredits;
    [SerializeField] RectTransform bgCreditsRectTransform;
    [SerializeField] RectTransform bgCreditsScrollViewRectTransform;
    [Space(8)]
    [Header("Screens rect")]
    [SerializeField] RectTransform mainScreenRect;
    [SerializeField] RectTransform settingsRect;
    [SerializeField] GameObject bgUpElements;
    [SerializeField] GameObject buttonStart;
    [SerializeField] GameObject settingsElements;
    [SerializeField] RectTransform highscoreRect;
    [SerializeField] GameObject highscoreElements;
    [SerializeField] RectTransform customRect;
    [SerializeField] GameObject customElements;
    [SerializeField] RectTransform spaceRect;
    [SerializeField] RectTransform skyRect;
    [SerializeField] RectTransform earthRect;
    [SerializeField] GameObject skyElements;
    [SerializeField] GameObject characterElements;
    [SerializeField] Transform characterSpawnPoint;
    [SerializeField] GameObject adsPopUpElements;

    [SerializeField] Transform fuelSpawnPoint;
    [SerializeField] GameObject fuelElements;

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
        //SetupSettings();
    }

    void Update()
    {
        fuelElements.transform.position = new Vector3(fuelSpawnPoint.position.x, fuelElements.transform.position.y, fuelElements.transform.position.z);
    }

    Vector2 GetResolution()
	{
        Vector2 res;
        res.x = Screen.width;  //.currentResolution.width;
        res.y = Screen.height; //.currentResolution.height;
        return res;
	}

    void SetupCanvasScaler()
	{
        menusCanvasScaler.referenceResolution = GetResolution();
        backgroundCanvasScaler.referenceResolution = GetResolution();
        playerCanvasScaler.referenceResolution = GetResolution();
        resultsCanvasScaler.referenceResolution = GetResolution();
        //hudCanvasScaler.referenceResolution = GetResolution();
    }

    void SetupScreenRect()
	{
        mainScreenRect.sizeDelta = new Vector2(Screen.width, Screen.height);
        highscoreRect.sizeDelta = new Vector2(Screen.width, Screen.height);
        customRect.sizeDelta = new Vector2(Screen.width, Screen.height);

        float newScale = GetResolution().y / 2400;
        bgUpElements.transform.localScale = new Vector3(newScale, newScale, newScale);
        //buttonStart.transform.localScale = new Vector3(newScale, newScale, newScale);
        settingsElements.transform.localScale = new Vector3(newScale, newScale, newScale);
        highscoreElements.transform.localScale = new Vector3(newScale, newScale, newScale);
        customElements.transform.localScale = new Vector3(newScale, newScale, newScale);

        settingsRect.sizeDelta = new Vector2(settingsRect.sizeDelta.x * newScale, settingsRect.sizeDelta.y * newScale);

        spaceRect.sizeDelta = new Vector2(Screen.width, Screen.height);
        skyRect.sizeDelta = new Vector2(Screen.width, Screen.height);
        //earthRect.sizeDelta = new Vector2(Screen.height/2, Screen.height/2);
        skyElements.transform.localScale = new Vector3(newScale, newScale, newScale);
        characterElements.transform.position = characterSpawnPoint.position;
        characterElements.transform.localScale = new Vector3(newScale, newScale, newScale);
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
        //bgUp.transform.localPosition = new Vector3(0, GetResolution().y/2, 0);
        //bgUpRectTransform.sizeDelta = new Vector2(GetResolution().x, GetResolution().y / 2);

        bgUp.transform.localPosition = new Vector3(0, 0, 0);
        bgUpRectTransform.sizeDelta = GetResolution();

        float newScale = GetResolution().y / 2400;
        ribbonRect.position = new Vector3(ribbonRect.position.x, ribbonRect.position.y * newScale, ribbonRect.position.z);
        ribbonRect.sizeDelta = new Vector2(Screen.width, Screen.height / 6);

        bgDownRectTransform.sizeDelta = new Vector2(GetResolution().x, GetResolution().y / 2);
    }

    void SetupSettings()
	{
        bgInfo.transform.position = new Vector3(bgInfo.transform.position.x, GetSettingsSizeY(), bgInfo.transform.position.z);
        bgCredits.transform.position = bgInfo.transform.position;
    }

    public float GetSettingsSizeY()
	{
        return settingsRect.sizeDelta.y;
    }
}
