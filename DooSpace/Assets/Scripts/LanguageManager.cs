using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager instance;

    [SerializeField] Text txtHighscore;
    [SerializeField] Text txtPlay;

    public string language = "";
    public string[] elementInfo;

    private void Awake()
	{
        if (instance == null)
        {
            instance = this;
            SetupElementInfo();
            SetGameLanguage();
        }
	}

	void Start()
    {      
        //SetGameLanguage();
    }

    void Update()
    {
        
    }

    void SetGameLanguage()
    {
        if (PlayerPrefs.HasKey("language"))
        {
            //Debug.Log("language : " + PlayerPrefs.GetString("language"));
            if (PlayerPrefs.GetString("language") == "fr")
                language = "fr";
            else
                language = "en";
        }
        else
        {
            if (Application.systemLanguage == SystemLanguage.French)
            {
                PlayerPrefs.SetString("language", "fr");
                language = "fr";
            }
            else
            {
                PlayerPrefs.SetString("language", "en");
                language = "en";
            }
        }

        //SetupElementInfo();
        UpdateLanguageText();
    }

    public string GetLanguage()
	{
        return language;
	}

    void SetupElementInfo()
    {
        elementInfo = new string[3];
        if (language == "fr")
        {
            elementInfo[0] = "augmente la vitesse de la fusée (+25%)";
            elementInfo[1] = "diminue le taux de carburant consommé";
            elementInfo[2] = "diminue le taux de carburant perdu au contacte des météorites";
        }
        else
        {
            elementInfo[0] = "increases the speed of the rocket (+25%)"; //fuel
            elementInfo[1] = "decreases the amount of fuel consumed"; //wing
            elementInfo[2] = "decreases the amount of fuel lost due to meteorites"; //bumper
        }
    }

    public void UpdateLanguageText()
	{
        language = PlayerPrefs.GetString("language");

        if(language == "fr")
		{
            txtHighscore.text = "CLASSEMENT";
            txtPlay.text = "Appuyez pour commencer";
            txtPlay.fontSize = 125;
            elementInfo[0] = "augmente la vitesse de la fusée (+25%)";
            elementInfo[1] = "diminue le taux de carburant consommé";
            elementInfo[2] = "diminue le taux de carburant perdu au contacte des météorites";

            //PlayerPrefs.SetString("language", "fr");
        }
        else if(language == "en")
		{
            txtHighscore.text = "HIGHSCORE";
            txtPlay.text = "Tap to play";
            txtPlay.fontSize = 187;
            elementInfo[0] = "increases the speed of the rocket (+25%)"; //fuel
            elementInfo[1] = "decreases the amount of fuel consumed"; //wing
            elementInfo[2] = "decreases the amount of fuel lost due to meteorites"; //bumper

            //PlayerPrefs.SetString("language", "en");
        }
	}
}
