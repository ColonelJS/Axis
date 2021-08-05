using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager instance;

    [SerializeField] Text txtHighscore;

    string language = "";

    private void Awake()
	{
        if (instance == null)
            instance = this;
	}

	void Start()
    {
        SetGameLanguage();
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
    }

    public string GetLanguage()
	{
        return language;
	}

    public void UpdateLanguageText()
	{
        language = PlayerPrefs.GetString("language");

        if(language == "fr")
		{
            txtHighscore.text = "CLASSEMENT";
            CustomScreen.instance.elementInfo[0] = "augmente la vitesse de la fusée (+25%)";
            CustomScreen.instance.elementInfo[1] = "diminue le taux de carburant consommé";
            CustomScreen.instance.elementInfo[2] = "diminue le taux de carburant perdu au contacte des météorites";
        }
        else if(language == "en")
		{
            txtHighscore.text = "HIGHSCORE";
            CustomScreen.instance.elementInfo[0] = "increases the speed of the rocket (+25%)"; //fuel
            CustomScreen.instance.elementInfo[1] = "decreases the amount of fuel consumed"; //wing
            CustomScreen.instance.elementInfo[2] = "decreases the amount of fuel lost due to meteorites"; //bumper
        }
	}
}
