using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomScreen : MonoBehaviour
{
    public static CustomScreen instance;

    [SerializeField] private Text moneyText;

    [SerializeField] private Image fuelLevelImg;
    [SerializeField] private Image wingLevelImg;
    [SerializeField] private Image bumperLevelImg;

    [SerializeField] private GameObject popUpValidate;
    [SerializeField] private GameObject popUpInfo;
    [SerializeField] private Text costValidateText;
    [SerializeField] private Text elementInfoText;

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
    string[] elementInfo;

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
        SetupElementInfo();
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
    }

    public int GetBumperLevel()
	{
        return bumperLevel;
	}

    public int GetWingLevel()
    {
        return wingLevel;
    }

    public int GetFuelLevel()
    {
        return fuelLevel;
    }

    void SetupElementInfo()
	{
        elementInfo[0] = "increases the speed of the rocket (+25%)"; //fuel
        elementInfo[1] = "decreases the amount of fuel consumed"; //wing
        elementInfo[2] = "decreases the amount of fuel lost due to meteorites"; //bumper
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
        fuelLevelImg.fillAmount = fuelLevel / levelMax;

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
            SoundManager.instance.CloseSlider();
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
            fuelLevelImg.fillAmount = floatLevel / levelMax;
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
        elementInfoText.text = elementInfo[_strIndex];
	}
}
