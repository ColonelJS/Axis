using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private RectTransform settingRect;
    [SerializeField] private RectTransform extraRect;
    [SerializeField] private RectTransform extraFlagRect;
    [SerializeField] private GameObject info;
    [SerializeField] private GameObject credits;
    [SerializeField] private Image flagImg;
    [SerializeField] private Sprite flagFrSpr;
    [SerializeField] private Sprite flagEnSpr;
    [SerializeField] private Button buttonFlagFr;
    [SerializeField] private Button buttonFlagEn;
    [SerializeField] private Transform infoContentTransform;
    [SerializeField] private Transform creditContentTransform;
    [SerializeField] private CharacterMovement characterMovements;
    [SerializeField] private Sprite spGyroUnactive;
    [SerializeField] private Sprite spGyroActive;
    [SerializeField] private Image buttonGyroImg;
    [SerializeField] private GameObject fadeTextGyro;

    Vector3 startExtraPos;
    Vector3 endExtraPos;

    Vector3 startExtraFlagPos;
    Vector3 endExtraFlagPos;

    bool isExtraOpen = false;
    bool isOpenExtra;
    bool isCloseExtra;
    bool isExtraFlagOpen = false;
    bool isOpenExtraFlag;
    bool isCloseExtraFlag;
    bool isSoundOpen;
    bool isFlagOpen;
    bool isInfoOpen;
    bool isCreditsOpen;
    bool isGyroActive;
    float moveSpeed = 500f;

    void Start()
    {
        startExtraPos = extraRect.localPosition;
        endExtraPos = startExtraPos - new Vector3(0, settingRect.rect.height, 0);

        startExtraFlagPos = extraFlagRect.localPosition;
        endExtraFlagPos = startExtraFlagPos - new Vector3(0, settingRect.rect.height, 0);

        if (LanguageManager.instance.GetLanguage() == "fr")
            SetFlag("fr");
        else
            SetFlag("en");

        int gyro = PlayerPrefs.GetInt("gyroscopeEnabled", 0);
        if (gyro == 0)
        {
            buttonGyroImg.sprite = spGyroUnactive;
            isGyroActive = false;
        }
        else if (gyro == 1)
        {
            buttonGyroImg.sprite = spGyroActive;
            isGyroActive = true;
        }
    }

    void Update()
    {
        UpdateExtra();
    }

    void UpdateExtra()
	{
        if (isOpenExtra)
            OpenExtra();
        else if (isCloseExtra)
            CloseExtra();

        if (isOpenExtraFlag)
            OpenExtraFlag();
        else if (isCloseExtraFlag)
            CloseExtraFlag();
    }

    void OpenExtra()
	{
        if (extraRect.localPosition.y > endExtraPos.y)
            extraRect.localPosition -= new Vector3(0, moveSpeed, 0) * Time.deltaTime;
        else
		{
            extraRect.localPosition = endExtraPos;
            isOpenExtra = false;
            isExtraOpen = true;
        }
    }

    public void CloseExtra()
    {
        if (extraRect.localPosition.y < startExtraPos.y)
            extraRect.localPosition += new Vector3(0, moveSpeed, 0) * Time.deltaTime;
        else
        {
            isCloseExtra = false;
            ResetExtra();
        }
    }

    void OpenExtraFlag()
    {
        if (extraFlagRect.localPosition.y > endExtraFlagPos.y)
            extraFlagRect.localPosition -= new Vector3(0, moveSpeed, 0) * Time.deltaTime;
        else
        {
            extraFlagRect.localPosition = endExtraFlagPos;
            isOpenExtraFlag = false;
            isExtraFlagOpen = true;
        }
    }

    public void CloseExtraFlag()
    {
        if (extraFlagRect.localPosition.y < startExtraFlagPos.y)
            extraFlagRect.localPosition += new Vector3(0, moveSpeed, 0) * Time.deltaTime;
        else
        {
            isCloseExtraFlag = false;
            ResetExtraFlag();
        }
    }

    void ResetExtra()
	{
        extraRect.localPosition = startExtraPos;
        isExtraOpen = false;
        isSoundOpen = false;
    }

    void ResetExtraFlag()
    {
        extraFlagRect.localPosition = startExtraFlagPos;
        isExtraFlagOpen = false;
        isFlagOpen = false;
    }

    void SetExtraOpen()
	{
        if (isExtraOpen)
            ResetExtra();
        if (isInfoOpen)
            SwitchInfo();
        if (isCreditsOpen)
            SwitchCredits();
        isOpenExtra = true;
    }

    void SetExtraFlagOpen()
    {
        if (isExtraFlagOpen)
            ResetExtra();
        if (isInfoOpen)
            SwitchInfo();
        if (isCreditsOpen)
            SwitchCredits();
        isOpenExtraFlag = true;
    }

    public void SetExtraClose()
	{
        isCloseExtra = true;
    }

    public void SetExtraFlagClose()
    {
        isCloseExtraFlag = true;
    }

    public void SwitchExtraSound()
	{
        if (!isSoundOpen)
        {
            if(isFlagOpen)
                SetExtraFlagClose();

            SetExtraOpen();
            isSoundOpen = true;
        }
        else
            SetExtraClose();
    }

    public void SwitchExtraFlag()
    {
        if (!isFlagOpen)
        {
            if (isSoundOpen)
                SetExtraClose();

            SetExtraFlagOpen();
            isFlagOpen = true;
        }
        else
            SetExtraFlagClose();
    }

    public void SwitchInfo()
    {
        if (isInfoOpen)
        {
            info.SetActive(false);
            isInfoOpen = false;
        }
        else
        {
            if (isCreditsOpen)
                SwitchCredits();

            if (isExtraOpen)
                ResetExtra();
            if (isExtraFlagOpen)
                ResetExtraFlag();

            info.SetActive(true);
            isInfoOpen = true;
        }
    }

    public void SwitchCredits()
    {
        if (isCreditsOpen)
        {
            credits.SetActive(false);
            isCreditsOpen = false;
        }
        else
        {
            if (isInfoOpen)
                SwitchInfo();

            if (isExtraOpen)
                ResetExtra();
            if (isExtraFlagOpen)
                ResetExtraFlag();

            credits.SetActive(true);
            isCreditsOpen = true;
        }
    }

    public void CloseInfo()
	{
        info.SetActive(false);
        isInfoOpen = false;
    }

    public void CloseCredits()
    {
        credits.SetActive(false);
        isCreditsOpen = false;
    }

    public bool GetIsInfoOpen()
	{
        return isInfoOpen;
	}

    public bool GetIsCreditsOpen()
    {
        return isCreditsOpen;
    }

    public void SetFlag(string _country)
	{
        if (_country == "fr")
        {
            flagImg.sprite = flagFrSpr;
            buttonFlagFr.interactable = false;
        }
        else if (_country == "en")
        {
            flagImg.sprite = flagEnSpr;
            buttonFlagEn.interactable = false;
        }

        PlayerPrefs.SetString("language", _country);
        LanguageManager.instance.UpdateLanguageText();
    }

    public void SwitchGyroscope()
	{
        if(isGyroActive)
		{
            characterMovements.SetGyroscope(false);
            isGyroActive = false;
            buttonGyroImg.sprite = spGyroUnactive;
            fadeTextGyro.GetComponent<AutoFade>().StartFade();
            if(PlayerPrefs.GetString("language") == "fr")
                fadeTextGyro.GetComponent<AutoFade>().SetText("Gyroscope désactivé !");
            else
                fadeTextGyro.GetComponent<AutoFade>().SetText("Gyroscope disabled !");
        }
        else
		{
            characterMovements.SetGyroscope(true);
            isGyroActive = true;
            buttonGyroImg.sprite = spGyroActive;
            fadeTextGyro.GetComponent<AutoFade>().StartFade();
            if (PlayerPrefs.GetString("language") == "fr")
                fadeTextGyro.GetComponent<AutoFade>().SetText("Gyroscope activé !");
            else
                fadeTextGyro.GetComponent<AutoFade>().SetText("Gyroscope enabled !");
        }
	}
}
