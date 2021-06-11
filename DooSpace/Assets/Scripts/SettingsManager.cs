using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    //[SerializeField] private RectTransform creditsRect;
    //[SerializeField] private RectTransform infoRect;
    [SerializeField] private RectTransform settingRect;
    [SerializeField] private RectTransform extraRect;
    [SerializeField] private GameObject soundBar;
    [SerializeField] private GameObject flags;
    [SerializeField] private GameObject info;
    [SerializeField] private Toggle infoToggle;
    [SerializeField] private Image flagImg;
    [SerializeField] private Sprite flagFrSpr;
    [SerializeField] private Sprite flagEnSpr;

    Vector3 startExtraPos;
    Vector3 endExtraPos;
    Vector3 startInfoPos;
    Vector3 endInfoPos;

    bool isExtraOpen = false;
    bool isOpenExtra;
    bool isCloseExtra;
    bool isSoundOpen;
    bool isFlagOpen;
    bool isInfoOpen;
    bool isGyroActive;
    float moveSpeed = 500f;

    void Start()
    {
        startExtraPos = extraRect.localPosition;
        endExtraPos = startExtraPos - new Vector3(0, settingRect.rect.height, 0);

        flags.SetActive(false);
        soundBar.SetActive(false);
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

    void ResetExtra()
	{
        extraRect.localPosition = startExtraPos;
        isExtraOpen = false;
        flags.SetActive(false);
        soundBar.SetActive(false);
        isFlagOpen = false;
        isSoundOpen = false;
    }

    void SetExtraOpen()
	{
        if (isExtraOpen)
            ResetExtra();
        if (isInfoOpen)
        {
            SwitchInfo();
            infoToggle.isOn = false;
        }
        isOpenExtra = true;
    }

    public void SetExtraClose()
	{
        isCloseExtra = true;
    }

    public void SwitchExtraSound()
	{
        if (!isSoundOpen)
        {
            SetExtraOpen();
            soundBar.SetActive(true);
            isSoundOpen = true;
        }
        else
            SetExtraClose();
    }

    public void SwitchExtraFlag()
    {
        if (!isFlagOpen)
        {
            SetExtraOpen();
            flags.SetActive(true);
            isFlagOpen = true;
        }
        else
            SetExtraClose();
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
            if (isExtraOpen)
                ResetExtra();
            info.SetActive(true);
            isInfoOpen = true;
        }
    }

    public void CloseInfo()
	{
        infoToggle.isOn = false;
        info.SetActive(false);
        isInfoOpen = false;
    }

    public void SetFlag(string _country)
	{
        if (_country == "fr")
            flagImg.sprite = flagFrSpr;
        else if (_country == "en")
            flagImg.sprite = flagEnSpr;
    }

    public void SwitchGyroscope()
	{
        if(isGyroActive)
		{

            isGyroActive = false;
		}
        else
		{

            isGyroActive = true;
        }
	}
}
