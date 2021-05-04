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

    [SerializeField] private GameObject scoreScreen;
    [SerializeField] private GameObject menuButton;
    [SerializeField] private GameObject hud;

    float scoreScreenSpeed = 1200f;

    int textIndex = 0;
    bool animationStart = true;
    bool animationEnd = false;
    bool scoreSetup = false;
    bool[] scoreEndDraw;
    bool scoreDrawed = false;
    float cooldownAnimation = 0.75f;

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

    List<int> listScore = new List<int>();
    List<float> listScoreBase = new List<float>();
    List<GameObject> listText = new List<GameObject>();
    List<string> listScoreText = new List<string>();

    void Start()
    {
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
    }

    void Update()
    {
        if (GameManager.instance.GetGameState() == GameManager.GameState.SCORE)
        {
            if (animationEnd)
            {
                if (!scoreDrawed)
                {
                    print("score text index : " + textIndex);
                    //if(textIndex == 0)
                    // DrawText(textIndex);

                    if (scoreEndDraw[textIndex])
                    {
                        //if (textIndex == 4)
                            //return;

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
                    if (cooldownAnimation <= 0)
                        menuButton.SetActive(true);
                    else
                        cooldownAnimation -= Time.deltaTime;
                }
            }
            else
            {
                if (animationStart)
                {
                    if (scoreSetup)
                        UpdateAnimation();
                    else
                        SetValues();
                }
            }
        }
    }

    void SetValues()
    {
        scoreDist = (int)CharacterManager.instance.GetScore();
        scoreAlien = CharacterManager.instance.GetNbAlienHit() * CharacterManager.instance.GetAlienBonusScore();
        scoreMeteorite = (CharacterManager.instance.GetNbMeteoriteHit() * 100);
        scoreTotal = scoreDist + scoreAlien - scoreMeteorite;
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

    void UpdateValue(int _index)
	{
        if (listScoreBase[_index] < listScore[_index])
        {
            float scoreSpeed = 1;
            if (_index == 0)
                scoreSpeed = 2000;
            if (_index == 1)
                scoreSpeed = 1200;
            if (_index == 2)
                scoreSpeed = 800;
            if (_index == 3)
                scoreSpeed = 2500;


            listScoreBase[_index] += (scoreSpeed * Time.deltaTime);

            if(_index == 0)
                scoreDistanceText.text = ((int)listScoreBase[_index]).ToString();
            if (_index == 1)
                scoreAlienText.text = ((int)listScoreBase[_index]).ToString();
            if (_index == 2)
                scoreMeteoriteText.text = ((int)listScoreBase[_index]).ToString();
            if (_index == 3)
                scoreTotalText.text = ((int)listScoreBase[_index]).ToString();
        }
        else
        {
            listScoreBase[_index] = listScore[_index];
            if (_index == 0)
                scoreDistanceText.text = ((int)listScoreBase[_index]).ToString();
            if (_index == 1)
                scoreAlienText.text = ((int)listScoreBase[_index]).ToString();
            if (_index == 2)
                scoreMeteoriteText.text = ((int)listScoreBase[_index]).ToString();
            if (_index == 3)
                scoreTotalText.text = ((int)listScoreBase[_index]).ToString();

            //if (_index == 4)
            //GameManager.instance.SetGameState(GameManager.GameState.MENU);
            if (_index == 3)
                scoreDrawed = true;
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
}
