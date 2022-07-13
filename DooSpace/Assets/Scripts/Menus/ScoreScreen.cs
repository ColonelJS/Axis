using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScreen : MonoBehaviour
{
    [SerializeField] private Text distanceText;
    [SerializeField] private Text alienText;
    [SerializeField] private Text meteoriteText;
    [SerializeField] private Text totalText;

    [SerializeField] private Text scoreDistanceText;
    [SerializeField] private Text scoreAlienText;
    [SerializeField] private Text scoreMeteoriteText;
    [SerializeField] private Text scoreTotalText;
    [SerializeField] private Text moneyGainText;

    [SerializeField] private GameObject scoreScreen;
    [SerializeField] private GameObject menuButton;
    [SerializeField] private ChestPopUp chestPopUp;
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject moneyGainGo;

    [SerializeField] private GameObject enterName;
    [SerializeField] private InputField nameEnteredIF;

    [SerializeField] bool isDoubleXp = false;

    float scoreScreenSpeed = 1550f;//1200

    int textIndex = 0;
    bool animationStart = true;
    bool animationEnd = false;
    bool scoreSetup = false;
    bool[] scoreEndDraw;
    bool scoreDrawed = false;
    bool isReverse = false;
    bool chestPopUpOpened = false;
    bool moneyGained = false;
    float cooldownAnimation = 0.75f;
    float cooldownChestPopUp = 1.2f;
    int cheatUsed = 0;
    int skinCheatIndex = 0;

    string[] oldName;
    int[] oldScore;

    int scoreDistBase = 0;
    int scoreAlienBase = 0;
    int scoreMeteoriteBase = 0;
    int scoreTotalBase = 0;

    int scoreDist;
    int scoreAlien;
    int scoreMeteorite;
    int scoreTotal;
    int moneyGain;

    string nameSaved = "";
    int scoreSaved = 0;
    int chestIndex = 0;
    int lastChestIndex = 0;

    int globalScore = 0;

    float xpEarnedLeft = 0;
    bool scoreSet = false;
    bool scoreSend = false;

    List<int> listScore = new List<int>();
    List<float> listScoreBase = new List<float>();
    List<GameObject> listText = new List<GameObject>();
    List<string> listScoreText = new List<string>();
    float debugXp = 0;

    void Start()
    {
        oldName = new string[9];
        oldScore = new int[9];
        enterName.SetActive(false);
        SetupValues();

        for (int i = 0; i < 4; i++)
            listText[i].SetActive(false);

        scoreEndDraw = new bool[5];
        for (int i = 0; i < 5; i++)
        {
            scoreEndDraw[i] = false;
        }
        scoreEndDraw[0] = true;
        menuButton.SetActive(false);
        moneyGainGo.SetActive(false);

        cheatUsed = ZPlayerPrefs.GetInt("cheatUsed", 0);

        skinCheatIndex = ZPlayerPrefs.GetInt("skinCheatIndex", 0);
        if(skinCheatIndex > 0)
            SkinManager.instance.SetSpecialSprite(skinCheatIndex-1);

        string lastNameEntered = PlayerPrefs.GetString("lastNameEntered", "");
        nameEnteredIF.text = lastNameEntered;
    }

    void Update()
    {
        if (GameManager.instance.GetGameState() == GameManager.GameState.SCORE && scoreScreen != null)
        {
            if (animationEnd)
            {
                if (!scoreDrawed)
                {
                    if (scoreEndDraw[textIndex])
                    {
                        if (cooldownAnimation <= 0)
                        {
                            DrawText(textIndex);
                            cooldownAnimation = 0.75f;
                            textIndex++;
                        }
                        else
                        {
                            if (Input.touches.Length > 0)
                            {
                                if (Input.GetTouch(0).phase == TouchPhase.Began)
                                    cooldownAnimation = 0;
                            }
                            else
                                cooldownAnimation -= Time.deltaTime;
                        }
                    }
                    else
                        UpdateValue(textIndex - 1);
                }
                else
				{
                    globalScore = int.Parse(scoreTotalText.text);

                    if (SkinManager.instance.GetNbSkinOwn() < SkinManager.instance.GetNbSkin())
                    {
                        if (cooldownChestPopUp <= 0)
                        {
                            if (!chestPopUpOpened)
                            {
                                if (!scoreSet)
                                {
                                    if(!isDoubleXp)
                                        xpEarnedLeft = int.Parse(scoreTotalText.text);
                                    else
                                        xpEarnedLeft = int.Parse(scoreTotalText.text) * 2;
                                    scoreSet = true;
                                    debugXp = xpEarnedLeft;
                                }

                                chestPopUp.OpenPopUp();
                                chestPopUpOpened = true;

                                if (chestIndex > lastChestIndex)
                                {
                                    xpEarnedLeft -= chestPopUp.GetNextLevelXpNeed() - chestPopUp.GetLastCurrentXp();
                                    if (xpEarnedLeft < 0)
                                        xpEarnedLeft = 0;
                                    lastChestIndex = chestIndex;
                                }
                                chestPopUp.SetXpEarned(xpEarnedLeft);
                                chestPopUp.SetIsSetValue();
                                chestIndex++;
                            }
                        }
                        else
                            cooldownChestPopUp -= Time.deltaTime;
                    }
                    else
                    {
                        OpenEnterName();
                    }
                 
                    if (!moneyGained)
                    {
                        moneyGainGo.SetActive(true);
                        SetMoneyGain();
                        moneyGained = true;

                        if (FireBaseAuthScript.instance.GetIsConnectedToGPGSAndFirebase())
                        {
                            if (!FireBaseAuthScript.instance.GetIsLocalPlayerScoreFind())
                            {
                                if (globalScore > 0)
                                {
                                    FireBaseAuthScript.instance.SendScoreToDatabase(globalScore);
                                    FireBaseAuthScript.instance.SendPlayerDataToDatabase();
                                }
                            }
                            else
                            {
                                int currentBestScore = FireBaseAuthScript.instance.GetCurrentPlayer().score.score;
                                if (globalScore > currentBestScore && globalScore != 0)
                                    FireBaseAuthScript.instance.SendScoreToDatabase(globalScore);
                            }
                        }
                    }
                }
            }
            else
            {
                if (animationStart)
                {
                    if (scoreSetup && !isReverse)
                        UpdateAnimation();
                    else
                        SetValues();
                }
            }
        }
    }

    public void OpenEnterName()
	{
        int score = int.Parse(scoreTotalText.text);
        if (score > 0)
        {
            chestPopUp.ClosePopUp();
            enterName.SetActive(true);
            SoundManager.instance.PlaySound("openEnterName");
        }
        else
            TransitionScreen.instance.SetTransitionStart();
    }

    public void ValidateName()
	{
        if (nameEnteredIF.text == "")
        {
            TransitionScreen.instance.SetTransitionStart();
        }
        else
        {
            enterName.SetActive(false);
            nameSaved = nameEnteredIF.text;
            scoreSaved = listScore[3];
            PlayerPrefs.SetString("lastNameEntered", nameSaved);

            if (nameSaved == "ilovemoney" && cheatUsed == 0)
            {
                int currentMoney = ZPlayerPrefs.GetInt("money");
                int newMoney = currentMoney + 2500;
                ZPlayerPrefs.SetInt("money", newMoney);
                cheatUsed = 1;
                ZPlayerPrefs.SetInt("cheatUsed", 1);
            }

            if (nameSaved == "axis" || nameSaved == "Axis")
            {
                skinCheatIndex = 0;
                ZPlayerPrefs.SetInt("skinCheatIndex", skinCheatIndex);
            }

            if (nameSaved == "yoshikage" || nameSaved == "Yoshikage" 
                || nameSaved == "yoshikagek" || nameSaved == "YoshikageK" || nameSaved == "yoshikageK" || nameSaved == "Yoshikagek")
            {
                skinCheatIndex = 1;
                ZPlayerPrefs.SetInt("skinCheatIndex", skinCheatIndex);
            }

            if(nameSaved == "ona" || nameSaved == "Ona" || nameSaved == "ONA")
            {
                skinCheatIndex = 2;
                ZPlayerPrefs.SetInt("skinCheatIndex", skinCheatIndex);
            }

            if (nameSaved == "nsfwpenis" || nameSaved == "NSFWPenis" || nameSaved == "NsfwPenis" || nameSaved == "NSFWpenis")
            {
                skinCheatIndex = 3;
                ZPlayerPrefs.SetInt("skinCheatIndex", skinCheatIndex);
            }

            if (nameSaved == "randomskin" || nameSaved == "RandomSkin" || nameSaved == "Randomskin" || nameSaved == "randomSkin")
            {
                skinCheatIndex = Random.Range(0, 4);
                ZPlayerPrefs.SetInt("skinCheatIndex", skinCheatIndex);
            }

            SetRankValue();
            TransitionScreen.instance.SetTransitionStart();
        }
    }

    void SetMoneyGain()
	{
        int newMoney = 0;    
        
        int currentMoney = CustomScreen.instance.GetPlayerMoney();

        if (GameManager.instance.GetDoubleCoinReward())
        {
            moneyGainText.text = "+" + moneyGain.ToString() + "(x2)";
            newMoney = currentMoney + moneyGain * 2;
        }
        else
        {
            moneyGainText.text = "+" + moneyGain.ToString();
            newMoney = currentMoney + moneyGain;
        }

        CustomScreen.instance.SetNewMoney(newMoney);

        if(GooglePlayServicesManager.instance.GetIsConnectedToGPGS())
        {
            int stepsSucces = moneyGain / 10;
            //wealthy
            GooglePlayServicesManager.instance.incrementSucces("CgkI6LzEr7kGEAIQBg", stepsSucces); //ACHIEVEMENT 2  ///succes steps : 100*2 //// : 100(*10)
            GooglePlayServicesManager.instance.incrementSucces("CgkI6LzEr7kGEAIQBw", stepsSucces); //ACHIEVEMENT 2.2 ///succes steps : 1000*2 //// : 1000(*10)
            GooglePlayServicesManager.instance.incrementSucces("CgkI6LzEr7kGEAIQCA", stepsSucces); //ACHIEVEMENT 2.3 ///succes steps : 10000*2 ////New : 5000(*10)

            //if (FireBaseAuthScript.instance.GetIsConnectedToFireBase())
                //FireBaseAuthScript.instance.SendPlayerMoneyData(newMoney);
        }
    }

    void SetRankValue()
	{
        for (int i = 0; i < 9; i++)
        {
            if (SaveManager.instance.GetScore(i) < scoreSaved)
            {
                for (int z = 0; z < 9; z++)
                {
                    oldName[z] = SaveManager.instance.GetName(z);
                    oldScore[z] = SaveManager.instance.GetScore(z);
                }

                SaveManager.instance.SetValue(i, nameSaved, scoreSaved);
                HighscoreManager.instance.UpdateHighscore(i, nameSaved, scoreSaved);

                for (int y = i+1; y < 9; y++)
				{
                    SaveManager.instance.SetValue(y, oldName[y-1], oldScore[y-1]);
                    HighscoreManager.instance.UpdateHighscore(y, SaveManager.instance.GetName(y), SaveManager.instance.GetScore(y));
                }
                SaveManager.instance.SaveGame();
                break;
            }
        }
    }

    void SetValues()
    {
        scoreDist = (int)CharacterManager.instance.GetScore();
        scoreAlien = CharacterManager.instance.GetNbMiniAlienHit() * CharacterManager.instance.GetMiniAlienBonusScore();
        scoreMeteorite = (CharacterManager.instance.GetNbMeteoriteHit() * 100);
        scoreTotal = scoreDist + scoreAlien - scoreMeteorite;
        if (scoreTotal < 0)
            scoreTotal = 0;
        moneyGain = (scoreTotal / 10) + (CharacterManager.instance.GetNbAlienHit() * CharacterManager.instance.GetAlienBonusMoney());
        if (moneyGain < 0)
            moneyGain = 0;

        listScore.Add(scoreDist);
        listScore.Add(scoreAlien);
        listScore.Add(scoreMeteorite);
        listScore.Add(scoreTotal);
        scoreSetup = true;

        hud.SetActive(false);
    }

    void DrawText(int _index)
	{
        listText[_index].SetActive(true);
	}

    public void SetLocalPos(Vector3 _value)
	{
        scoreScreen.transform.localPosition += _value * Time.deltaTime;
    }

    void UpdateValue(int _index)
	{
        if (listScoreBase[_index] < listScore[_index])
        {
            if (Input.touches.Length > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                    listScoreBase[_index] = listScore[_index];
            }

            float scoreSpeed = 1;
            float speedFactor = 1 + (listScore[0] * (0.5f / 5000));
            if (speedFactor > 2)
                speedFactor = 2;

            if (_index == 0)
                scoreSpeed = 2000 * speedFactor;
            if (_index == 1)
                scoreSpeed = 1200 * speedFactor;
            if (_index == 2)
                scoreSpeed = 800 * speedFactor;
            if (_index == 3)
                scoreSpeed = 2750 * speedFactor;


            listScoreBase[_index] += (scoreSpeed * Time.deltaTime);

            if(_index == 0)
                scoreDistanceText.text = "+" + ((int)listScoreBase[_index]).ToString();
            if (_index == 1)
                scoreAlienText.text = "+" + ((int)listScoreBase[_index]).ToString();
            if (_index == 2)
                scoreMeteoriteText.text = "-" + ((int)listScoreBase[_index]).ToString();
            if (_index == 3)
                scoreTotalText.text = ((int)listScoreBase[_index]).ToString();
        }
        else
        {
            listScoreBase[_index] = listScore[_index];
            if (_index == 0)
                scoreDistanceText.text = "+" + ((int)listScoreBase[_index]).ToString();
            if (_index == 1)
                scoreAlienText.text = "+" + ((int)listScoreBase[_index]).ToString();
            if (_index == 2)
                scoreMeteoriteText.text = "-" + ((int)listScoreBase[_index]).ToString();
            if (_index == 3)
            {
                scoreTotalText.text = ((int)listScoreBase[_index]).ToString();
                scoreDrawed = true;
            }
            else
                scoreEndDraw[_index + 1] = true;
        }
    }

    void SetupValues()
    {
        scoreDistanceText.text = "";
        scoreAlienText.text = "";
        scoreMeteoriteText.text = "";
        scoreTotalText.text = "";

        listText.Add(distanceText.gameObject);
        listText.Add(alienText.gameObject);
        listText.Add(meteoriteText.gameObject);
        listText.Add(totalText.gameObject);

        listScoreText.Add(scoreDistanceText.text);
        listScoreText.Add(scoreAlienText.text);
        listScoreText.Add(scoreMeteoriteText.text);
        listScoreText.Add(scoreTotalText.text);

        listScoreBase.Add(scoreDistBase);
        listScoreBase.Add(scoreAlienBase);
        listScoreBase.Add(scoreMeteoriteBase);
        listScoreBase.Add(scoreTotalBase);
    }

    void UpdateAnimation()
    {
        if (scoreScreen.transform.localPosition.x < 0)
        {
            scoreScreen.transform.localPosition += new Vector3(scoreScreenSpeed, 0, 0) * Time.deltaTime;
        }
        else
        {
            scoreScreen.transform.localPosition = Vector3.zero;
            animationEnd = true;
        }
    }

    public void UpdateAnimationReverse()
    {
        isReverse = true;
        if (scoreScreen.transform.localPosition.x < Screen.width)
        {
            scoreScreen.transform.localPosition += new Vector3(scoreScreenSpeed, 0, 0) * Time.deltaTime;
        }
    }

    public void ResetChestScreen()
	{
        chestPopUpOpened = false;
    }
}
