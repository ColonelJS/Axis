using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreManager : MonoBehaviour
{
    public static HighscoreManager instance;

    [SerializeField] private GooglePlayServicesManager gpgsManager;
    [SerializeField] private FireBaseAuthScript firebaseManager;
    [SerializeField] private SkinManager skinManager;

    [SerializeField] private GameObject goGlobalPanel;
    [SerializeField] private GameObject goLoginToGP;
    [SerializeField] private GameObject goLocalPanel;
    [SerializeField] private Image imgButtonGlobal;
    [SerializeField] private Image imgButtonLocal;

    [SerializeField] private Text[] txtScoreNames;
    [SerializeField] private Text[] txtScores;

    [SerializeField] private GameObject[] scorePrefab;
    [SerializeField] private GameObject goScoreNotSetYet;
    [SerializeField] private GameObject goScoreLoadingScreen;

    [SerializeField] private Text[] txtGlobalScore_rank;
    [SerializeField] private Text[] txtGlobalScore_name;
    [SerializeField] private Text[] txtGlobalScore_score;
    [SerializeField] private Image[] spGlobalScore_top;
    [SerializeField] private Image[] spGlobalScore_body;
    [SerializeField] private Image[] spGlobalScore_wings;

    [SerializeField] private Text txtLocalGlobalScore_rank;
    [SerializeField] private Text txtLocalGlobalScore_name;
    [SerializeField] private Text txtLocalGlobalScore_score;
    [SerializeField] private Image spLocalGlobalScore_top;
    [SerializeField] private Image spLocalGlobalScore_body;
    [SerializeField] private Image spLocalGlobalScore_wings;

    [SerializeField] private Button buttonPrevious;
    [SerializeField] private Button buttonNext;
    [SerializeField] private Text txtScore_min;
    [SerializeField] private Text txtScore_max;
    [SerializeField] private ScrollRect scrollRect;

    [SerializeField] private Color goldRankNameColor;
    Color colorButtonSelected;
    Color colorButtonUnselected;

    int globalScoresPageIndex = 1;
    int maxGlobalScoresPage;

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
        goLoginToGP.SetActive(false);
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
        string name;
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
            listName.Add(txtScoreNames[i].text);
            string scoreStr = txtScores[i].text;
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

        txtScoreNames[i].text = scores.rank[i].name;
        txtScores[i].text = scores.rank[i].score.ToString();
    }

    public void UpdateHighscore(int _index, string _names, int _score)
    {
        int i = _index;
        scores.rank[i].name = _names;
        scores.rank[i].score = _score;

        txtScoreNames[i].text = scores.rank[i].name;
        txtScores[i].text = scores.rank[i].score.ToString();
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
        imgButtonGlobal.color = colorButtonSelected;
        imgButtonLocal.color = colorButtonUnselected;

        if (CheckIsConnected())
        {
            goLoginToGP.SetActive(false);
            goScoreLoadingScreen.SetActive(true);
            goGlobalPanel.SetActive(true);
            buttonPrevious.interactable = false;
            buttonNext.interactable = false;
            SetupGlobalScores();
        }
        else
        {
            goLoginToGP.SetActive(true);
        }
    }

    public void OnClickButtonLocal()
    {
        if (goGlobalPanel.activeSelf)
            goGlobalPanel.SetActive(false);
        else if (goLoginToGP.activeSelf)
            goLoginToGP.SetActive(false);

        goLocalPanel.SetActive(true);

        imgButtonGlobal.color = colorButtonUnselected;
        imgButtonLocal.color = colorButtonSelected;
    }

    void SetupGlobalScores()
    {
        if(CheckIsConnected())
        {
            UpdateMaxGlobalScorePage();
            UpdatePageArrows();
            firebaseManager.ReadFromDatabase(false);
        }
    }

    public void SetLocalPlayerGlobalScore(int _rank, string _name, RocketPartsStruct _rocketPartsId, int _score)
    {
        txtLocalGlobalScore_rank.text = _rank.ToString();
        txtLocalGlobalScore_name.text = _name;  
        txtLocalGlobalScore_score.text = _score.ToString();

        spLocalGlobalScore_top.sprite = skinManager.GetListSkinsOrdered()[_rocketPartsId._0].sprite;
        spLocalGlobalScore_body.sprite = skinManager.GetListSkinsOrdered()[_rocketPartsId._1].sprite;
        spLocalGlobalScore_wings.sprite = skinManager.GetListSkinsOrdered()[_rocketPartsId._2].sprite;

        if (_hasPass)
            txtLocalGlobalScore_name.color = goldRankNameColor;
    }

    public void OpenLocalScoreNotYetSet()
    {
        goScoreNotSetYet.SetActive(true);
    }

    void UpdateMaxGlobalScorePage()
    {
        maxGlobalScoresPage = (firebaseManager.GetUsers().Count / 10) + 1;
    }

    public void UpdateGlobalScores()
    {
        Debug.Log("update global scores");
        goScoreLoadingScreen.SetActive(false);

        scrollRect.verticalNormalizedPosition = 1;

        int pageIndex = globalScoresPageIndex;
        int min = (pageIndex - 1) * 10; //start = 0,  second = 10
        int max = pageIndex * 10;       //start = 10, second = 20
        txtScore_min.text = (min+1).ToString();
        txtScore_max.text = max.ToString();

        int objIndex = 0;

        Debug.Log("min : " + min + ", max : " + max);

        for (int i = min; i < max; i++)
        {
            if (pageIndex != 1)
            {
                objIndex = (i - (((pageIndex - 1) * 10) - 1))-1;
            }
            else
                objIndex = i;

            if (i < firebaseManager.GetUsers().Count)
            {
                scorePrefab[objIndex].SetActive(true);

                txtGlobalScore_rank[objIndex].text = (i + 1).ToString();
                txtGlobalScore_name[objIndex].text = firebaseManager.GetUsers()[i].score.name;
                txtGlobalScore_score[objIndex].text = firebaseManager.GetUsers()[i].score.score.ToString();

                spGlobalScore_top[objIndex].sprite = skinManager.GetListSkinsOrdered()[firebaseManager.GetUsers()[i].score.rocketPartId._0].sprite;
                spGlobalScore_body[objIndex].sprite = skinManager.GetListSkinsOrdered()[firebaseManager.GetUsers()[i].score.rocketPartId._1].sprite;
                spGlobalScore_wings[objIndex].sprite = skinManager.GetListSkinsOrdered()[firebaseManager.GetUsers()[i].score.rocketPartId._2].sprite;

                if (firebaseManager.GetUsers()[i].score.hasPass)
                    txtGlobalScore_name[objIndex].color = goldRankNameColor;
            }
            else
                scorePrefab[objIndex].SetActive(false);
        }
    }

    bool CheckIsConnected()
    {
        return FireBaseAuthScript.instance.GetIsConnectedToGPGSAndFirebase();
    }

    public void NextGlobalScorePage()
    {
        if (globalScoresPageIndex < maxGlobalScoresPage)
        {
            globalScoresPageIndex++;
            UpdatePageArrows();
            UpdateGlobalScores();
        }
        else
            globalScoresPageIndex = maxGlobalScoresPage;
    }

    public void PreviousGlobalScorePage()
    {
        if (globalScoresPageIndex > 1)
        {
            globalScoresPageIndex--;
            UpdatePageArrows();
            UpdateGlobalScores();
        }
        else
            globalScoresPageIndex = 1;
    }

    void UpdatePageArrows()
    {
        buttonPrevious.interactable = true;
        buttonNext.interactable = true;

        if (globalScoresPageIndex == 1)
        {
            buttonPrevious.interactable = false;
        }

        if (globalScoresPageIndex == maxGlobalScoresPage)
        {
            buttonNext.interactable = false;
        }
    }
}
