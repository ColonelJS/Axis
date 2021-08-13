using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScreen : MonoBehaviour
{
    [SerializeField] private Text distanceText;
    //[SerializeField] private Text alienNbText;
    [SerializeField] private Text alienText;
    //[SerializeField] private Text meteoriteNbText;
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
    //[SerializeField] private Text nameEnteredText;

    float scoreScreenSpeed = 1200f;

    int textIndex = 0;
    bool animationStart = true;
    bool animationEnd = false;
    bool scoreSetup = false;
    bool[] scoreEndDraw;
    bool scoreDrawed = false;
    bool isReverse = false;
    bool enterNameOpened = false;
    bool chestPopUpOpened = false;
    bool moneyGained = false;
    float cooldownAnimation = 0.75f;
    float cooldownChestPopUp = 1.2f;

    string newOldName = "";
    int newOldScore = 0;
    string[] oldName;
    int[] oldScore;

    int scoreDistBase = 0;
    int scoreAlienBase = 0;
    int scoreMeteoriteBase = 0;
    int scoreTotalBase = 0;

    int scoreDist;
    //int scoreAlienNb;
    int scoreAlien;
    //int scoreMeteoriteNb;
    int scoreMeteorite;
    int scoreTotal;
    int moneyGain;

    string nameSaved = "";
    int scoreSaved = 0;

    List<int> listScore = new List<int>();
    List<float> listScoreBase = new List<float>();
    List<GameObject> listText = new List<GameObject>();
    List<string> listScoreText = new List<string>();

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
    }

    void Update()
    {
        if (GameManager.instance.GetGameState() == GameManager.GameState.SCORE && scoreScreen != null)
        {
            if (animationEnd)
            {
                if (!scoreDrawed)
                {
                    print("score text index : " + textIndex);

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
                            cooldownAnimation -= Time.deltaTime;
                        }
                    }
                    else
                        UpdateValue(textIndex - 1);
                }
                else
				{
                    if (cooldownChestPopUp <= 0)
                    {
                        if (!chestPopUpOpened)
                        {
                            chestPopUp.OpenPopUp();
                            chestPopUpOpened = true;

                            /*float tempScoreDist = (int)CharacterManager.instance.GetScore();
                            float tempScoreAlien = CharacterManager.instance.GetNbAlienHit() * CharacterManager.instance.GetAlienBonusScore();
                            float tempScoreMeteorite = (CharacterManager.instance.GetNbMeteoriteHit() * 100);
                            float tempScoreTotal = tempScoreDist + tempScoreAlien - tempScoreMeteorite;*/

                            float score = int.Parse(scoreTotalText.text);

                            chestPopUp.SetXpEarned(score);
                            chestPopUp.SetIsSetValue();
                        }
                    }
                    else
                        cooldownChestPopUp -= Time.deltaTime;
                 
                    if (!moneyGained)
                    {
                        moneyGainGo.SetActive(true);
                        SetMoneyGain();
                        moneyGained = true;
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
        /*int score = listScore[3];
        if (score >= SaveManager.instance.GetScore(8))
            enterName.SetActive(true);
        else
        {
            ValidateName();
            //TransitionScreen.instance.SetTransitionStart();
            //print("set transition start");
        }*/
        chestPopUp.ClosePopUp();
        enterName.SetActive(true);
        SoundManager.instance.PlaySound("openEnterName");
    }

    public void ValidateName()
	{
        enterName.SetActive(false);
        nameSaved = nameEnteredIF.text;
        scoreSaved = listScore[3];

        if(nameSaved == "ilovemoney")
		{
            int currentMoney = PlayerPrefs.GetInt("money");
            int newMoney = currentMoney + 2500;
            PlayerPrefs.SetInt("money", newMoney);
        }

        SetRankValue();
        TransitionScreen.instance.SetTransitionStart();
    }

    void SetMoneyGain()
	{
        int newMoney = 0;    
        int currentMoney = PlayerPrefs.GetInt("money");

        if (GameManager.instance.GetDoubleCoinReward())
        {
            moneyGainText.text = moneyGain.ToString() + " x2";
            newMoney = currentMoney + moneyGain * 2;
        }
        else
        {
            moneyGainText.text = moneyGain.ToString();
            newMoney = currentMoney + moneyGain;
        }
        PlayerPrefs.SetInt("money", newMoney);
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

                //bool isFirst = true;
                //string lastName = SaveManager.instance.GetName(i);
                //int lastScore = SaveManager.instance.GetScore(i);

                SaveManager.instance.SetValue(i, nameSaved, scoreSaved);
                HighscoreManager.instance.UpdateHighscore(i, nameSaved, scoreSaved);
                for (int y = i+1; y < 9; y++)
				{
                    /*if(isFirst)
					{
                        SaveManager.instance.SetValue(y, lastName, lastScore);
                        isFirst = false;
                    }
                    else*/
                        SaveManager.instance.SetValue(y, oldName[y-1], oldScore[y-1]);

                    //SaveManager.instance.SetValue(y, SaveManager.instance.GetName(y-1), SaveManager.instance.GetScore(y-1));
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
        scoreAlien = CharacterManager.instance.GetNbAlienHit() * CharacterManager.instance.GetAlienBonusScore();
        scoreMeteorite = (CharacterManager.instance.GetNbMeteoriteHit() * 100);
        scoreTotal = scoreDist + scoreAlien - scoreMeteorite;
        moneyGain = scoreTotal / 10;
        if (moneyGain < 0)
            moneyGain = 0;
        print("scores : distance : " + scoreDist + ", alien : " + scoreAlien + ", meteorite : " + scoreMeteorite + ", total : " + scoreTotal);
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
        //alienNbText.text = "Alien hit : " + "";
        scoreAlienText.text = "";
        //meteoriteNbText.text = "Meteorite hit : " + "";
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

        /*listScore.Add(scoreDist);
        listScore.Add(scoreAlien);
        listScore.Add(scoreMeteorite);
        listScore.Add(scoreTotal);*/
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
        /*else
        {
            scoreScreen.transform.localPosition = Vector3.zero;
            animationEnd = true;
        }*/
    }
}
