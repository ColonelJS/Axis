using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager instance;

    [SerializeField] Text txtHighscore;
    [SerializeField] Text txtUpgrade;
    [SerializeField] Text txtHowToPlay;
    [SerializeField] GameObject tutoScrollView;
    [SerializeField] Text startText;
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
    [SerializeField] Text txtWatchAds;
    [SerializeField] Text insuffisentMoneyText;
    [SerializeField] Text reviveCostText;
    [SerializeField] Text moneyCostText;
    [SerializeField] Text adsMoneyLeftText;
    [Header("Highscore")]
    [SerializeField] List<Text> highscoreNameText;
    [SerializeField] Text highscoreNotRegisteredText;
    [SerializeField] Text highscoreLoadingText;
    [SerializeField] Text highscoreConnectToGoogleText;
    [SerializeField] Text highscoreButtonConnectToGoogleText;
    [Header("Credits")]
    [SerializeField] List<Text> creditsTitleText;
    [SerializeField] Text textPrivacy;
    [Header("Custom")]
    [SerializeField] Text topText;
    [SerializeField] Text bodyText;
    [SerializeField] Text wingsText;
    [Header("Player Info")]
    [SerializeField] Text rankText;
    [SerializeField] Text levelText;
    [SerializeField] Text achievementsText;
    [SerializeField] Text connectToGoogleText;
    [SerializeField] Text showLeaderBoardText;
    [SerializeField] Text gameVersionText;
    [Header("New Version PopUp")]
    [SerializeField] Text versionFoundText;
    [SerializeField] Text downloadVersionText;
    [Header("Support pass")]
    [SerializeField] Text passTitleText;
    [SerializeField] Text passDescText;
    [SerializeField] Text passButtonTitleText;
    [SerializeField] List<Text> passTitleElementsText;
    [SerializeField] List<Text> passDescElementsText;
    /*[Header("Christmas")]
    [SerializeField] Text christmasTitleText;
    [SerializeField] Text christmasDescText;*/

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
            elementInfo[2] = "Diminue le taux de carburant perdu au contact des météorites";
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

            txtUpgrade.text = "Améliorer pour :";
            txtUpgrade.fontSize = 44;

            startText.text = "Jouer";

            elementInfo[0] = "Augmente la vitesse de la fusée (+25%)";
            elementInfo[1] = "Diminue le taux de carburant consommé";
            elementInfo[2] = "Diminue le taux de carburant perdu au contact des météorites";

            enterNameTitle.text = "Entrez votre nom pour ajouter votre score au classement :";
            enterNamePlaceHolder.text = "Entrez votre nom ici...";
            resultsText.text = "Résultats :";
            alienHitText.text = "Extra-terrestre :";
            meteorHitText.text = "Météorites :";

            nextChestText.text = "Progression :";
            newChestText.text = "Nouveau coffre débloqué !";
            tapToText.text = "Appuyez pour ouvrir";

            txtWatchAds.text = "Regarder une pub pour:";
            insuffisentMoneyText.text = "Argent insuffisant !";
            reviveCostText.text = "Gratuit";
            moneyCostText.text = "Gratuit";
            adsMoneyLeftText.text = "Axius restant :";

            for (int i = 0; i < highscoreNameText.Count; i++)
            {
                if (highscoreNameText[i].text == "Empty")
                {
                    highscoreNameText[i].text = "Vide";
                    HighscoreManager.instance.SetScoreName(i, "Vide");
                }
            }

            txtHowToPlay.text = "Comment jouer ?";

            topText.text = "Tête";
            bodyText.text = "Corps";
            wingsText.text = "Ailes";

            creditsTitleText[0].text = "Programmeur / Art";
            creditsTitleText[1].text = "Musiques";
            creditsTitleText[2].text = "Remerciements spécial";
            creditsTitleText[3].text = "Réalisé pour";
            creditsTitleText[4].text = "développé sur";
            creditsTitleText[5].text = "Aucun extra-terrestre n'a était maltraité durant le développement !";
            creditsTitleText[6].text = "Crédits";
            textPrivacy.text = "politique de confidentialité";

            listTutoItemText[0].text = "Fusée";
            listTutoItemText[1].text = "La fusée est votre personnage que vous devez amener le plus haut possible. " +
                "Pour cela vous devez maintenir et glisser votre doigt de gauche à droite de l'ecran pour la déplacer horizontalement";

            listTutoItemText[2].text = "Astéroïde";
            listTutoItemText[3].text = "À leurs contact, Les astéroïdes vous ralentissent fortement dans votre course, vous devez donc les esquiver pour continuer de progresser. " +
                "Cela vous fait aussi perdre des points de score.";

            listTutoItemText[4].text = "Carburant";
            listTutoItemText[5].text = "Le carburant est l'objet principal du jeu, celui-ci doit impérativement etre ramassé pour remplir son réservoir et continuer de progresser. " +
                "Ceui-ci se consumme au fil de la partie. Vous pouvez remplir jusqu'a deux réservoirs de carburant maximum.";

            listTutoItemText[6].text = "Bouclier";
            listTutoItemText[7].text = "Le bouclier permet de se protéger des astéroïdes, pouvant ainsi vous sauver la vie. " +
                "Celui-ci disparait après un certain temps ou se détruit suite à une collision avec un astéroïde.";

            listTutoItemText[8].text = "Grand extra-terrestre";
            listTutoItemText[9].text = "Rentrer an collision avec un grand extra-terrestre vous apporte une petite somme d'argent en plus à la fin de la partie.";

            listTutoItemText[10].text = "Petit extra-terrestre";
            listTutoItemText[11].text = "Le petit extra-terrestre se trouve seulement par vagues en grand nombre et augmente votre score à la fin de la partie pour chaque extra-terrestre heurté.";

            listTutoItemText[12].text = "Trou noir";
            listTutoItemText[13].text = "Entrer dans un trou noir vous permet de passer à travers chaque astéroïdes pendant une durée limitée. " +
                "Durant ce temps là, les objets que vous rencontrez sont fortement attirés vers votre fusée";

            rankText.text = "Classement :";
            levelText.text = "Niveau :";
            achievementsText.text = "Succès :";
            connectToGoogleText.text = "Vous êtes hors ligne\nSe connecter à Google Play";
            showLeaderBoardText.text = "Ouvrir la liste des succès";
            gameVersionText.text = "Version du jeu :";

            versionFoundText.text = "Nouvelle version trouvée !";
            downloadVersionText.text = "Téléchargez-la ici :";

            highscoreNotRegisteredText.text = "Aucun score enregistré";
            highscoreLoadingText.text = "Chargement...";
            highscoreConnectToGoogleText.text = "Connectez-vous à Google Play pour voir le classement global !";
            highscoreButtonConnectToGoogleText.text = "Connexion";

            passTitleText.text = "Pass de soutien";
            passDescText.text = "Obtenez le nouveau pass de soutien et gagnez de nombreux cadeaux et récompenses";
            passButtonTitleText.text = "Obtenez le pass de soutien maintenant !";

            passTitleElementsText[0].text = "Coffre x8";
            passTitleElementsText[1].text = "Unique Skins";
            passTitleElementsText[2].text = "Axius x20.000";
            passTitleElementsText[3].text = "Plus de pubs";
            passTitleElementsText[4].text = "Nom dorée au classement";

            passDescElementsText[0].text = "Vous permet d'ouvrir plus de 8 coffres spéciaux quand vous le voulez";
            passDescElementsText[1].text = "Débloquez de nouveaux skins unique";
            passDescElementsText[2].text = "Gagnez 20.000 Axius à dépenser comme vous le voulez";
            passDescElementsText[3].text = "Gagnez les récompenses des pubs sans même les regarder";
            passDescElementsText[4].text = "Distinguez-vous de vos amis, votre nom apparait désormais en dorée dans le classement global";

            /*christmasTitleText.text = "Joyeux Noël !";
            christmasTitleText.fontSize = 85;
            christmasDescText.text = "Profitez d'un bonus expérience x2 pour la période de noël!";*/
        }
        else if(language == "en")
		{
            txtHighscore.text = "HIGHSCORE";

            txtUpgrade.text = "Upgrade for :";
            txtUpgrade.fontSize = 54;

            startText.text = "Start";

            elementInfo[0] = "Increases the speed of the rocket (+25%)"; //fuel
            elementInfo[1] = "Decreases the amount of fuel consumed"; //wing
            elementInfo[2] = "Decreases the amount of fuel lost due to meteorites"; //bumper

            enterNameTitle.text = "Enter your name to add your score to the ranking :";
            enterNamePlaceHolder.text = "Enter name here...";
            resultsText.text = "Results :";
            alienHitText.text = "Alien hit :";
            meteorHitText.text = "Meteor-hit :";

            nextChestText.text = "Unlock progress :";
            newChestText.text = "New chest unlocked !";
            tapToText.text = "Tap to open";

            txtWatchAds.text = "Watch ads for:";
            insuffisentMoneyText.text = "Insufficient money !";
            reviveCostText.text = "Free";
            moneyCostText.text = "Free";
            adsMoneyLeftText.text = "Axius left :";

            for (int i = 0; i < highscoreNameText.Count; i++)
            {
                if (highscoreNameText[i].text == "Vide")
                {
                    highscoreNameText[i].text = "Empty";
                    HighscoreManager.instance.SetScoreName(i, "Empty");
                }
            }

            txtHowToPlay.text = "How to play ?";
            topText.text = "Top";
            bodyText.text = "Body";
            wingsText.text = "Wings";

            creditsTitleText[0].text = "Programmer / Art";
            creditsTitleText[1].text = "Musics";
            creditsTitleText[2].text = "special thanks";
            creditsTitleText[3].text = "made for";
            creditsTitleText[4].text = "developed on";
            creditsTitleText[5].text = "Any alien were abused during development !";
            creditsTitleText[6].text = "Credits";
            textPrivacy.text = "privacy policy";

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

            rankText.text = "Rank :";
            levelText.text = "Level :";
            achievementsText.text = "Achievements :";
            connectToGoogleText.text = "You are offline\nConnect to Google play";
            showLeaderBoardText.text = "Open achievement list";
            gameVersionText.text = "Game version :";

            versionFoundText.text = "New version found !";
            downloadVersionText.text = "Download it here :";

            highscoreNotRegisteredText.text = "No score registered yet !";
            highscoreLoadingText.text = "Loading...";
            highscoreConnectToGoogleText.text = "Connect to Google Play to see the global ranking !";
            highscoreButtonConnectToGoogleText.text = "Connection";

            passTitleText.text = "Support pass";
            passDescText.text = "Get the new Axis support pass and win a lot of gift & rewards !";
            passButtonTitleText.text = "Get the Support Pass now !";

            passTitleElementsText[0].text = "chest x8";
            passTitleElementsText[1].text = "Unique Skins";
            passTitleElementsText[2].text = "Axius x20.000";
            passTitleElementsText[3].text = "No more ads";
            passTitleElementsText[4].text = "Golden rank name";

            passDescElementsText[0].text = "Allow you to open up to 8 chest whenever you want";
            passDescElementsText[1].text = "Unlock unique skins ";
            passDescElementsText[2].text = "Gain 20.000 Axius to spend as you want";
            passDescElementsText[3].text = "win ads rewards without even watching them";
            passDescElementsText[4].text = "Stand out from your friends with a unique color for your nickname which now appears in gold";

            /*christmasTitleText.text = "Merry Christmas !";
            christmasTitleText.fontSize = 78;
            christmasDescText.text = "Enjoy a bonus of x2 experience for the Christmas period!";*/
        }
	}
}
