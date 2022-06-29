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
    [SerializeField] private GameObject playerInfo;
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
    [SerializeField] private Sprite spInfoUnactive;
    [SerializeField] private Sprite spInfoActive;
    [SerializeField] private Sprite spCreditsUnactive;
    [SerializeField] private Sprite spCreditsActive;
    [SerializeField] private Sprite spPlayerInfoUnactive;
    [SerializeField] private Sprite spPlayerInfoActive;
    [SerializeField] private Sprite spAudioUnactive;
    [SerializeField] private Sprite spAudioActive;
    [SerializeField] private Sprite spFlagsUnactive;
    [SerializeField] private Sprite spFlagsActive;
    [SerializeField] private Image buttonGyroImg;
    [SerializeField] private Image buttonInfoImg;
    [SerializeField] private Image buttonCreditsImg;
    [SerializeField] private Image buttonPlayerInfoImg;
    [SerializeField] private Image buttonAudioImg;
    [SerializeField] private Image buttonFlagsImg;
    [SerializeField] private GameObject fadeTextGyro;
    [SerializeField] private TitleScreen titleScreen;

    [SerializeField] private Text txtPlayerName;
    [SerializeField] private Text txtLevel;
    [SerializeField] private Text txtNbSucces;
    [SerializeField] private Text txtNbSuccesMax;
    [SerializeField] private Text txtRank;
    [SerializeField] private Text txtScore;
    [SerializeField] private Text txtGameVersion;
    [SerializeField] private GameObject GoConnectToGPGS;

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
    bool isPlayerInfoOpen;
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

        GoConnectToGPGS.SetActive(false);
        txtGameVersion.text = Application.version;
    }

    void Update()
    {
        UpdateExtra();

        if(isExtraOpen)
		{
            RaycastHit2D hit = Physics2D.Raycast(Input.touches[0].position, Vector2.zero);
            if (hit.collider == null)
                isCloseExtra = true;
        }
    }

    public bool GetIsExtrasOpen()
	{
        if (isExtraOpen || isExtraFlagOpen)
            return true;
        else
            return false;
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

        if(!titleScreen.GetIsSettingsTrueOpen())
		{
            if (isFlagOpen)
                SwitchExtraFlag();

            if (isSoundOpen)
                SwitchExtraSound();
		}
    }

    void OpenExtra()
	{
        buttonAudioImg.sprite = spAudioActive;
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
        buttonAudioImg.sprite = spAudioUnactive;
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
        buttonFlagsImg.sprite = spFlagsActive;
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
        buttonFlagsImg.sprite = spFlagsUnactive;
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
        if (isPlayerInfoOpen)
            SwitchPlayerInfo();
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
        if (isPlayerInfoOpen)
            SwitchPlayerInfo();
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
            buttonInfoImg.sprite = spInfoUnactive;
        }
        else
        {
            if (isCreditsOpen)
                SwitchCredits();

            if (isPlayerInfoOpen)
                SwitchPlayerInfo();

            if (isExtraOpen)
                ResetExtra();
            if (isExtraFlagOpen)
                ResetExtraFlag();

            info.SetActive(true);
            isInfoOpen = true;
            buttonInfoImg.sprite = spInfoActive;
        }
    }

    public void SwitchCredits()
    {
        if (isCreditsOpen)
        {
            credits.SetActive(false);
            isCreditsOpen = false;
            buttonCreditsImg.sprite = spCreditsUnactive;
        }
        else
        {
            if (isInfoOpen)
                SwitchInfo();

            if (isPlayerInfoOpen)
                SwitchPlayerInfo();

            if (isExtraOpen)
                ResetExtra();
            if (isExtraFlagOpen)
                ResetExtraFlag();

            credits.SetActive(true);
            isCreditsOpen = true;
            buttonCreditsImg.sprite = spCreditsActive;
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

    public void ClosePlayerInfo()
    {
        playerInfo.SetActive(false);
        isPlayerInfoOpen = false;
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

    public void SwitchPlayerInfo()
    {
        if(isPlayerInfoOpen)
        {
            playerInfo.SetActive(false);
            isPlayerInfoOpen = false;
            buttonPlayerInfoImg.sprite = spPlayerInfoUnactive;
        }
        else
        {
            if (isInfoOpen)
                SwitchInfo();

            if (isCreditsOpen)
                SwitchCredits();

            if (isExtraOpen)
                ResetExtra();
            if (isExtraFlagOpen)
                ResetExtraFlag();

            playerInfo.SetActive(true);
            LoadPlayerInfo();
            isPlayerInfoOpen = true;
            buttonPlayerInfoImg.sprite = spPlayerInfoActive;
        }
    }

    public bool GetIsPlayerInfoOpen()
    {
        return isPlayerInfoOpen;
    }

    void LoadPlayerInfo()
    {
        if(GooglePlayServicesManager.instance.GetIsConnectedToGPGS() && FireBaseAuthScript.instance.GetIsConnectedToFireBase())
        {
            if (FireBaseAuthScript.instance.GetIsLocalPlayerScoreFind())
            {
                GoConnectToGPGS.SetActive(false);
                int nbSucces = GooglePlayServicesManager.instance.GetNbSuccesunlocked();
                int nbSuccesMax = GooglePlayServicesManager.instance.GetNbSuccesMax();
                string strSuccesMax = "/" + nbSuccesMax.ToString();
                string strNbSucces = "";
                if (nbSucces < 10)
                    strNbSucces = "0" + nbSucces.ToString();
                else
                    strNbSucces = nbSucces.ToString();

                int rank = FireBaseAuthScript.instance.GetCurrentPlayerRank();

                string strRank = "";
                if (rank < 10)
                    strRank = "#0" + rank.ToString();
                else
                    strRank = "#" + rank.ToString();

                string strScore = FireBaseAuthScript.instance.GetCurrentPlayer().score.ToString();
                string strName = FireBaseAuthScript.instance.GetCurrentPlayer().name;
                string strLevel = (SkinManager.instance.GetPlayerData().currentSkinIndexToOpen + 1).ToString();

                txtPlayerName.text = strName;
                txtLevel.text = strLevel;
                txtNbSucces.text = strNbSucces;
                txtNbSuccesMax.text = strSuccesMax;
                txtRank.text = strRank;
                txtScore.text = strScore;
            }
            else
            {
                LoadPlayerNotRegistered();
            }
        }
        else
        {
            LoadPlayerNotConnect();
        }
    }

    void LoadPlayerNotConnect()
    {
        txtPlayerName.text = "unknown";
        txtLevel.text = "00";
        txtNbSucces.text = "00";
        txtNbSuccesMax.text = "/00";
        txtRank.text = "#00";
        txtScore.text = "0";
        GoConnectToGPGS.SetActive(true);
    }

    void LoadPlayerNotRegistered()
    {
        txtPlayerName.text =  Social.localUser.userName;
        txtLevel.text = "01";
        txtNbSucces.text = "00";
        txtNbSuccesMax.text = "/" + GooglePlayServicesManager.instance.GetNbSuccesMax().ToString();
        txtRank.text = "#00";
        txtScore.text = "0";
    }
}
