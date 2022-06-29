using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SkinManager : MonoBehaviour
{
    public static SkinManager instance;

    [SerializeField] private List<Skin> baseShapeSmall, baseShapeMedium, baseShapeLarge;
    [SerializeField] private List<Skin> topShapeSmall, topShapeMedium, topShapeLarge;
    [SerializeField] private List<Skin> wingsShapeSmall, wingsShapeMedium, wingsShapeLarge;
    [Space(10)]
    [SerializeField] private List<Button> listCaseButtonInventory;
    [SerializeField] private List<Image> listCaseImgInventory;
    [SerializeField] private List<Text> listCaseTextInventory;
    [Space(10)]
    [SerializeField] private Image topModelImg, topModelImgPlayer;
    [SerializeField] private Image baseModelImg, baseModelImgPlayer;
    [SerializeField] private Image wingsModelImg, wingsModelImgPlayer;
    [Space(8)]
    [SerializeField] private List<Sprite> spTopHidden;
    [SerializeField] private List<Sprite> spBodyHidden;
    [SerializeField] private List<Sprite> spWingsHidden;
    [Space(8)]
    [SerializeField] private GameObject customNotifGo;
    [SerializeField] private GameObject customButtonNotifGo;
    [SerializeField] private GameObject topGo;
    [SerializeField] private GameObject bodyGo;
    [SerializeField] private GameObject wingsGo;
    [Space(8)]
    [SerializeField] private List<GameObject> listTopGo;
    [SerializeField] private List<GameObject> listBodyGo;
    [SerializeField] private List<GameObject> listWingsGo;

    [Serializable]
    public class NotifState
    {
        public PartTypeState[] parts;
        public bool customNotif;
    }

    [Serializable]
    public class PartTypeState
    {
        public bool partTypeState;
        public PartSizeState[] partSize;
    }

    [Serializable]
    public class PartSizeState
    {
        public bool partSizeState;
    }

    public NotifState notifState = null;

    List<Skin> listSkins = new List<Skin>();
    List<Skin> listSkinsOrdered;
    List<Skin> listSkinOwned = new List<Skin>();
    List<Sprite> listSpriteInventory = new List<Sprite>();
    int nbSkin = 54;
    int nbColor = 6;
    int nbSkinOwn;
    int nbCases = 6;
    int currentSkinIndexToOpen;
    string strSkinPlayerOwn;
    string strRandomListOrder;
    int partSelected;
    bool saveLoaded = false;
    bool needToLoadNotif = false;

    string currentTopName;
    string currentBodyName;
    string currentWingsName;

    PlayerData pData;

    public enum PartType { TOP, BASE, WINGS };
    public enum PartSize { SMALL, MEDIUM, LARGE };
    public enum ColorName { Axis, eightys, Metal, Thanos, Luxury, GameAcademy };
    public enum Rarety { BASIC, RARE, LEGENDARY};
    private string[] strColorName;

    private void Awake()
	{
        if (instance == null)
            instance = this;
	}

	void Start()
    {
        for (int i = 0; i < baseShapeSmall.Count; i++)
            listSkins.Add(baseShapeSmall[i]);
        for (int i = 0; i < baseShapeMedium.Count; i++)
            listSkins.Add(baseShapeMedium[i]);
        for (int i = 0; i < baseShapeLarge.Count; i++)
            listSkins.Add(baseShapeLarge[i]);

        for (int i = 0; i < topShapeSmall.Count; i++)
            listSkins.Add(topShapeSmall[i]);
        for (int i = 0; i < topShapeMedium.Count; i++)
            listSkins.Add(topShapeMedium[i]);
        for (int i = 0; i < topShapeLarge.Count; i++)
            listSkins.Add(topShapeLarge[i]);

        for (int i = 0; i < wingsShapeSmall.Count; i++)
            listSkins.Add(wingsShapeSmall[i]);
        for (int i = 0; i < wingsShapeMedium.Count; i++)
            listSkins.Add(wingsShapeMedium[i]);
        for (int i = 0; i < wingsShapeLarge.Count; i++)
            listSkins.Add(wingsShapeLarge[i]);

        listSkinsOrdered = new List<Skin>(listSkins);
        listSkinsOrdered.Sort((skin1, skin2) => skin1.index.CompareTo(skin2.index));

        strColorName = new string[nbColor];
        SetStringColorName();

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        LoadDefaultLocalPlayerData();

        //complete inventory with 1/2 all skins
        /*for (int y = 0; y < nbSkin; y++)
        {
            if (y % 2 == 0)
                listSkinOwned.Add(listSkins[y]);
        }*/

        ///////////////////////////////////////////////////////////////////////////////////////////////////////

        for (int i = 0; i < listCaseImgInventory.Count; i++)
		{
            listSpriteInventory.Add(listCaseImgInventory[i].sprite);
		}

        HideCaseInfo();

        notifState = new NotifState();
        notifState.parts = new PartTypeState[3];
        for (int i = 0; i < 3; i++)
        {
            notifState.parts[i] = new PartTypeState();
            notifState.parts[i].partSize = new PartSizeState[3];
            for (int y = 0; y < 3; y++)
                notifState.parts[i].partSize[y] = new PartSizeState();
        }
    }

    void LoadDefaultLocalPlayerData()
    {
        currentSkinIndexToOpen = PlayerPrefs.GetInt("currentSkinIndexToOpen", 0);
        PlayerPrefs.SetInt("currentSkinIndexToOpen", currentSkinIndexToOpen);
        strSkinPlayerOwn = PlayerPrefs.GetString("strSkinPlayerOwn", "0/12/24/");
        PlayerPrefs.SetString("strSkinPlayerOwn", strSkinPlayerOwn);
        nbSkinOwn = PlayerPrefs.GetInt("nbSkinOwn", 3);
        PlayerPrefs.SetInt("nbSkinOwn", nbSkinOwn);

        currentTopName = PlayerPrefs.GetString("currentTopName", "Top-small Axis");
        currentBodyName = PlayerPrefs.GetString("currentBodyName", "Body-small Axis");
        currentWingsName = PlayerPrefs.GetString("currentWingsName", "Wings-small Axis");

        for (int i = 0; i < listSkins.Count; i++)
        {
            if (currentTopName == listSkins[i].skinName)
            {
                topModelImg.sprite = listSkins[i].sprite;
                topModelImgPlayer.sprite = listSkins[i].sprite;
            }

            if (currentBodyName == listSkins[i].skinName)
            {
                baseModelImg.sprite = listSkins[i].sprite;
                baseModelImgPlayer.sprite = listSkins[i].sprite;
            }

            if (currentWingsName == listSkins[i].skinName)
            {
                wingsModelImg.sprite = listSkins[i].sprite;
                wingsModelImgPlayer.sprite = listSkins[i].sprite;
            }
        }

        strRandomListOrder = "0/12/24/";
        if (!PlayerPrefs.HasKey("randomListOrder"))
        {
            //shuffle
            listSkins.Sort(new sort());
            for (int i = 0; i < nbSkin; i++)
            {
                if (listSkins[i].index != 0 && listSkins[i].index != 12 && listSkins[i].index != 24)
                    strRandomListOrder += listSkins[i].index.ToString() + "/";
            }

            PlayerPrefs.SetString("randomListOrder", strRandomListOrder);
        }
        else
        {
            List<Skin> tempListSkin = new List<Skin>();
            for (int y = 0; y < nbSkin; y++)
                tempListSkin.Add(listSkins[y]);
            listSkins.Clear();

            strRandomListOrder = PlayerPrefs.GetString("randomListOrder");
            string strRandomListOrderToCut = strRandomListOrder;
            for (int i = 0; i < nbSkin; i++)
            {
                int charIndex = strRandomListOrderToCut.IndexOf('/');
                int currentSkinIndex = int.Parse(strRandomListOrderToCut.Substring(0, charIndex));
                strRandomListOrderToCut = strRandomListOrderToCut.Substring(charIndex + 1);

                for (int y = 0; y < nbSkin; y++)
                {
                    if (tempListSkin[y].index == currentSkinIndex)
                    {
                        listSkins.Add(tempListSkin[y]);
                        break;
                    }
                }
            }
        }

        if (nbSkinOwn > 0)
            SetStartSkinOwned();

        int newMoney;
        if (PlayerPrefs.HasKey("money"))
            newMoney = PlayerPrefs.GetInt("money");
        else
            newMoney = 0;

        int newBumperLevel;       
        if (PlayerPrefs.HasKey("bumperLevel"))
            newBumperLevel = PlayerPrefs.GetInt("bumperLevel");
        else
            newBumperLevel = 0;

        int newWingLevel;        
        if (PlayerPrefs.HasKey("wingLevel"))
            newWingLevel = PlayerPrefs.GetInt("wingLevel");
        else
            newWingLevel = 0;

        pData = new PlayerData(currentSkinIndexToOpen, strRandomListOrder, strSkinPlayerOwn, nbSkinOwn, currentTopName, currentBodyName, currentWingsName, newMoney, newBumperLevel, newWingLevel);
    }

    public int GetCurrentTopIndex()
    {
        for (int i = 0; i < listSkinsOrdered.Count; i++)
        {
            if (currentTopName == listSkinsOrdered[i].skinName)
                return listSkinsOrdered[i].index;
        }
        return 0;
    }

    public int GetCurrentBodyIndex()
    {
        for (int i = 0; i < listSkinsOrdered.Count; i++)
        {
            if (currentBodyName == listSkinsOrdered[i].skinName)
                return listSkinsOrdered[i].index;
        }
        return 0;
    }

    public int GetCurrentWingsIndex()
    {
        for (int i = 0; i < listSkinsOrdered.Count; i++)
        {
            if (currentWingsName == listSkinsOrdered[i].skinName)
                return listSkinsOrdered[i].index;
        }
        return 0;
    }

    public void LoadDatabasePlayerData(int _currentSkinIndexToOpen, string _randomListOrder, string _strSkinPlayerOwn, int _nbSkinOwn,
        string _currentTopName, string _currentBodyName, string _currentWingsName, int _money, int _bumperLevel, int _wingLevel)
    {
        Debug.Log("enter load database PlayerData");
        currentSkinIndexToOpen = _currentSkinIndexToOpen;
        PlayerPrefs.SetInt("currentSkinIndexToOpen", currentSkinIndexToOpen);
        strSkinPlayerOwn = _strSkinPlayerOwn;
        PlayerPrefs.SetString("strSkinPlayerOwn", strSkinPlayerOwn);
        nbSkinOwn = _nbSkinOwn;
        PlayerPrefs.SetInt("nbSkinOwn", nbSkinOwn);
        Debug.Log("SET playerpref");
        currentTopName = _currentTopName;
        currentBodyName = _currentBodyName;
        currentWingsName = _currentWingsName;

        for (int i = 0; i < listSkins.Count; i++)
        {
            if (currentTopName == listSkins[i].skinName)
            {
                topModelImg.sprite = listSkins[i].sprite;
                topModelImgPlayer.sprite = listSkins[i].sprite;
            }

            if (currentBodyName == listSkins[i].skinName)
            {
                baseModelImg.sprite = listSkins[i].sprite;
                baseModelImgPlayer.sprite = listSkins[i].sprite;
            }

            if (currentWingsName == listSkins[i].skinName)
            {
                wingsModelImg.sprite = listSkins[i].sprite;
                wingsModelImgPlayer.sprite = listSkins[i].sprite;
            }
        }

        strRandomListOrder = _randomListOrder;
        Debug.Log("aaaaaa");
        List<Skin> tempListSkin = new List<Skin>();
        for (int y = 0; y < listSkins.Count; y++)
            tempListSkin.Add(listSkins[y]);
        listSkins.Clear();
        Debug.Log("bbbbbbb");
        PlayerPrefs.SetString("randomListOrder", strRandomListOrder);

        string strRandomListOrderToCut = strRandomListOrder;
        for (int i = 0; i < nbSkin; i++)
        {
            int charIndex = strRandomListOrderToCut.IndexOf('/');
            int currentSkinIndex = int.Parse(strRandomListOrderToCut.Substring(0, charIndex));
            strRandomListOrderToCut = strRandomListOrderToCut.Substring(charIndex + 1);

            for (int y = 0; y < tempListSkin.Count; y++)
            {
                if (tempListSkin[y].index == currentSkinIndex)
                {
                    listSkins.Add(tempListSkin[y]);
                    break;
                }
            }
        }
        Debug.Log("ccccccc");
        if (nbSkinOwn > 0)
            UpdateSkinOwned();

        pData = new PlayerData(currentSkinIndexToOpen, strRandomListOrder, strSkinPlayerOwn, nbSkinOwn, currentTopName, currentBodyName, currentWingsName, _money, _bumperLevel, _wingLevel);

        Debug.Log("SET MONEY 1");
        CustomScreen.instance.SetMoneyAndUpgradesLevel(_money, _bumperLevel, _wingLevel);
    }

    void CreateSaveFile()
    {
        if(!File.Exists(Application.persistentDataPath + "/Resources/NotifSave.json"))
            File.Create(Application.persistentDataPath + "/Resources/NotifSave.json");
    }

    public void SaveNotif()
    {
        StartCoroutine(SaveCoroutine());
    }

    private IEnumerator SaveCoroutine()
    {
        WriteSave();
        yield return null;
    }

    void WriteSave()
    {
        string toJson = JsonUtility.ToJson(notifState);
        if (File.Exists(Application.persistentDataPath + "/Resources/NotifSave.json"))
            File.WriteAllText(Application.persistentDataPath + "/Resources/NotifSave.json", toJson);
        else
            File.Create(Application.persistentDataPath + "/Resources/NotifSave.json");
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
            needToLoadNotif = true;
        }
        else
        {
            CreateSaveFile();
            SetNotifFalse();
            needToLoadNotif = true;
            customNotifGo.SetActive(false);
        }
    }

    void LoadNotif()
	{
        customNotifGo.SetActive(notifState.customNotif);
        customButtonNotifGo.SetActive(notifState.customNotif);

        topGo.SetActive(notifState.parts[0].partTypeState);
        listTopGo[0].SetActive(notifState.parts[0].partSize[0].partSizeState);
        listTopGo[1].SetActive(notifState.parts[0].partSize[1].partSizeState);
        listTopGo[2].SetActive(notifState.parts[0].partSize[2].partSizeState);

        bodyGo.SetActive(notifState.parts[1].partTypeState);
        listBodyGo[0].SetActive(notifState.parts[1].partSize[0].partSizeState);
        listBodyGo[1].SetActive(notifState.parts[1].partSize[1].partSizeState);
        listBodyGo[2].SetActive(notifState.parts[1].partSize[2].partSizeState);

        wingsGo.SetActive(notifState.parts[2].partTypeState);
        listWingsGo[0].SetActive(notifState.parts[2].partSize[0].partSizeState);
        listWingsGo[1].SetActive(notifState.parts[2].partSize[1].partSizeState);
        listWingsGo[2].SetActive(notifState.parts[2].partSize[2].partSizeState);
    }

    bool ReadSave()
    {
        string toJson = null;

        if (File.Exists(Application.persistentDataPath + "/Resources/NotifSave.json"))
        {
            toJson = File.ReadAllText(Application.persistentDataPath + "/Resources/NotifSave.json");

            if (toJson != null)
            {
                notifState = JsonUtility.FromJson<NotifState>(toJson);
                return true;
            }

            return false;
        }

        return false;
    }

    void Update()
    {
        if (!saveLoaded)
        {
            LoadSave();
            saveLoaded = true;
        }

        if (needToLoadNotif)
        {
            if (File.Exists(Application.persistentDataPath + "/Resources/NotifSave.json"))
            {
                string file = File.ReadAllText(Application.persistentDataPath + "/Resources/NotifSave.json");
                if (file != "")
                {
                    LoadNotif();
                    needToLoadNotif = false;
                }
                else
                    SaveNotif();
            }
        }
    }

    public void HideCaseInfo()
	{
        for (int i = 0; i < nbCases; i++)
        {
            listCaseImgInventory[i].color = new Color(1, 1, 1, 0);
            listCaseTextInventory[i].text = "";
        }
    }

    void ShowCaseInfo()
	{
        for (int i = 0; i < nbCases; i++)
        {
            listCaseImgInventory[i].color = new Color(1, 1, 1, 1);
        }
    }

    public void SetPlayerData(int _currentSkinIndexToOpen, string _randomListOrder, string _strSkinPlayerOwn, int _nbSkinOwn,
        string _currentTopName, string _currentBodyName, string _currentWingsName)
    {
        currentSkinIndexToOpen = _currentSkinIndexToOpen;
        strRandomListOrder = _randomListOrder;
        strSkinPlayerOwn = _strSkinPlayerOwn;
        nbSkinOwn = _nbSkinOwn;
        currentTopName = _currentTopName;
        currentBodyName = _currentBodyName;
        currentWingsName = _currentWingsName;
    }

    public PlayerData GetPlayerData()
    {
        return pData;
    }

    public void SetPlayerDataMoney(int _newMoney)
    {
        pData.money = _newMoney;
    }

    void SetStringColorName()
	{
        strColorName[(int)ColorName.Axis] = "Axis";
        strColorName[(int)ColorName.eightys] = "Retro";
        strColorName[(int)ColorName.Metal] = "Metal";
        strColorName[(int)ColorName.Thanos] = "Thanos";
        if(PlayerPrefs.GetString("language") == "fr")
            strColorName[(int)ColorName.Luxury] = "Luxe";
        else
            strColorName[(int)ColorName.Luxury] = "Luxury";
        strColorName[(int)ColorName.GameAcademy] = "Game Academy";
    }

    public int GetCurrentSkinIndexToOpen()
	{
        return currentSkinIndexToOpen;
	}

    public void IncrementCurrentSkinIndex()
	{
        currentSkinIndexToOpen++;
        PlayerPrefs.SetInt("currentSkinIndexToOpen", currentSkinIndexToOpen);
    }

    public List<Skin> GetListSkin()
	{
        return listSkins;
	}

    public List<Skin> GetListSkinsOrdered()
    {
        return listSkinsOrdered;
    }

    private class sort : IComparer<Skin>
    {
        int IComparer<Skin>.Compare(Skin _skinA, Skin _skinB)
        {
            int id1 = _skinA.id;
            int id2 = _skinB.id;
            return id1.CompareTo(id2);
        }
    }

    void UpdateSkinOwned()
    {
        Debug.Log("strrrrrrrrrr : " + strSkinPlayerOwn);
        listSkinOwned.Clear();
        string strSkin = strSkinPlayerOwn;
        for (int i = 0; i < nbSkinOwn; i++)
        {
            int charIndex = strSkin.IndexOf('/');
            int currentSkinIndex = int.Parse(strSkin.Substring(0, charIndex));
            strSkin = strSkin.Substring(charIndex + 1);

            for (int y = 0; y < listSkinsOrdered.Count; y++)
            {
                if (currentSkinIndex == listSkinsOrdered[y].index)
                {
                    listSkinOwned.Add(listSkinsOrdered[y]);
                    break;
                }
            }
        }
    }

    void SetStartSkinOwned()
    {
        string strSkin = strSkinPlayerOwn;
        for (int i = 0; i < nbSkinOwn; i++)
        {
            int charIndex = strSkin.IndexOf('/');
            int currentSkinIndex = int.Parse(strSkin.Substring(0, charIndex));
            strSkin = strSkin.Substring(charIndex + 1);

            for (int y = 0; y < listSkinsOrdered.Count; y++)
			{
                if (currentSkinIndex == listSkinsOrdered[y].index)
                {
                    listSkinOwned.Add(listSkinsOrdered[y]);
                    break;
                }
            }
        }

        for (int i = 0; i < listSkins.Count; i++)
        {
            if (listSkins[i].index == 0)
            {
                listSkins.Remove(listSkins[i]);
                break;
            }
        }

        for (int i = 0; i < listSkins.Count; i++)
        {
            if (listSkins[i].index == 12)
            {
                listSkins.Remove(listSkins[i]);
                break;
            }
        }

        for (int i = 0; i < listSkins.Count; i++)
        {
            if (listSkins[i].index == 24)
            {
                listSkins.Remove(listSkins[i]);
                break;
            }
        }
    }

    public int GetNbSkin()
	{
        return nbSkin;
	}

    public void AddSkinToInventory(int _index)
	{
        for (int i = 0; i < listSkinsOrdered.Count; i++)
        {
            if (listSkinsOrdered[i].index == _index)
            {
                //listSkins[i].isNew = true;
                listSkinsOrdered[i].isNew = true;
                listSkinOwned.Add(listSkinsOrdered[i]);
                strSkinPlayerOwn += listSkinsOrdered[i].index.ToString() + "/";
                nbSkinOwn++;
                PlayerPrefs.SetInt("nbSkinOwn", nbSkinOwn);
                PlayerPrefs.SetString("strSkinPlayerOwn", strSkinPlayerOwn);
                IncrementCurrentSkinIndex();

                OpenSkinNotif(listSkinsOrdered[i].partType, listSkinsOrdered[i].partSize);
                break;
            }
        }
    }

    void SetNotifFalse()
	{
        notifState.customNotif = false;
        notifState.parts[0].partTypeState = false;
        notifState.parts[0].partSize[0].partSizeState = false;
        notifState.parts[0].partSize[1].partSizeState = false;
        notifState.parts[0].partSize[2].partSizeState = false;

        notifState.parts[1].partTypeState = false;
        notifState.parts[1].partSize[0].partSizeState = false;
        notifState.parts[1].partSize[1].partSizeState = false;
        notifState.parts[1].partSize[2].partSizeState = false;

        notifState.parts[2].partTypeState = false;
        notifState.parts[2].partSize[0].partSizeState = false;
        notifState.parts[2].partSize[1].partSizeState = false;
        notifState.parts[2].partSize[2].partSizeState = false;
    }

    void OpenSkinNotif(PartType _type, PartSize _size)
	{
        notifState.customNotif = true;
        if (_type == PartType.TOP)
        {
            notifState.parts[0].partTypeState = true;
            if (_size == PartSize.SMALL)
                notifState.parts[0].partSize[0].partSizeState = true;
            else if (_size == PartSize.MEDIUM)
                notifState.parts[0].partSize[1].partSizeState = true;
            else if (_size == PartSize.LARGE)
                notifState.parts[0].partSize[2].partSizeState = true;
        }
        else if (_type == PartType.BASE)
        {
            notifState.parts[1].partTypeState = true;
            if (_size == PartSize.SMALL)
                notifState.parts[1].partSize[0].partSizeState = true;
            else if (_size == PartSize.MEDIUM)
                notifState.parts[1].partSize[1].partSizeState = true;
            else if (_size == PartSize.LARGE)
                notifState.parts[1].partSize[2].partSizeState = true;
        }
        else if (_type == PartType.WINGS)
        {
            notifState.parts[2].partTypeState = true;
            if (_size == PartSize.SMALL)
                notifState.parts[2].partSize[0].partSizeState = true;
            else if (_size == PartSize.MEDIUM)
                notifState.parts[2].partSize[1].partSizeState = true;
            else if (_size == PartSize.LARGE)
                notifState.parts[2].partSize[2].partSizeState = true;
        }

        SaveNotif();
    }

    void GetAllNotifClosed()
	{
        bool allClosed = true;
        for(int i = 0; i < 3; i++)
		{
            for (int y = 0; y < 3; y++)
            {
                if (notifState.parts[i].partSize[y].partSizeState == true)
                    allClosed = false;
            }
        }

        if (allClosed)
        {
            notifState.customNotif = false;
            customNotifGo.SetActive(false);
            customButtonNotifGo.SetActive(false);
        }
        else
        {
            notifState.customNotif = true;
            customNotifGo.SetActive(true);
            customButtonNotifGo.SetActive(false);
        }
	}

    public void SetTopNotifStateFalse(int _partSize)
    {
        if(_partSize == 0)
            notifState.parts[0].partSize[0].partSizeState = false;
        else if (_partSize == 1)
            notifState.parts[0].partSize[1].partSizeState = false;
        else if (_partSize == 2)
            notifState.parts[0].partSize[2].partSizeState = false;

        if(notifState.parts[0].partSize[0].partSizeState == false && notifState.parts[0].partSize[1].partSizeState == false && notifState.parts[0].partSize[2].partSizeState == false)
        {
            notifState.parts[0].partTypeState = false;
            topGo.SetActive(false);
        }
        GetAllNotifClosed();

        SaveNotif();
    }

    public void SetBodyNotifStateFalse(int _partSize)
    {
        if (_partSize == 0)
            notifState.parts[1].partSize[0].partSizeState = false;
        else if (_partSize == 1)
            notifState.parts[1].partSize[1].partSizeState = false;
        else if (_partSize == 2)
            notifState.parts[1].partSize[2].partSizeState = false;

        if (notifState.parts[1].partSize[0].partSizeState == false && notifState.parts[1].partSize[1].partSizeState == false && notifState.parts[1].partSize[2].partSizeState == false)
        {
            notifState.parts[1].partTypeState = false;
            bodyGo.SetActive(false);
        }
        GetAllNotifClosed();

        SaveNotif();
    }

    public void SetWingsNotifStateFalse(int _partSize)
    {
        if (_partSize == 0)
            notifState.parts[2].partSize[0].partSizeState = false;
        else if (_partSize == 1)
            notifState.parts[2].partSize[1].partSizeState = false;
        else if (_partSize == 2)
            notifState.parts[2].partSize[2].partSizeState = false;

        if (notifState.parts[2].partSize[0].partSizeState == false && notifState.parts[2].partSize[1].partSizeState == false && notifState.parts[2].partSize[2].partSizeState == false)
        {
            notifState.parts[2].partTypeState = false;
            wingsGo.SetActive(false);
        }
        GetAllNotifClosed();

        SaveNotif();
    }

    public void OpenListSkinTopOwned(int _partSize)
	{
        ShowCaseInfo();
        int caseIndex = 0;
        for(int i = 0; i < listSkinOwned.Count; i++)
		{
            if (listSkinOwned[i].partType == PartType.TOP)
			{
                if((int)listSkinOwned[i].partSize == _partSize)
				{
                    listCaseButtonInventory[caseIndex].enabled = true;
                    listSpriteInventory[caseIndex] = listSkinOwned[i].sprite;
                    listCaseImgInventory[caseIndex].sprite = listSkinOwned[i].spriteDisplayed;

                    if (listSkinOwned[i].isNew)
                    {
                        listCaseTextInventory[caseIndex].text = strColorName[(int)listSkinOwned[i].colorName] + " (new)";
                        listSkinOwned[i].isNew = false;
                    }
                    else
                        listCaseTextInventory[caseIndex].text = strColorName[(int)listSkinOwned[i].colorName];

                    partSelected = 0;
                    caseIndex++;
                }
			}
        }

        if(caseIndex < nbCases-1)
		{
            for(int i = caseIndex; i < nbCases; i++)
			{
                listCaseButtonInventory[i].enabled = false;
                listSpriteInventory[i] = spTopHidden[_partSize];
                listCaseImgInventory[i].sprite = spTopHidden[_partSize];
                listCaseTextInventory[i].text = "???";
            }
        }
	}

    public void OpenListSkinBaseOwned(int _partSize)
    {
        ShowCaseInfo();
        int caseIndex = 0;
        for (int i = 0; i < listSkinOwned.Count; i++)
        {
            if (listSkinOwned[i].partType == PartType.BASE)
            {
                if ((int)listSkinOwned[i].partSize == _partSize)
                {
                    listCaseButtonInventory[caseIndex].enabled = true;
                    listSpriteInventory[caseIndex] = listSkinOwned[i].sprite;
                    listCaseImgInventory[caseIndex].sprite = listSkinOwned[i].sprite;
                    listCaseTextInventory[caseIndex].text = strColorName[(int)listSkinOwned[i].colorName];

                    if (listSkinOwned[i].isNew)
                    {
                        listCaseTextInventory[caseIndex].text = strColorName[(int)listSkinOwned[i].colorName] + " (new)";
                        listSkinOwned[i].isNew = false;
                    }
                    else
                        listCaseTextInventory[caseIndex].text = strColorName[(int)listSkinOwned[i].colorName];

                    partSelected = 1;
                    caseIndex++;
                }
            }
        }

        if (caseIndex < nbCases - 1)
        {
            for (int i = caseIndex; i < nbCases; i++)
            {
                listCaseButtonInventory[i].enabled = false;
                listSpriteInventory[i] = spBodyHidden[_partSize];
                listCaseImgInventory[i].sprite = spBodyHidden[_partSize];
                listCaseTextInventory[i].text = "???";
            }
        }
    }

    public void OpenListSkinWingsOwned(int _partSize)
    {
        ShowCaseInfo();
        int caseIndex = 0;
        for (int i = 0; i < listSkinOwned.Count; i++)
        {
            if (listSkinOwned[i].partType == PartType.WINGS)
            {
                if ((int)listSkinOwned[i].partSize == _partSize)
                {
                    listCaseButtonInventory[caseIndex].enabled = true;
                    listSpriteInventory[caseIndex] = listSkinOwned[i].sprite;
                    listCaseImgInventory[caseIndex].sprite = listSkinOwned[i].spriteDisplayed;
                    listCaseTextInventory[caseIndex].text = strColorName[(int)listSkinOwned[i].colorName];

                    if (listSkinOwned[i].isNew)
                    {
                        listCaseTextInventory[caseIndex].text = strColorName[(int)listSkinOwned[i].colorName] + " (new)";
                        listSkinOwned[i].isNew = false;
                    }
                    else
                        listCaseTextInventory[caseIndex].text = strColorName[(int)listSkinOwned[i].colorName];

                    partSelected = 2;
                    caseIndex++;
                }
            }
        }

        if (caseIndex < nbCases - 1)
        {
            for (int i = caseIndex; i < nbCases; i++)
            {
                listCaseButtonInventory[i].enabled = false;
                listSpriteInventory[i] = spWingsHidden[_partSize];
                listCaseImgInventory[i].sprite = spWingsHidden[_partSize];
                listCaseTextInventory[i].text = "???";
            }
        }
    }

    public void AddSelectedSkinToPlayer(int _caseIndex)
	{
        if (partSelected == 0)
        {
            topModelImg.sprite = listSpriteInventory[_caseIndex];
            topModelImgPlayer.sprite = listSpriteInventory[_caseIndex];
        }
        else if (partSelected == 1)
        {
            baseModelImg.sprite = listSpriteInventory[_caseIndex];
            baseModelImgPlayer.sprite = listSpriteInventory[_caseIndex];
        }
        else if (partSelected == 2)
        {
            wingsModelImg.sprite = listSpriteInventory[_caseIndex];
            wingsModelImgPlayer.sprite = listSpriteInventory[_caseIndex];
        }

        for (int i = 0; i < listSkinOwned.Count; i++)
        {
            if (listSpriteInventory[_caseIndex] == listSkinOwned[i].sprite)
            {
                if (listSkinOwned[i].partType == PartType.BASE)
                {
                    currentBodyName = listSkinOwned[i].skinName;
                    pData.currentBodyName = currentBodyName;
                    PlayerPrefs.SetString("currentBodyName", listSkinOwned[i].skinName);
                    break;
                }
                else if (listSkinOwned[i].partType == PartType.TOP)
                {
                    currentTopName = listSkinOwned[i].skinName;
                    pData.currentTopName = currentTopName;
                    PlayerPrefs.SetString("currentTopName", listSkinOwned[i].skinName);
                    break;
                }
                else if (listSkinOwned[i].partType == PartType.WINGS)
                {
                    currentWingsName = listSkinOwned[i].skinName;
                    pData.currentWingsName = currentWingsName;
                    PlayerPrefs.SetString("currentWingsName", listSkinOwned[i].skinName);
                    break;
                }
            }
        }

        if(FireBaseAuthScript.instance.GetIsLocalPlayerScoreFind())
            FireBaseAuthScript.instance.SendPlayerDataToDatabase();
    }

	private void OnApplicationQuit()
	{
        SaveNotif();
    }
	private void OnApplicationFocus(bool focus)
	{
        if (focus == false)
            SaveNotif();
    }
}


