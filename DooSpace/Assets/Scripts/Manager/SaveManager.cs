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

    bool pathIsValid = false;
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
        /*if (GetSaveDirectoryExist())
        {
            if (!GetSaveFileExist())
                CreateSaveFile();
        }
        else
            CreateSaveDirectory();*/
        //LoadSave();
    }

    void Update()
    {
        if (!saveLoaded)
        {
            LoadSave();
            saveLoaded = true;
        }

        if (Input.GetKeyUp(KeyCode.S))
            SaveGame();
    }

    bool GetSaveDirectoryExist()
	{
        if (Directory.Exists(Application.dataPath + "/Resources"))
            return true;
        else
            return false;
    }

    bool GetSaveFileExist()
	{
        if (File.Exists(Application.dataPath + "/Resources/Save.json"))
        {
            pathIsValid = true;
            return true;
        }
        else
            return false;
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
            //SaveGame();
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
            else
                print("reading saveFile error");

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

    void SaveHighscore()
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
        //print("writing saveFile...");
        string toJson = JsonUtility.ToJson(save);
        if (File.Exists(Application.persistentDataPath + "/Resources/Save.json"))
        {
            //print("file found");
            File.WriteAllText(Application.persistentDataPath + "/Resources/Save.json", toJson);

            //print("file writed");
        }
        else
        {
            print("file.path : " + (Application.persistentDataPath + "/Resources/Save.json") + "not found, create one");
            File.Create(Application.persistentDataPath + "/Resources/Save.json");

            print("saveFile create, save back needed");
        }
    }

    void CreateSaveFile()
	{
        //File.Create(Application.persistentDataPath + "/Ressource/Save.json");
        File.Create(Application.persistentDataPath + "/Resources/Save.json");
    }

    void CreateSaveDirectory()
	{
        //Directory.CreateDirectory(Application.persistentDataPath + "/Resources");
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
