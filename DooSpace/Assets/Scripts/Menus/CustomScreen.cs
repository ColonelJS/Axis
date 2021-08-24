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

    float levelMax = 4;
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

    Vector3 startMoneyPos;
    Vector3 endMoneyPos;
    Vector3 startElementPos;
    Vector3 endElementPos;
    Vector3 startColorPos;
    Vector3 endColorPos;
    float animSpeed = 1000f;

    private void Awake()
	{
        if (instance == null)
            instance = this;
	}

	void Start()
    {
        //PlayerPrefs.DeleteAll();
        fuelUpgradeCost = new float[4];
        wingUpgradeCost = new float[4];
        bumperUpgradeCost = new float[4];
        elementInfo = new string[3];
        upgradeCost.Add("fuel", fuelUpgradeCost);
        upgradeCost.Add("wing", wingUpgradeCost);
        upgradeCost.Add("bumper", bumperUpgradeCost);
        SetupUpgradeCost();
        SetupValueState();
        popUpValidate.SetActive(false);
        popUpInfo.SetActive(false);

        //startMoneyPos = moneyRect.localPosition;
        //endMoneyPos = new Vector3(moneyRect.localPosition.x, Screen.height/2 + moneyRect.rect.height, moneyRect.localPosition.z);

        startElementPos = elementRect.localPosition;
        endElementPos = new Vector3(elementRect.localPosition.x, /*-Screen.height / 1.5f*/elementRect.localPosition.y - elementRect.rect.height, elementRect.localPosition.z);

        Vector3 colorPos = new Vector3(colorRect.localPosition.x, colorRect.localPosition.y, colorRect.localPosition.z);
        endColorPos = colorPos;
        startColorPos = new Vector3(colorPos.x, colorPos.y - colorRect.rect.height, colorPos.z);
        colorRect.localPosition = startColorPos;

        PlayAnimationsPartsBack();
    }

    void Update()
    {
        /*if(Input.GetMouseButtonDown(0))
		{
            GetTouchVoid();
        }*/

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
        if (PlayerPrefs.HasKey("money"))
            money = PlayerPrefs.GetInt("money");
        else
            money = 0; //5000
        moneyText.text = money.ToString();


        if (PlayerPrefs.HasKey("fuelLevel"))
            fuelLevel = PlayerPrefs.GetInt("fuelLevel");
        else
            fuelLevel = 0;
        //fuelLevelImg.fillAmount = fuelLevel / levelMax;

        if (PlayerPrefs.HasKey("wingLevel"))
            wingLevel = PlayerPrefs.GetInt("wingLevel");
        else
            wingLevel = 0;
        wingLevelImg.fillAmount = wingLevel / levelMax;

        if (PlayerPrefs.HasKey("bumperLevel"))
            bumperLevel = PlayerPrefs.GetInt("bumperLevel");
        else
            bumperLevel = 0;
        bumperLevelImg.fillAmount = bumperLevel / levelMax;
    }

    void SetupUpgradeCost()
	{
        upgradeCost["fuel"][0] = 500;
        upgradeCost["fuel"][1] = 1100;
        upgradeCost["fuel"][2] = 1800;
        upgradeCost["fuel"][3] = 2600;

        upgradeCost["wing"][0] = 500;
        upgradeCost["wing"][1] = 1100;
        upgradeCost["wing"][2] = 1800;
        upgradeCost["wing"][3] = 2600;

        upgradeCost["bumper"][0] = 500;
        upgradeCost["bumper"][1] = 1100;
        upgradeCost["bumper"][2] = 1800;
        upgradeCost["bumper"][3] = 2600;
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
        //Vector2 clickPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        RaycastHit2D hit = Physics2D.Raycast(Input.touches[0].position, Vector2.zero);

        if (hit.collider == null)
		{
            popUpValidate.SetActive(false);
            CloseInfo();
            //SoundManager.instance.CloseSlider();
        }
	}

    public void UpgradeElement()
	{
        string str = elementSelected;
        if(str == "fuelLevel")
		{
            if (money >= upgradeCost["fuel"][fuelLevel])
            {
                fuelLevel++;
                money -= (int)upgradeCost["fuel"][fuelLevel-1];
                PlayerPrefs.SetInt("fuelLevel", fuelLevel);
                PlayerPrefs.SetInt("money", money);
                popUpValidate.SetActive(false);
            }
        }
        if (str == "wingLevel")
        {
            if (money >= upgradeCost["wing"][wingLevel])
            {
                wingLevel++;
                money -= (int)upgradeCost["wing"][wingLevel - 1];
                PlayerPrefs.SetInt("wingLevel", wingLevel);
                PlayerPrefs.SetInt("money", money);
                popUpValidate.SetActive(false);
            }
        }
        if (str == "bumperLevel")
        {
            if (money >= upgradeCost["bumper"][bumperLevel])
            {
                bumperLevel++;
                money -= (int)upgradeCost["bumper"][bumperLevel - 1];
                PlayerPrefs.SetInt("bumperLevel", bumperLevel);
                PlayerPrefs.SetInt("money", money);
                popUpValidate.SetActive(false);
            }
        }
        UpdateElement(str);
        SoundManager.instance.PlaySound("buyUpgrade");
    }

    void UpdateElement(string _elementName)
    {
        string str = _elementName;
        if (str == "fuelLevel")
        {
            float floatLevel = fuelLevel;
            //fuelLevelImg.fillAmount = floatLevel / levelMax;
        }
        if (str == "wingLevel")
        {
            float floatLevel = wingLevel;
            wingLevelImg.fillAmount = wingLevel / levelMax;
        }
        if (str == "bumperLevel")
        {
            float floatLevel = bumperLevel;
            bumperLevelImg.fillAmount = bumperLevel / levelMax;
        }
        moneyText.text = money.ToString();
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
        //elementInfoText.text = elementInfo[_strIndex];
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
            return true;
        }

        return false;
    }

    bool MoveMoneyEnd()
	{
        if (moneyRect.localPosition.y < endMoneyPos.y)
            moneyRect.localPosition += new Vector3(0, animSpeed, 0) * Time.deltaTime;
        else
        {
            moneyRect.localPosition = new Vector3(moneyRect.localPosition.x, endMoneyPos.y, moneyRect.localPosition.z);
            return true;
        }

        return false;
    }

    bool MoveMoneyBackEnd()
    {
        if (moneyRect.localPosition.y > startMoneyPos.y)
            moneyRect.localPosition -= new Vector3(0, animSpeed, 0) * Time.deltaTime;
        else
        {
            moneyRect.localPosition = new Vector3(moneyRect.localPosition.x, startMoneyPos.y, moneyRect.localPosition.z);
            return true;
        }

        return false;
    }

    bool MoveElementsEnd()
    {
        bool moneyEnd = false;
        bool elementsEnd = false;

        /*if (moneyRect.localPosition.y < endMoneyPos.y)
            moneyRect.localPosition += new Vector3(0, animSpeed, 0) * Time.deltaTime;
        else
        {
            moneyRect.localPosition = new Vector3(moneyRect.localPosition.x, endMoneyPos.y, moneyRect.localPosition.z);
            moneyEnd = true;
        }*/

        if (elementRect.localPosition.y > endElementPos.y)
            elementRect.localPosition -= new Vector3(0, animSpeed, 0) * Time.deltaTime;
        else
        {
            elementRect.localPosition = new Vector3(elementRect.localPosition.x, endElementPos.y, elementRect.localPosition.z);
            elementsEnd = true;
        }

        if (/*moneyEnd &&*/ elementsEnd)
            return true;
        else
            return false;
    }

    bool MoveElementsBackEnd()
    {
        bool moneyEnd = false;
        bool elementsEnd = false;

        /*if (moneyRect.localPosition.y > startMoneyPos.y)
            moneyRect.localPosition -= new Vector3(0, animSpeed, 0) * Time.deltaTime;
        else
        {
            moneyRect.localPosition = new Vector3(moneyRect.localPosition.x, startMoneyPos.y, moneyRect.localPosition.z);
            moneyEnd = true;
        }*/

        if (elementRect.localPosition.y < startElementPos.y)
            elementRect.localPosition += new Vector3(0, animSpeed, 0) * Time.deltaTime;
        else
        {
            elementRect.localPosition = new Vector3(elementRect.localPosition.x, startElementPos.y, elementRect.localPosition.z);
            elementsEnd = true;
        }

        if (/*moneyEnd && */elementsEnd)
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
            /*if (!hasAnimDownscale)
            {
                PlayAnimationsPartsBack();
                hasAnimDownscale = true;
            }*/

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
