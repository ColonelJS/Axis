using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestPopUp : MonoBehaviour
{
    [SerializeField] private GameObject popUp;
    [SerializeField] private Image progressBar;
    [SerializeField] private GameObject menuButton;
    [SerializeField] private GameObject chest;
    [SerializeField] private GameObject chestImg;
    [SerializeField] private GameObject item;
    [SerializeField] private GameObject itemEndPos;
    [SerializeField] private GameObject itemNameGo;
    [SerializeField] private Text itemNameText;
    [SerializeField] private Image itemImg;
    [SerializeField] private GameObject textTapToGo;
    [SerializeField] private GameObject chestScreen;
    [SerializeField] private GameObject buttonCloseScreen;
    [SerializeField] private ScoreScreen scoreScreen;
    [SerializeField] private GameObject chestStatic;
    [SerializeField] private GameObject chestAnim;

    bool isProgress = false;
    bool isProgressBarEnd = false;
    bool isOpenChest = false;
    bool chestReady = false;
    bool chestOpen = false;
    bool chestAnimStart = false;
    bool isSetValue = false;
    float progressBarSpeed = 0.8f;
    float chestAnimSpeed = 1.8f;
    float itemSpeed = 62f;
    float cooldownChestAnim = 0.5f;

    float xpEarned;
    int currentXp;
    int lastCurrentXp = 0;
    float xpTotal;
    float nextLevelXpNeed;

    float xpEarnedNormalized;
    float currentXpNormalized;
    float xpTotalNormalized;

    Vector3 startItemPos;
    Sprite baseItemSprite;
    bool isItemWinSetup = false;

    void Start()
    {
        item.SetActive(false);
        chest.SetActive(false);
        chestScreen.SetActive(false);
        buttonCloseScreen.SetActive(false);
        ClosePopUp();
        startItemPos = item.transform.position;
        baseItemSprite = itemImg.sprite;
    }

    void Update()
    {
        if (!isOpenChest)
        {
            if (!isProgressBarEnd)
            {
                if (isSetValue)
                {
                    SetValue();
                    isSetValue = false;
                }
                else
                if (isProgress)
                    UpdateProgressBar();
            }
            else
            {
                menuButton.SetActive(true);
            }
        }
        else
		{
            if(!chestReady)
			{
                if (chestImg.transform.localScale.x < 1)
                    chestImg.transform.localScale += new Vector3(chestAnimSpeed, chestAnimSpeed, chestAnimSpeed) * Time.deltaTime;
                else
                {
                    chestImg.transform.localScale = new Vector3(1f, 1f, 1f);
                    chestReady = true;
                }
            }
			else
			{
                if(chestAnimStart)
				{
                    if (!chestOpen)
                    {
                        if (cooldownChestAnim <= 0)
                        {
                            OpenChest();
                            cooldownChestAnim = 0.5f;
                        }
                        else
                            cooldownChestAnim -= Time.deltaTime;
                    }
                    else
                        UpdateItemAnimation();
                }
			}
        }
    }

    public void ResetChestScale()
	{
        chestImg.transform.localScale = new Vector3(0f, 0f, 0f);
    }

	void UpdateProgressBar()
	{
        float fillAmout;
        if (xpTotalNormalized >= 1)
            fillAmout = 1;
        else
            fillAmout = xpTotalNormalized;

        if (progressBar.fillAmount < fillAmout)
            progressBar.fillAmount += progressBarSpeed * Time.deltaTime;
        else
        {
            progressBar.fillAmount = fillAmout;
            if (xpTotalNormalized >= 1)
			{
                CreateChest();
                xpTotal = xpTotal - nextLevelXpNeed;
                xpTotalNormalized = xpTotal / nextLevelXpNeed;
                xpEarned -= nextLevelXpNeed;
                progressBar.fillAmount = 0;
                lastCurrentXp = currentXp;
                currentXp = 0;

                CharacterManager.instance.IncrementPlayerChestLevel();
                UpdateNextLevelXpNeed();
            }
			else
			{
                currentXp = (int)xpTotal;
                isProgressBarEnd = true;
                isProgress = false;
            }
            ZPlayerPrefs.SetInt("currentXp", currentXp);
        }
    }

    public void SetIsSetValue()
	{
        isSetValue = true;
	}

    public void SetXpEarned(float _value)
	{
        xpEarned = _value;
	}

    void SetValue()
	{
        UpdateNextLevelXpNeed();

        currentXp = ZPlayerPrefs.GetInt("currentXp", 0);
        xpTotal = currentXp + xpEarned;
        SetValueNormalized();
    }

    void UpdateNextLevelXpNeed()
	{
        float a = 2;
        float b = 2700;
        float c = 2f;
        int x = CharacterManager.instance.GetPlayerChestLevel() + 9;

        //xp cost limit
        if (x > 46)
            x = 46;
        float levelXpCurve = a * Mathf.Pow(x, c) + b + a;
        nextLevelXpNeed = (int)levelXpCurve;
    }

    void SetValueNormalized()
	{
        currentXpNormalized = (float)currentXp / nextLevelXpNeed;
        progressBar.fillAmount = currentXpNormalized;
        xpTotalNormalized = xpTotal / nextLevelXpNeed;
        xpEarnedNormalized = xpEarned / nextLevelXpNeed;
    }

    void CreateChest()
	{
        isOpenChest = true;
        chestScreen.SetActive(true);
        chest.SetActive(true);
    }

    public void OpenChest()
	{
        item.SetActive(true);
        chestOpen = true;
        textTapToGo.SetActive(false);

        //hype beast
        GooglePlayServicesManager.instance.incrementSucces("CgkI6LzEr7kGEAIQDA", 1); //ACHIEVEMENT 4  ///succes steps : 5
        GooglePlayServicesManager.instance.incrementSucces("CgkI6LzEr7kGEAIQDQ", 1); //ACHIEVEMENT 4.2 ///succes steps : 15
        GooglePlayServicesManager.instance.incrementSucces("CgkI6LzEr7kGEAIQDg", 1); //ACHIEVEMENT 4.3 ///succes steps : 30
    }

    public void PlayChestOpen()
	{
        chestAnimStart = true;
        chestStatic.SetActive(false);
        chestAnim.SetActive(true);
	}

    void UpdateItemAnimation()
	{
        item.SetActive(true);
        if (item.transform.position.y < itemEndPos.transform.position.y)
            item.transform.position += new Vector3(0, itemSpeed, 0) * Time.deltaTime;
        else
        {
            if (!isItemWinSetup)
            {
                item.transform.position = itemEndPos.transform.position;
                itemNameGo.SetActive(true);

                Skin newSkin = SkinManager.instance.GetListSkinsOrdered()[SkinManager.instance.GetCurrentSkinIndexToOpen()];
                if (PlayerPrefs.GetString("language") == "fr")
                    itemNameText.text = newSkin.skinNameFr;
                else
                    itemNameText.text = newSkin.skinName;
                itemImg.sprite = newSkin.sprite;

                buttonCloseScreen.SetActive(true);

                SkinManager.instance.AddSkinToInventory(newSkin.index);
                isItemWinSetup = true;
            }
        }
    }

    public void OpenPopUp()
    {
        popUp.SetActive(true);
        isProgress = true;
    }

    public void ClosePopUp()
    {
        popUp.SetActive(false);
    }

    public void CloseChestScreen()
	{
        ResetChestScreen();
    }

    void ResetChestScreen()
	{
        if (PlayerPrefs.GetString("language") == "fr")
            itemNameText.text = "Progression :";
        else
            itemNameText.text = "Unlock progress :";

        item.transform.position = new Vector3(itemEndPos.transform.position.x, startItemPos.y, startItemPos.z);
        itemImg.sprite = baseItemSprite;
        textTapToGo.SetActive(true);
        item.SetActive(false);
        chest.SetActive(false);
        chestScreen.SetActive(false);
        buttonCloseScreen.SetActive(false);
        ClosePopUp();

        chestReady = false;
        chestOpen = false;
        chestAnimStart = false;
        chestStatic.SetActive(true);
        chestAnim.SetActive(false);
        isOpenChest = false;
        isItemWinSetup = false;
        chestImg.transform.localScale = new Vector3(0, 0, 0);
        scoreScreen.ResetChestScreen();
    }

    public float GetNextLevelXpNeed()
	{
        return nextLevelXpNeed;
	}

    public int GetLastCurrentXp()
	{
        return lastCurrentXp;
	}
}
