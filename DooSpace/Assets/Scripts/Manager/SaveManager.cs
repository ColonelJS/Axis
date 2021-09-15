using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [Serializable]
    public class Save
    {
        public Score[] rank;
    }

    [Serializable]
    public class Score
    {
        public string name;
        public int score;
    }

    public Save save = null;

    bool saveLoaded = false;

	private void Awake()
	{
        if (instance == null)
            instance = this;
	}

	void Start()
    {
        save = new Save();
        save.rank = new Score[9];
        for (int i = 0; i < 9; i++)
            save.rank[i] = new Score();

        CreateSaveDirectory();
    }

    void Update()
    {
        if (!saveLoaded)
        {
            LoadSave();
            saveLoaded = true;
        }
    }

    void LoadSave()
	{
        StartCoroutine(LoadCoroutine());
	}

    IEnumerator LoadCoroutine()
	{
        LoadGame();
        yield return null;
	}

    void LoadGame()
	{
        if (ReadSave())
        {
            LoadHighscore(false);
        }
        else
        {
            CreateSaveFile();
            LoadHighscore(true);
        }
    }

    void LoadHighscore(bool _isFirstLoad)
	{
        if (_isFirstLoad)
        {
            HighscoreManager.instance.SetIsNeedToSetup();
        }
        else
            for (int i = 0; i < 9; i++)
                HighscoreManager.instance.SetHighscore(i, save.rank[i].name, save.rank[i].score);
    }

    bool ReadSave()
	{
        string toJson = null;

        if (File.Exists(Application.persistentDataPath + "/Resources/Save.json"))
        {
            toJson = File.ReadAllText(Application.persistentDataPath + "/Resources/Save.json");

            if (toJson != null)
            {
                save = JsonUtility.FromJson<Save>(toJson);
                return true;
            }

            return false;
        }

        return false;
    }

    public void SaveGame()
    {
        StartCoroutine(SaveCoroutine());
    }

    private IEnumerator SaveCoroutine()
    {
        SaveHighscore();
        WriteSave();
        yield return null;
    }

    public void SaveHighscore()
	{
        for (int i = 0; i < 9; i++)
        {
            save.rank[i].name = HighscoreManager.instance.GetHighscoreName(i);
            save.rank[i].score = HighscoreManager.instance.GetHighscoreScore(i);
        }
    }

    public void SetValue(int _index, string _name, int _score)
	{
        int i = _index;
        save.rank[i].name = _name;
        save.rank[i].score = _score;
    }

    public int GetScore(int _index)
	{
        int i = _index;
        return save.rank[i].score;
    }

    public string GetName(int _index)
    {
        int i = _index;
        return save.rank[i].name;
    }

    void WriteSave()
	{
        string toJson = JsonUtility.ToJson(save);
        if (File.Exists(Application.persistentDataPath + "/Resources/Save.json"))
            File.WriteAllText(Application.persistentDataPath + "/Resources/Save.json", toJson);
        else
            File.Create(Application.persistentDataPath + "/Resources/Save.json");
    }

    void CreateSaveFile()
	{
        if (!File.Exists(Application.persistentDataPath + "/Resources/Save.json"))
            File.Create(Application.persistentDataPath + "/Resources/Save.json");
    }

    void CreateSaveDirectory()
	{
        if(!Directory.Exists(Application.persistentDataPath + "/Resources"))
            Directory.CreateDirectory(Application.persistentDataPath + "/Resources");
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus == false)
        {
            SaveGame();
        }
    }
}
