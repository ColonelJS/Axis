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
    [SerializeField] private GameObject textTapToGo;
    [SerializeField] private GameObject chestScreen;
    [SerializeField] private GameObject buttonCloseScreen;

    bool isProgress = false;
    bool isProgressBarEnd = false;
    bool isOpenChest = false;
    bool chestReady = false;
    bool chestOpen = false;
    bool isSetValue = false;
    float progressBarSpeed = 1f;
    float chestAnimSpeed = 2.5f;
    float itemSpeed = 60f;

    float xpEarned;
    int currentXp;
    float xpTotal;
    float nextLevelXpNeed;

    float xpEarnedNormalized;
    float currentXpNormalized;
    float xpTotalNormalized;

    void Start()
    {
        item.SetActive(false);
        chest.SetActive(false);
        chestScreen.SetActive(false);
        buttonCloseScreen.SetActive(false);
        ClosePopUp();
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
                Debug.Log("chest img scale : " + chestImg.transform.localScale);
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
            currentXp = (int)xpTotal;

            if (xpTotalNormalized >= 1)
			{
                CreateChest();
                xpTotal = xpTotal - nextLevelXpNeed;
                xpTotalNormalized = xpTotal / nextLevelXpNeed;

                //currentXp = (int)xpTotal;
                progressBar.fillAmount = 0;
            }
			else
			{
                //currentXp = (int)xpTotal;
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
	}

    void SetValue()
	{
        nextLevelXpNeed = 1000;//courbe
        currentXp = PlayerPrefs.GetInt("currentXp", 0);
        xpTotal = currentXp + xpEarned;
        Debug.Log("current xp : " + currentXp + ", xp earned : " + xpEarned + ", xp total : " + xpTotal);
        SetValueNormalized();
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
        Debug.Log("is create chest");
        isOpenChest = true;
        chestScreen.SetActive(true);
        chest.SetActive(true);
    }

    public void OpenChest()
	{
        item.SetActive(true);
        chestOpen = true;
        textTapToGo.SetActive(false);
        //int randValue = Random.Range(0, )
    }

    void UpdateItemAnimation()
	{
        if (item.transform.position.y < itemEndPos.transform.position.y)
            item.transform.position += new Vector3(0, itemSpeed, 0) * Time.deltaTime;
        else
        {
            item.transform.position = itemEndPos.transform.position;
            itemNameGo.SetActive(true);
            itemNameText.text = "itemName";
            buttonCloseScreen.SetActive(true);
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
        chestScreen.SetActive(false);
        isOpenChest = false;
    }

    void ResetChestScreen()
	{

	}
}
