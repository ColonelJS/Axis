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
		}
        else if(language == "en")
		{
            txtHighscore.text = "HIGHSCORE";
        }
	}
}
