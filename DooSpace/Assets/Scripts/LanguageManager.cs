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
    [SerializeField] Text txtWatchAds;
    [SerializeField] Text txtHowToPlay;
    [SerializeField] GameObject tutoScrollView;

    public string language = "";
    public string[] elementInfo;

    Text[] listTutoItemText;

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
        listTutoItemText = tutoScrollView.GetComponentsInChildren<Text>();
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

            txtWatchAds.text = "Regarder une pub pour argent x2";

            txtHowToPlay.text = "Comment jouer";

            listTutoItemText[0].text = "item 1 fr";
            listTutoItemText[1].text = "item 1 desc fr";

            listTutoItemText[2].text = "item 2 fr";
            listTutoItemText[3].text = "item 2 desc fr";

            listTutoItemText[4].text = "item 3 fr";
            listTutoItemText[5].text = "item 3 desc fr";

            listTutoItemText[6].text = "item 4 fr";
            listTutoItemText[7].text = "item 4 desc fr";

            listTutoItemText[8].text = "item 5 fr";
            listTutoItemText[9].text = "item 5 desc fr";
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

            txtWatchAds.text = "Watch ads for money x2";

            txtHowToPlay.text = "How to play";

            listTutoItemText[0].text = "item 1 en";
            listTutoItemText[1].text = "item 1 desc en";

            listTutoItemText[2].text = "item 2 en";
            listTutoItemText[3].text = "item 2 desc en";

            listTutoItemText[4].text = "item 3 en";
            listTutoItemText[5].text = "item 3 desc en";

            listTutoItemText[6].text = "item 4 en";
            listTutoItemText[7].text = "item 4 desc en";

            listTutoItemText[8].text = "item 5 en";
            listTutoItemText[9].text = "item 5 desc en";
        }
	}
}
