using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager instance;

    [SerializeField] Text txtHighscore;
    [SerializeField] Text txtPlay;
    [SerializeField] Text txtUpgrade;

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

    }

    void Update()
    {
        
    }

    void SetGameLanguage()
    {
        if (PlayerPrefs.HasKey("language"))
        {
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
            elementInfo[0] = "Augmente la vitesse de la fusée (+25%)";
            elementInfo[1] = "Diminue le taux de carburant consommé";
            elementInfo[2] = "Diminue le taux de carburant perdu au contacte des météorites";
        }
        else
        {
            elementInfo[0] = "Increases the speed of the rocket (+25%)"; //fuel
            elementInfo[1] = "Decreases the amount of fuel consumed"; //wing
            elementInfo[2] = "Decreases the amount of fuel lost due to meteorites"; //bumper
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

            txtUpgrade.text = "Améliorer pour :";
            txtUpgrade.fontSize = 44;

            elementInfo[0] = "Augmente la vitesse de la fusée (+25%)";
            elementInfo[1] = "Diminue le taux de carburant consommé";
            elementInfo[2] = "Diminue le taux de carburant perdu au contacte des météorites";
        }
        else if(language == "en")
		{
            txtHighscore.text = "HIGHSCORE";

            txtPlay.text = "Tap to play";
            txtPlay.fontSize = 187;

            txtUpgrade.text = "Upgrade for :";
            txtUpgrade.fontSize = 54;

            elementInfo[0] = "Increases the speed of the rocket (+25%)"; //fuel
            elementInfo[1] = "Decreases the amount of fuel consumed"; //wing
            elementInfo[2] = "Decreases the amount of fuel lost due to meteorites"; //bumper
        }
	}
}
