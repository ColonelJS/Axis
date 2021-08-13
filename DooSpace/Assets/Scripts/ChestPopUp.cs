using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestPopUp : MonoBehaviour
{
    [SerializeField] private GameObject popUp;
    [SerializeField] private Image progressBar;
    [SerializeField] private GameObject menuButton;

    bool isProgress = false;
    bool isProgressBarEnd = false;
    bool valueSet = false;
    bool isSetValue = false;
    float progressBarSpeed = 1f;

    float xpEarned;
    int currentXp;
    float xpTotal;
    float nextLevelXpNeed;

    float xpEarnedNormalized;
    float currentXpNormalized;
    float xpTotalNormalized;

    void Start()
    {
        ClosePopUp();
    }

    void Update()
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
            Debug.Log("current xp set to playerprefs : " + currentXp);
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
        Debug.Log("percent of : current xp : " + currentXpNormalized + ", xp earned : " + xpEarnedNormalized + ", xp total : " + xpTotalNormalized);
    }

    void CreateChest()
	{

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
}
