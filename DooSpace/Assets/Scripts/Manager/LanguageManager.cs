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
    [Header("Credits")]
    [SerializeField] List<Text> creditsTitleText;

    public string language = "";
    public string[] elementInfo;

    //Text[] listTutoItemText;

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
        //listTutoItemText = tutoScrollView.GetComponentsInChildren<Text>();
        if (language == "fr")
        {
            elementInfo[0] = "Augmente la vitesse de la fus�e (+25%)";
            elementInfo[1] = "Diminue le taux de carburant consomm�";
            elementInfo[2] = "Diminue le taux de carburant perdu au contacte des m�t�orites";
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

            txtUpgrade.text = "Am�liorer pour :";
            txtUpgrade.fontSize = 44;

            elementInfo[0] = "Augmente la vitesse de la fus�e (+25%)";
            elementInfo[1] = "Diminue le taux de carburant consomm�";
            elementInfo[2] = "Diminue le taux de carburant perdu au contacte des m�t�orites";

            txtWatchAds.text = "Regarder une pub pour argent x2";

            txtHowToPlay.text = "Comment jouer";

            creditsTitleText[0].text = "Programmeur / Art";
            creditsTitleText[1].text = "Musiques";
            creditsTitleText[2].text = "Remerciements sp�cial";
            creditsTitleText[3].text = "R�alis� pour";
            creditsTitleText[4].text = "d�velopp� sur";
            creditsTitleText[5].text = "Aucun extra-terrestre n'a �tait maltrait� durant le d�veloppement !";

            /*listTutoItemText[0].text = "item 1 fr";
            listTutoItemText[1].text = "item 1 desc fr";

            listTutoItemText[2].text = "item 2 fr";
            listTutoItemText[3].text = "item 2 desc fr";

            listTutoItemText[4].text = "item 3 fr";
            listTutoItemText[5].text = "item 3 desc fr";

            listTutoItemText[6].text = "item 4 fr";
            listTutoItemText[7].text = "item 4 desc fr";

            listTutoItemText[8].text = "item 5 fr";
            listTutoItemText[9].text = "item 5 desc fr";*/
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

            creditsTitleText[0].text = "Programmer / Art";
            creditsTitleText[1].text = "Musics";
            creditsTitleText[2].text = "special thanks";
            creditsTitleText[3].text = "made for";
            creditsTitleText[4].text = "developed on";
            creditsTitleText[5].text = "Any alien were abused during development !";

            /*listTutoItemText[0].text = "item 1 en";
            listTutoItemText[1].text = "item 1 desc en";

            listTutoItemText[2].text = "item 2 en";
            listTutoItemText[3].text = "item 2 desc en";

            listTutoItemText[4].text = "item 3 en";
            listTutoItemText[5].text = "item 3 desc en";

            listTutoItemText[6].text = "item 4 en";
            listTutoItemText[7].text = "item 4 desc en";

            listTutoItemText[8].text = "item 5 en";
            listTutoItemText[9].text = "item 5 desc en";*/
        }
	}
}
