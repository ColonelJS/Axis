using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomScreen : MonoBehaviour
{
    public static CustomScreen instance;

    [SerializeField] private Text moneyText;

    //[SerializeField] private Image fuelLevelImg;
    [SerializeField] private Image wingLevelImg;
    [SerializeField] private Image bumperLevelImg;

    [SerializeField] private GameObject popUpValidate;
    [SerializeField] private GameObject popUpInfo;
    [SerializeField] private Text costValidateText;
    [SerializeField] private Text elementInfoText;
    [Space(10)]
    [SerializeField] private GameObject bumperIteration1;
    [SerializeField] private GameObject bumperIteration2;
    [SerializeField] private GameObject bumperIteration3;
    [SerializeField] private GameObject baseIteration1;
    [SerializeField] private GameObject baseIteration2;
    [SerializeField] private GameObject baseIteration3;
    [SerializeField] private GameObject wingsIteration1;
    [SerializeField] private GameObject wingsIteration2;
    [SerializeField] private GameObject wingsIteration3;
    [Space(8)]
    [SerializeField] private Button buttonSwitchScreen;
    [SerializeField] private Image switchScreenToolImg;
    [SerializeField] private Sprite spPencil;
    [SerializeField] private Sprite spWrench;
    [Space(8)]
    [SerializeField] private Animation partBumperAnim;
    [SerializeField] private Animation partBaseAnim;
    [SerializeField] private Animation partWingsAnim;
    [Space(8)]
    [SerializeField] private RectTransform moneyRect;
    [SerializeField] private RectTransform elementRect;
    [SerializeField] private RectTransform colorRect;

    int money = 0;
    int fuelLevel = 0;
    int wingLevel = 0;
    int bumperLevel = 0;

    float levelMax = 6;//4
    string elementSelected;

    float[] fuelUpgradeCost;
    float[] wingUpgradeCost;
    float[] bumperUpgradeCost;
    Dictionary<string, float[]> upgradeCost = new Dictionary<string, float[]>();
    public string[] elementInfo;

    bool isCustomScreen = false;
    bool isSetCustomScreen = false;
    bool isSetUpgradeScreen = false;
    bool hasAnimUpscale = false;
    bool hasAnimDownscale = false;

    Vector3 startElementPos;
    Vector3 endElementPos;
    Vector3 startColorPos;
    Vector3 endColorPos;
    float animSpeed = 1550f;//1000

    private void Awake()
	{
        if (instance == null)
            instance = this;
	}

	void Start()
    {
        fuelUpgradeCost = new float[6];
        wingUpgradeCost = new float[6];
        bumperUpgradeCost = new float[6];
        elementInfo = new string[3];
        upgradeCost.Add("fuel", fuelUpgradeCost);
        upgradeCost.Add("wing", wingUpgradeCost);
        upgradeCost.Add("bumper", bumperUpgradeCost);
        SetupUpgradeCost();
        SetupValueState();
        popUpValidate.SetActive(false);
        popUpInfo.SetActive(false);

        startElementPos = elementRect.localPosition;
        endElementPos = new Vector3(elementRect.localPosition.x, elementRect.localPosition.y - elementRect.rect.height, elementRect.localPosition.z);

        Vector3 colorPos = new Vector3(colorRect.localPosition.x, colorRect.localPosition.y, colorRect.localPosition.z);
        endColorPos = colorPos;
        startColorPos = new Vector3(colorPos.x, colorPos.y - colorRect.rect.height, colorPos.z);
        colorRect.localPosition = startColorPos;

        PlayAnimationsPartsBack();
    }

    void Update()
    {
        if (Input.touchCount > 0 && GameManager.instance.GetGameState() == GameManager.GameState.MENU)
        {
            GetTouchVoid();
        }

        UpdateScreenSelection();
    }

    public int GetBumperLevel()
	{
        return bumperLevel;
	}

    void UpdateScreenSelection()
	{
        if (isSetCustomScreen || isSetUpgradeScreen)
        {
            buttonSwitchScreen.interactable = false;
            if (isSetCustomScreen)
                SetCustomScreen();
            if (isSetUpgradeScreen)
                SetUpgradeScreen();
        }
        else
        {
            buttonSwitchScreen.interactable = true;
            if(isCustomScreen)
                switchScreenToolImg.sprite = spPencil;
            else
                switchScreenToolImg.sprite = spWrench;
        }
    }

    public int GetWingLevel()
    {
        return wingLevel;
    }

    public int GetFuelLevel()
    {
        return fuelLevel;
    }

    void SetupValueState()
	{
        /*if (PlayerPrefs.HasKey("money"))
            money = PlayerPrefs.GetInt("money");
        else
            money = 0;*/

        money = ZPlayerPrefs.GetInt("money", 0);
        moneyText.text = money.ToString();


        /*if (PlayerPrefs.HasKey("fuelLevel"))
            fuelLevel = PlayerPrefs.GetInt("fuelLevel");
        else
            fuelLevel = 0;*/
        //fuelLevelImg.fillAmount = fuelLevel / levelMax;

        /*if (PlayerPrefs.HasKey("wingLevel"))
            wingLevel = PlayerPrefs.GetInt("wingLevel");
        else
            wingLevel = 0;*/

        wingLevel = ZPlayerPrefs.GetInt("wingLevel", 0);
        wingLevelImg.fillAmount = wingLevel / levelMax;

        /*if (PlayerPrefs.HasKey("bumperLevel"))
            bumperLevel = PlayerPrefs.GetInt("bumperLevel");
        else
            bumperLevel = 0;*/

        bumperLevel = ZPlayerPrefs.GetInt("bumperLevel", 0);
        bumperLevelImg.fillAmount = bumperLevel / levelMax;
    }

    public void SetMoneyAndUpgradesLevel(int _money, int _bumperLevel, int _wingsLevel)
    {
        money = _money;
        bumperLevel = _bumperLevel;
        wingLevel = _wingsLevel;
        Debug.Log("SET MONEY 2 : " + money);
        ZPlayerPrefs.SetInt("money", money);
        ZPlayerPrefs.SetInt("bumperLevel", bumperLevel);
        ZPlayerPrefs.SetInt("wingLevel", wingLevel);
        UpdatesLevelsMoney();
    }

    public int GetPlayerMoney()
    {
        return money;
    }

    public int GetWingsLevel()
    {
        return wingLevel;
    }

    public void SetNewMoney(int _newMoney)
    {
        money = _newMoney;
        ZPlayerPrefs.SetInt("money", money);
        SkinManager.instance.SetPlayerDataMoney(money);
    }

    void SetupUpgradeCost()
	{
        /*upgradeCost["fuel"][0] = 1000;//500
        upgradeCost["fuel"][1] = 2400;//1100
        upgradeCost["fuel"][2] = 4000;//1800
        upgradeCost["fuel"][3] = 6000;//2600

        upgradeCost["wing"][0] = 1000;
        upgradeCost["wing"][1] = 2400;
        upgradeCost["wing"][2] = 4000;
        upgradeCost["wing"][3] = 6000;

        upgradeCost["bumper"][0] = 1000;
        upgradeCost["bumper"][1] = 2400;
        upgradeCost["bumper"][2] = 4000;
        upgradeCost["bumper"][3] = 6000;*/

        float x = 500;
        float fx;

        for (int i = 1; i < 7; i++)
		{
            fx = 500 * Mathf.Pow(i, 1.8f) + x;
            //Debug.Log("lvl " + i + " :" + fx);
            upgradeCost["fuel"][i - 1] = fx;
            upgradeCost["wing"][i - 1] = fx;
            upgradeCost["bumper"][i - 1] = fx;
        }
    }

    public void OpenPopUpValidate(string _elementName)
	{
        string str = _elementName;
        elementSelected = str;
        if (str == "fuelLevel")
            costValidateText.text = ((int)upgradeCost["fuel"][fuelLevel]).ToString();
        if (str == "wingLevel")
            costValidateText.text = ((int)upgradeCost["wing"][wingLevel]).ToString();
        if (str == "bumperLevel")
            costValidateText.text = ((int)upgradeCost["bumper"][bumperLevel]).ToString();

        popUpValidate.SetActive(true);
        CloseInfo();
    }

    void GetTouchVoid()
	{
        RaycastHit2D hit = Physics2D.Raycast(Input.touches[0].position, Vector2.zero);

        if (hit.collider == null)
		{
            popUpValidate.SetActive(false);
            CloseInfo();
        }
	}

    public void UpgradeElement()
    {
        string str = elementSelected;
        bool isFirstUpgrade = false;
        bool isUpgrade = false;
        if (/*fuelLevel == 0 && */wingLevel == 0 && bumperLevel == 0)
            isFirstUpgrade = true;

        if (str == "fuelLevel")
		{
            if (money >= upgradeCost["fuel"][fuelLevel])
            {
                fuelLevel++;
                money -= (int)upgradeCost["fuel"][fuelLevel-1];
                ZPlayerPrefs.SetInt("fuelLevel", fuelLevel);
                ZPlayerPrefs.SetInt("money", money);
                popUpValidate.SetActive(false);
                //isUpgrade = true;
            }
        }
        if (str == "wingLevel")
        {
            if (money >= upgradeCost["wing"][wingLevel])
            {
                wingLevel++;
                money -= (int)upgradeCost["wing"][wingLevel - 1];
                ZPlayerPrefs.SetInt("wingLevel", wingLevel);
                ZPlayerPrefs.SetInt("money", money);
                popUpValidate.SetActive(false);
                FireBaseAuthScript.instance.SendPlayerWingLevelData(wingLevel);
                isUpgrade = true;
            }
        }
        if (str == "bumperLevel")
        {
            if (money >= upgradeCost["bumper"][bumperLevel])
            {
                bumperLevel++;
                money -= (int)upgradeCost["bumper"][bumperLevel - 1];
                ZPlayerPrefs.SetInt("bumperLevel", bumperLevel);
                ZPlayerPrefs.SetInt("money", money);
                popUpValidate.SetActive(false);
                FireBaseAuthScript.instance.SendPlayerBumperLevelData(bumperLevel);
                isUpgrade = true;
            }
        }
        UpdateElement(str);
        FireBaseAuthScript.instance.SendPlayerMoneyData(money);
        SoundManager.instance.PlaySound("buyUpgrade");

        if (isFirstUpgrade && isUpgrade)
            GooglePlayServicesManager.instance.ReportSucces("CgkI6LzEr7kGEAIQDw", 100f); //ACHIEVEMENT 5 / tuning
        else if(wingLevel == 6 && bumperLevel == 6)
            GooglePlayServicesManager.instance.ReportSucces("CgkI6LzEr7kGEAIQFQ", 100f); //ACHIEVEMENT 11 / armored
    }

    void UpdateElement(string _elementName)
    {
        string str = _elementName;
        if (str == "fuelLevel")
        {
            //fuelLevelImg.fillAmount = floatLevel / levelMax;
        }
        if (str == "wingLevel")
            wingLevelImg.fillAmount = wingLevel / levelMax;
        if (str == "bumperLevel")
            bumperLevelImg.fillAmount = bumperLevel / levelMax;

        moneyText.text = money.ToString();
    }

    void UpdatesLevelsMoney()
    {
        wingLevelImg.fillAmount = wingLevel / levelMax;
        bumperLevelImg.fillAmount = bumperLevel / levelMax;

        moneyText.text = money.ToString();
        Debug.Log("UPDATE MONEY");
    }

    public void OpenInfo(string _elementName)
	{
        SoundManager.instance.PlaySound("openInfo");
        string str = _elementName;
        popUpInfo.SetActive(true);
        if (str == "fuel")
            SetElementInfoText(0);
        else if (str == "wing")
            SetElementInfoText(1);
        else if (str == "bumper")
            SetElementInfoText(2);
        popUpValidate.SetActive(false);
    }

    public void CloseInfo()
	{
        popUpInfo.SetActive(false);
    }

    void SetElementInfoText(int _strIndex)
	{
        elementInfoText.text = LanguageManager.instance.elementInfo[_strIndex];
    }

    bool MoveColorsEnd()
    {
        if (colorRect.localPosition.y < endColorPos.y)
            colorRect.localPosition += new Vector3(0, animSpeed, 0) * Time.deltaTime;
        else
        {
            colorRect.localPosition = new Vector3(colorRect.localPosition.x, endColorPos.y, colorRect.localPosition.z);
            return true;
        }

        return false;
    }

    bool MoveColorsBackEnd()
    {
        if (colorRect.localPosition.y > startColorPos.y)
            colorRect.localPosition -= new Vector3(0, animSpeed, 0) * Time.deltaTime;
        else
        {
            colorRect.localPosition = new Vector3(colorRect.localPosition.x, startColorPos.y, colorRect.localPosition.z);
            SkinManager.instance.HideCaseInfo();
            return true;
        }

        return false;
    }

    bool MoveElementsEnd()
    {
        bool elementsEnd = false;

        if (elementRect.localPosition.y > endElementPos.y)
            elementRect.localPosition -= new Vector3(0, animSpeed, 0) * Time.deltaTime;
        else
        {
            elementRect.localPosition = new Vector3(elementRect.localPosition.x, endElementPos.y, elementRect.localPosition.z);
            elementsEnd = true;
        }

        if (elementsEnd)
            return true;
        else
            return false;
    }

    bool MoveElementsBackEnd()
    {
        bool elementsEnd = false;

        if (elementRect.localPosition.y < startElementPos.y)
            elementRect.localPosition += new Vector3(0, animSpeed, 0) * Time.deltaTime;
        else
        {
            elementRect.localPosition = new Vector3(elementRect.localPosition.x, startElementPos.y, elementRect.localPosition.z);
            elementsEnd = true;
        }

        if (elementsEnd)
            return true;
        else
            return false;
    }

    public void SwitchScreen()
    {
        if (isCustomScreen)
            isSetUpgradeScreen = true;
        else
            isSetCustomScreen = true;

        hasAnimDownscale = false;
        hasAnimUpscale = false;
    }

    void SetCustomScreen()
	{
        if (MoveElementsEnd())
        {
            if (!hasAnimUpscale)
            {
                PlayAnimationsParts();
                hasAnimUpscale = true;
            }

            if (MoveColorsEnd())
            {
                isSetCustomScreen = false;
                isCustomScreen = true;
            }
        }
	}

    void SetUpgradeScreen()
    {
        if (!hasAnimDownscale)
        {
            PlayAnimationsPartsBack();
            hasAnimDownscale = true;
        }

        if (MoveColorsBackEnd())
		{
            if (MoveElementsBackEnd())
			{
                isSetUpgradeScreen = false;
                isCustomScreen = false;
            }
		}
    }

    void PlayAnimationsParts()
    {
        partBumperAnim.Play("Part_Upscale");
        partBaseAnim.Play("Part_Upscale");
        partWingsAnim.Play("Part_Upscale");
    }

    void PlayAnimationsPartsBack()
    {
        partBumperAnim.Play("Part_Downscale");
        partBaseAnim.Play("Part_Downscale");
        partWingsAnim.Play("Part_Downscale");
    }

    public void OpenBumperIterations()
    {
        if (bumperIteration1.activeSelf)
            ClosePartsIterations();
        else
        {
            if(baseIteration1.activeSelf || wingsIteration1.activeSelf)
                ClosePartsIterations();

            bumperIteration1.SetActive(true);
            bumperIteration2.SetActive(true);
            bumperIteration3.SetActive(true);
        }
    }
    public void OpenBaseIterations()
    {
        if (baseIteration1.activeSelf)
            ClosePartsIterations();
        else
        {
            if (bumperIteration1.activeSelf || wingsIteration1.activeSelf)
                ClosePartsIterations();

            baseIteration1.SetActive(true);
            baseIteration2.SetActive(true);
            baseIteration3.SetActive(true);
        }
    }
    public void OpenWindsIterations()
    {
        if (wingsIteration1.activeSelf)
            ClosePartsIterations();
        else
        {
            if (bumperIteration1.activeSelf || baseIteration1.activeSelf)
                ClosePartsIterations();

            wingsIteration1.SetActive(true);
            wingsIteration2.SetActive(true);
            wingsIteration3.SetActive(true);
        }
    }

    public void ClosePartsIterations()
	{
        bumperIteration1.SetActive(false);
        bumperIteration2.SetActive(false);
        bumperIteration3.SetActive(false);
        baseIteration1.SetActive(false);
        baseIteration2.SetActive(false);
        baseIteration3.SetActive(false);
        wingsIteration1.SetActive(false);
        wingsIteration2.SetActive(false);
        wingsIteration3.SetActive(false);
    }
}
