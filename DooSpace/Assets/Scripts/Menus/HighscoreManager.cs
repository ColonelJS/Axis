using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreManager : MonoBehaviour
{
    public static HighscoreManager instance;

    [SerializeField] private GameObject goGlobalPanel;
    [SerializeField] private GameObject goLocalPanel;
    [SerializeField] private Image imgButtonGlobal;
    [SerializeField] private Image imgButtonLocal;

    Color colorButtonSelected;
    Color colorButtonUnselected;

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
    bool isNeedToSave = false;

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
        colorButtonUnselected = new Color32(158, 255, 191, 255);
        colorButtonSelected = new Color32(173, 137, 255, 255);
    }

    void Update()
    {
        if(isNeedToSetup)
		{
            SetupBaseHighScore();
            isNeedToSetup = false;
		}

        if(isNeedToSave)
		{
            if (File.Exists(Application.persistentDataPath + "/Resources/Save.json"))
			{
                SaveManager.instance.SaveGame();
                isNeedToSave = false;
			}
        }
    }

    public void SetIsNeedToSetup()
	{
        isNeedToSetup = true;
	}

    public void SetupBaseHighScore()
    {
        string name = "Empty";
        if (PlayerPrefs.HasKey("language"))
        {
            if (PlayerPrefs.GetString("language") == "fr")
                name = "Vide";
            else
                name = "Empty";
        }
        else
        {
            if (Application.systemLanguage == SystemLanguage.French)
            {
                PlayerPrefs.SetString("language", "fr");
                name = "Vide";
            }
            else
            {
                PlayerPrefs.SetString("language", "en");
                name = "Empty";
            }
        }

        for (int i = 0; i < 9; i++)
        {
            scores.rank[i].name = name;
            scores.rank[i].score = 0;
            SetHighscore(i, name, 0);
        }
        isNeedToSave = true;
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

        GameObject.Find("scoreName (" + i + ")").GetComponent<Text>().text = scores.rank[i].name;
        GameObject.Find("score (" + i + ")").GetComponent<Text>().text = scores.rank[i].score.ToString();
    }

    public void UpdateHighscore(int _index, string _names, int _score)
    {
        int i = _index;
        scores.rank[i].name = _names;
        scores.rank[i].score = _score;

        GameObject.Find("scoreName (" + i + ")").GetComponent<Text>().text = scores.rank[i].name;
        GameObject.Find("score (" + i + ")").GetComponent<Text>().text = scores.rank[i].score.ToString();
    }

    public string GetHighscoreName(int _index)
	{
        int i = _index;
        return scores.rank[i].name;

    }

    public void SetScoreName(int _index, string _name)
	{
        scores.rank[_index].name = _name;
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

    public void OnClickButtonGlobal()
    {
        goLocalPanel.SetActive(false);
        goGlobalPanel.SetActive(true);
        imgButtonGlobal.color = colorButtonSelected;
        imgButtonLocal.color = colorButtonUnselected;
    }

    public void OnClickButtonLocal()
    {
        goGlobalPanel.SetActive(false);
        goLocalPanel.SetActive(true);
        imgButtonGlobal.color = colorButtonUnselected;
        imgButtonLocal.color = colorButtonSelected;
    }
}
