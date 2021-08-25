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
    [Header("ScoreScreen")]
    [SerializeField] Text enterNameTitle;
    [SerializeField] Text enterNamePlaceHolder;
    [Space(6)]
    [SerializeField] Text resultsText;
    [SerializeField] Text alienHitText;
    [SerializeField] Text meteorHitText;
    [Header("ChestScreen")]
    [SerializeField] Text nextChestText;
    [SerializeField] Text newChestText;
    [SerializeField] Text tapToText;
    [Header("AdsPopUp")]
    [SerializeField] Text insuffisentMoneyText;
    [SerializeField] Text reviveText;
    [SerializeField] Text reviveCostText;
    [SerializeField] Text moneyText;
    [SerializeField] Text moneyCostText;
    [Header("Credits")]
    [SerializeField] List<Text> creditsTitleText;

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

            enterNameTitle.text = "Entrez votre nom pour ajouter votre score au classement :";
            enterNamePlaceHolder.text = "Entrez votre nom ici...";
            resultsText.text = "R�sultats :";
            //alienHitText.text = "Extra-terrestre heurt� :";
            //meteorHitText.text = "M�t�orites heurt� :";
            alienHitText.text = "Extra-terrestre :";
            meteorHitText.text = "M�t�orites :";

            nextChestText.text = "Progression d�verouillage du prochain coffre";
            nextChestText.fontSize = 65;
            newChestText.text = "Nouveau coffre d�bloqu� !";
            tapToText.text = "Appuyez pour ouvrir";

            txtWatchAds.text = "Regarder une pub pour:";
            insuffisentMoneyText.text = "Argent insuffisant !";
            reviveText.text = "Revivre";
            reviveCostText.text = "Gratuit";
            moneyText.text = "Argent x2";
            moneyCostText.text = "Gratuit";

            txtHowToPlay.text = "Comment jouer";

            creditsTitleText[0].text = "Programmeur / Art";
            creditsTitleText[1].text = "Musiques";
            creditsTitleText[2].text = "Remerciements sp�cial";
            creditsTitleText[3].text = "R�alis� pour";
            creditsTitleText[4].text = "d�velopp� sur";
            creditsTitleText[5].text = "Aucun extra-terrestre n'a �tait maltrait� durant le d�veloppement !";

            listTutoItemText[0].text = "Fus�e";
            listTutoItemText[1].text = "La fus�e est votre personnage que vous devez amener le plus haut possible. " +
                "Pour cela vous devez maintenir et glisser votre doigt de gauche � droite de l'ecran pour la d�placer horizontalement";

            listTutoItemText[2].text = "Ast�ro�de";
            listTutoItemText[3].text = "� leurs contacte, Les ast�ro�des vous ralentissent fortement dans votre course, vous devez donc les esquiver pour continuer de progresser. " +
                "Cela vous fait aussi perdre des points de score.";

            listTutoItemText[4].text = "Carburant";
            listTutoItemText[5].text = "Le carburant est l'objet principal du jeu, celui-ci doit imp�rativement etre ramass� pour remplir son r�servoir et continuer de progresser. " +
                "Ceui-ci se consumme au fil de la partie. Vous pouvez remplir jusqu'a deux r�servoirs de carburant maximum.";

            listTutoItemText[6].text = "Bouclier";
            listTutoItemText[7].text = "Le bouclier permet de se prot�ger des ast�ro�des, pouvant ainsi vous sauver la vie. " +
                "Celui-ci disparait apr�s un certain temps ou se d�truit suite � une collision avec un ast�ro�de.";

            listTutoItemText[8].text = "Grand extra-terrestre";
            listTutoItemText[9].text = "Rentrer an collision avec un grand extra-terrestre vous apporte une petite somme d'argent en plus � la fin de la partie.";

            listTutoItemText[10].text = "Petit extra-terrestre";
            listTutoItemText[11].text = "Le petit extra-terrestre se trouve seulement par vagues en grand nombre et augmente votre score � la fin de la partie pour chaque extra-terrestre heurt�.";

            listTutoItemText[12].text = "Trou noir";
            listTutoItemText[13].text = "Entrer dans un trou noir vous permet de passer � travers � travers chaque ast�ro�des pendant une dur�e limit�e. " +
                "Durant ce temps l�, les objets que vous rencontrez sont fortement attir�s vers votre fus�e";
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

            enterNameTitle.text = "Enter your name to add your score to the ranking :";
            enterNamePlaceHolder.text = "Enter name here...";
            resultsText.text = "Results :";
            alienHitText.text = "Alien hit :";
            meteorHitText.text = "Meteor-hit :";

            nextChestText.text = "Next chest\nunlock progress ";
            nextChestText.fontSize = 80;
            newChestText.text = "New chest unlocked !";
            tapToText.text = "Tap to open";

            txtWatchAds.text = "Watch ads for:";
            insuffisentMoneyText.text = "Insuffisent money !";
            reviveText.text = "Revive";
            reviveCostText.text = "Free";
            moneyText.text = "Money x2";
            moneyCostText.text = "Free";

            txtHowToPlay.text = "How to play";

            creditsTitleText[0].text = "Programmer / Art";
            creditsTitleText[1].text = "Musics";
            creditsTitleText[2].text = "special thanks";
            creditsTitleText[3].text = "made for";
            creditsTitleText[4].text = "developed on";
            creditsTitleText[5].text = "Any alien were abused during development !";

            listTutoItemText[0].text = "Rocket";
            listTutoItemText[1].text = "The rocket is your character that you need to get the highest possible. " +
                "For this you must hold and slide your finger from left to right of your screen to move it horizontally";

            listTutoItemText[2].text = "asteroid";
            listTutoItemText[3].text = "In contact with them, asteroids greatly slow you down in your run, so you have to dodge them to keep progressing. It also makes you lose score points.";

            listTutoItemText[4].text = "Fuel";
            listTutoItemText[5].text = "The fuel is the main item, it must imperatively be picked up to fill its tank and keep moving. " +
                "This is consumed over the course of the game and you can fill up to two tanks of fuel maximum.";

            listTutoItemText[6].text = "Shield";
            listTutoItemText[7].text = "The shield protects you against asteroids, which can save your life. " +
                "This one disappears after a certain time or is destroyed following a collision with an asteroid";

            listTutoItemText[8].text = "Large alien";
            listTutoItemText[9].text = "Colliding with a large alien brings you a small amount of extra money at the end of the game.";

            listTutoItemText[10].text = "Small alien";
            listTutoItemText[11].text = "The little alien only appear in large waves and increases your score at the end of the game for each alien struck.";

            listTutoItemText[12].text = "Black hole";
            listTutoItemText[13].text = "Entering a black hole allows you to pass right through each asteroids for a limited time. " +
                "During this time, the objects you encounter are strongly attracted to your rocket";
        }
	}
}
