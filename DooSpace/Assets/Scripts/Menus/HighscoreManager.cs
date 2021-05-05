using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreManager : MonoBehaviour
{
    public static HighscoreManager instance;

    [Serializable]
    public class Scores
	{
        public Score[] rank;
	}

    [Serializable]
    public class Score
    {
        public string name;
        public int score;
    }

    public Scores scores = null;
    bool scoreSetup = false;
    bool isNeedToSetup = false;

    List<string> listName = new List<string>();
    List<int> listScore = new List<int>();

    private void Awake()
	{
        if (instance == null)
        {
            instance = this;
        }
    }

	void Start()
    {
        SetupScore();
    }

    void Update()
    {
        if(isNeedToSetup)
		{
            SetupBaseHighScore();
            isNeedToSetup = false;
		}
    }

    public void SetIsNeedToSetup()
	{
        isNeedToSetup = true;
	}

    public void SetupBaseHighScore()
    {
        for (int i = 0; i < 9; i++)
        {
            scores.rank[i].name = "none -";
            scores.rank[i].score = 0;
        }
    }

    void SetupScore()
    {
        scores = new Scores();
        scores.rank = new Score[9];
        for (int i = 0; i < 9; i++)
        {
            scores.rank[i] = new Score();
        }
        scoreSetup = true;
        for (int i = 0; i < 9; i++)
        {
            listName.Add(GameObject.Find("scoreName (" + i + ")").GetComponent<Text>().text);
            string scoreStr = GameObject.Find("score (" + i + ")").GetComponent<Text>().text;
            int score = int.Parse(scoreStr);
            listScore.Add(score);
        }
    }

    public void SetHighscore(int _index, string _names, int _score)
	{
        if (!scoreSetup)
        {
            scores = new Scores();
            scores.rank = new Score[9];
            scoreSetup = true;
        }

        int i = _index;
        scores.rank[i] = new Score();
        scores.rank[i].name = _names;
        scores.rank[i].score = _score;
        listName[i] = scores.rank[i].name;
        listScore[i] = scores.rank[i].score;

        GameObject.Find("scoreName (" + i + ")").GetComponent<Text>().text = scores.rank[i].name + " -";
        GameObject.Find("score (" + i + ")").GetComponent<Text>().text = scores.rank[i].score.ToString();
    }

    public void UpdateHighscore(int _index, string _names, int _score)
    {
        int i = _index;
        scores.rank[i].name = _names;
        scores.rank[i].score = _score;

        GameObject.Find("scoreName (" + i + ")").GetComponent<Text>().text = scores.rank[i].name + " -";
        GameObject.Find("score (" + i + ")").GetComponent<Text>().text = scores.rank[i].score.ToString();
    }

    public string GetHighscoreName(int _index)
	{
        int i = _index;
        return scores.rank[i].name;

    }

    public int GetHighscoreScore(int _index)
    {
        int i = _index;
        return scores.rank[i].score;
    }

    public void SetHighscore(Score[] _rank)
	{
        scores.rank = _rank;
    }
}
