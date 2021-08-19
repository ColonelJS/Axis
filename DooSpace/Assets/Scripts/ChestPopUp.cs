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

    bool isProgress = false;
    bool isProgressBarEnd = false;
    bool isOpenChest = false;
    bool chestReady = false;
    bool chestOpen = false;
    bool isSetValue = false;
    float progressBarSpeed = 0.8f;
    float chestAnimSpeed = 1.8f;
    float itemSpeed = 62f;

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
        //Debug.Log("start current xp : " + PlayerPrefs.GetInt("currentXp", 0));
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
                if(chestOpen)
                    UpdateItemAnimation();
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
                //if (xpEarned < 0)
                    //xpEarned = 0;
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
            PlayerPrefs.SetInt("currentXp", currentXp);
        }
    }

    public void SetIsSetValue()
	{
        isSetValue = true;
	}

    public void SetXpEarned(float _value)
	{
        xpEarned = _value;
        //if (xpEarned < 0)
            //xpEarned = 0;
	}

    void SetValue()
	{
        UpdateNextLevelXpNeed();

        currentXp = PlayerPrefs.GetInt("currentXp", 0);
        xpTotal = currentXp + xpEarned;
        //Debug.Log("current xp : " + currentXp + ", xp earned : " + xpEarned + ", xp total : " + xpTotal);
        SetValueNormalized();
    }

    void UpdateNextLevelXpNeed()
	{
        float a = 2;
        float b = 2000;
        float c = 2.1f;
        int x = CharacterManager.instance.GetPlayerChestLevel();

        //xp cost limit
        if (x > 45)
            x = 45;
        float levelXpCurve = a * Mathf.Pow(x, c) + b + a;
        nextLevelXpNeed = (int)levelXpCurve;
        //nextLevelXpNeed = 100;
        //Debug.Log("next level xp need : " + nextLevelXpNeed);
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

                Skin newSkin = SkinManager.instance.GetListSkin()[SkinManager.instance.GetCurrentSkinIndexToOpen()];
                itemNameText.text = newSkin.skinName;
                itemImg.sprite = newSkin.sprite;

                buttonCloseScreen.SetActive(true);

                SkinManager.instance.AddSkinToInventory(newSkin.index);
                SkinManager.instance.IncrementCurrentSkinIndex();
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
