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
    [SerializeField] private List<Image> listCaseInlineInventory;
    [Space(10)]
    [SerializeField] private Image topModelImg, topModelImgPlayer;
    [SerializeField] private Image baseModelImg, baseModelImgPlayer;
    [SerializeField] private Image wingsModelImg, wingsModelImgPlayer;
    [Space(8)]
    [SerializeField] private List<Sprite> spTopHidden;
    [SerializeField] private List<Sprite> spBodyHidden;
    [SerializeField] private List<Sprite> spWingsHidden;
    [Space(8)]
    [SerializeField] private Sprite[] spTopSpecial;
    [SerializeField] private Sprite[] spBodySpecial;
    [SerializeField] private Sprite[] spWingsSpecial;
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
    [Space(8)]
    [SerializeField] public List<Color> listRarityColor;

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
    int currentTopIndex;
    int currentBodyIndex;
    int currentWingsIndex;

    PlayerData pData;

    public enum PartType { TOP, BASE, WINGS };
    public enum PartSize { SMALL, MEDIUM, LARGE };
    public enum ColorName { Axis, eightys, Metal, Thanos, Luxury, GameAcademy};
    public enum Rarety { BASIC, RARE, LEGENDARY};
    private string[] strColorName;

    private void Awake()
	{
        if (instance == null)
        {
            ZPlayerPrefs.Initialize("ranma", "4bfgr8nr54nop2tyn9s2fquyr64jty4");

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

            pData = new PlayerData();
            pData.chestData = new ChestData();

            instance = this;
        }
	}

	void Start()
    {
        strColorName = new string[nbColor];
        SetStringColorName();

        LoadDefaultLocalPlayerData();

        /*for (int y = 0; y < listSkinsOrdered.Count; y++)    //complete inventory with 1/2 all skins
        {
            //if (listSkinsOrdered[y].index < 54)
                listSkinOwned.Add(listSkinsOrdered[y]);
            //if (y % 2 == 0 && y != 0 && y != 12 && y != 24)
                //listSkinOwned.Add(listSkinsOrdered[y]);
        }*/

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

        for(int i = 0; i < nbCases; i++)
            listCaseButtonInventory[i].enabled = false;
    }

    void LoadDefaultLocalPlayerData()
    {
        currentSkinIndexToOpen = ZPlayerPrefs.GetInt("currentSkinIndexToOpen", 3);
        ZPlayerPrefs.SetInt("currentSkinIndexToOpen", currentSkinIndexToOpen);
        strSkinPlayerOwn = ZPlayerPrefs.GetString("strSkinPlayerOwn", "0/12/24/");
        ZPlayerPrefs.SetString("strSkinPlayerOwn", strSkinPlayerOwn);
        //SetNbSkinOwn();
        
        currentTopName = ZPlayerPrefs.GetString("currentTopName", "Top-small Axis");
        currentBodyName = ZPlayerPrefs.GetString("currentBodyName", "Body-small Axis");
        currentWingsName = ZPlayerPrefs.GetString("currentWingsName", "Wings-small Axis");

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
        if (!ZPlayerPrefs.HasKey("randomListOrder"))
        {
            //shuffle
            listSkins.Sort(new sort());
            for (int i = 0; i < nbSkin; i++)
            {
                if (listSkins[i].index != 0 && listSkins[i].index != 12 && listSkins[i].index != 24 && listSkins[i].index < 54)
                    strRandomListOrder += listSkins[i].index.ToString() + "/";
            }

            ZPlayerPrefs.SetString("randomListOrder", strRandomListOrder);
        }
        else
        {
            List<Skin> tempListSkin = new List<Skin>();
            for (int y = 0; y < nbSkin; y++)
                tempListSkin.Add(listSkins[y]);
            listSkins.Clear();

            strRandomListOrder = ZPlayerPrefs.GetString("randomListOrder");
            Debug.Log("str random : " + strRandomListOrder);
            /*string strRandomListOrderToCut = strRandomListOrder;
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
            }*/

            string[] itemStrList = strRandomListOrder.Split('/');
            for (int i = 0; i < itemStrList.Length-1; i++)
            {
                //Debug.Log("cur str : " + itemStrList[i]);
                int curIndex = int.Parse(itemStrList[i]);
                for (int y = 0; y < nbSkin; y++)
                {
                    if (tempListSkin[y].index == curIndex)
                    {
                        listSkins.Add(tempListSkin[y]);
                        break;
                    }
                }
            }
        }

        //if (nbSkinOwn > 0)
        SetStartSkinOwned();

        int newMoney;
        if (ZPlayerPrefs.HasKey("money"))
            newMoney = ZPlayerPrefs.GetInt("money");
        else
            newMoney = 0;

        int newBumperLevel;       
        if (ZPlayerPrefs.HasKey("bumperLevel"))
            newBumperLevel = ZPlayerPrefs.GetInt("bumperLevel");
        else
            newBumperLevel = 0;

        int newWingLevel;        
        if (ZPlayerPrefs.HasKey("wingLevel"))
            newWingLevel = ZPlayerPrefs.GetInt("wingLevel");
        else
            newWingLevel = 0;

        ChestData chestData = new ChestData(currentSkinIndexToOpen, strSkinPlayerOwn);
        pData = new PlayerData(strRandomListOrder, chestData, newMoney, newBumperLevel, newWingLevel);
    }

    public int GetCurrentTopIndex()
    {
        for (int i = 0; i < listSkinsOrdered.Count; i++)
        {
            if (currentTopIndex == listSkinsOrdered[i].index)
                return listSkinsOrdered[i].index;
        }
        return 0;
    }

    public int GetCurrentBodyIndex()
    {
        for (int i = 0; i < listSkinsOrdered.Count; i++)
        {
            if (currentBodyIndex == listSkinsOrdered[i].index)
                return listSkinsOrdered[i].index;
        }
        return 0;
    }

    public int GetCurrentWingsIndex()
    {
        for (int i = 0; i < listSkinsOrdered.Count; i++)
        {
            if (currentWingsIndex == listSkinsOrdered[i].index)
                return listSkinsOrdered[i].index;
        }
        return 0;
    }

    public void SetCurrentSkinParts(RocketPartsStruct _listIndex)
    {
        for (int i = 0; i < listSkinsOrdered.Count; i++)
        {
            if (_listIndex._0 == listSkinsOrdered[i].index)
            {
                currentTopIndex = _listIndex._0;
                topModelImg.sprite = listSkinsOrdered[i].sprite;
                topModelImgPlayer.sprite = listSkinsOrdered[i].sprite;
            }

            if (_listIndex._1 == listSkinsOrdered[i].index)
            {
                currentBodyIndex = _listIndex._1;
                baseModelImg.sprite = listSkinsOrdered[i].sprite;
                baseModelImgPlayer.sprite = listSkinsOrdered[i].sprite;
            }

            if (_listIndex._2 == listSkinsOrdered[i].index)
            {
                currentWingsIndex = _listIndex._2;
                wingsModelImg.sprite = listSkinsOrdered[i].sprite;
                wingsModelImgPlayer.sprite = listSkinsOrdered[i].sprite;
            }
        }
    }

    void SetNbSkinOwn()
    {
        nbSkinOwn = strSkinPlayerOwn.Split('/').Length - 1;
    }

    public void LoadDatabasePlayerData(int _currentSkinIndexToOpen, string _randomListOrder, string _strSkinPlayerOwn, int _money, int _bumperLevel, int _wingLevel)
    {
        Debug.Log("load database PlayerData");
        currentSkinIndexToOpen = _currentSkinIndexToOpen;
        ZPlayerPrefs.SetInt("currentSkinIndexToOpen", currentSkinIndexToOpen);
        strSkinPlayerOwn = _strSkinPlayerOwn;
        ZPlayerPrefs.SetString("strSkinPlayerOwn", strSkinPlayerOwn);
        //SetNbSkinOwn();

        strRandomListOrder = _randomListOrder;
        List<Skin> tempListSkin = new List<Skin>();
        for (int y = 0; y < listSkins.Count; y++)
            tempListSkin.Add(listSkins[y]);

        listSkins.Clear();
        ZPlayerPrefs.SetString("randomListOrder", strRandomListOrder);

        /*string strRandomListOrderToCut = strRandomListOrder;
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
        }*/

        string[] itemStrList = strRandomListOrder.Split('/');
        for (int i = 0; i < itemStrList.Length - 1; i++)
        {
            int curIndex = int.Parse(itemStrList[i]);
            for (int y = 0; y < tempListSkin.Count; y++)
            {
                if (tempListSkin[y].index == curIndex)
                {
                    listSkins.Add(tempListSkin[y]);
                    break;
                }
            }
        }

        UpdateSkinOwned();
        ChestData chestData = new ChestData(currentSkinIndexToOpen, strSkinPlayerOwn);
        pData = new PlayerData(strRandomListOrder, chestData, _money, _bumperLevel, _wingLevel);
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
            listCaseImgInventory[i].color = Color.clear;
            listCaseInlineInventory[i].color = Color.clear;
            listCaseTextInventory[i].text = "";
        }
    }

    void ShowCaseInfo()
	{
        for (int i = 0; i < nbCases; i++)
        {
            listCaseImgInventory[i].color = Color.white;// (1, 1, 1, 1);
            listCaseInlineInventory[i].color = Color.white;
        }
    }

    public void SetPlayerData(int _currentSkinIndexToOpen, string _randomListOrder, string _strSkinPlayerOwn,
        string _currentTopName, string _currentBodyName, string _currentWingsName)
    {
        currentSkinIndexToOpen = _currentSkinIndexToOpen;
        strRandomListOrder = _randomListOrder;
        strSkinPlayerOwn = _strSkinPlayerOwn;
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
        if(ZPlayerPrefs.GetString("language") == "fr")
            strColorName[(int)ColorName.Luxury] = "Luxe";
        else
            strColorName[(int)ColorName.Luxury] = "Luxury";
        strColorName[(int)ColorName.GameAcademy] = "Game Academy";
        //strColorName[(int)ColorName.Sample1] = "Sample 1";
        //strColorName[(int)ColorName.Sample2] = "Sample 2";
    }

    public Color GetRarityColor(ColorName _colorName)
    {
        switch (_colorName)
        {
            case ColorName.Axis:
                return listRarityColor[(int)Rarety.BASIC];
            case ColorName.eightys:
                return listRarityColor[(int)Rarety.BASIC];
            case ColorName.Metal:
                return listRarityColor[(int)Rarety.BASIC];
            case ColorName.Thanos:
                return listRarityColor[(int)Rarety.LEGENDARY];
            case ColorName.Luxury:
                return listRarityColor[(int)Rarety.RARE];
            case ColorName.GameAcademy:
                return listRarityColor[(int)Rarety.RARE];
            default:
                return listRarityColor[(int)Rarety.BASIC];
            //case ColorName.Sample1:
                //return listRarityColor[(int)Rarety.SPECIAL];
            //case ColorName.Sample2:
                //return listRarityColor[(int)Rarety.SPECIAL];
        }
    }

    public int GetCurrentSkinIndexToOpen()
	{
        return currentSkinIndexToOpen;
	}

    public void IncrementCurrentSkinIndex()
	{
        currentSkinIndexToOpen++;
        ZPlayerPrefs.SetInt("currentSkinIndexToOpen", currentSkinIndexToOpen);
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
        listSkinOwned.Clear();
        string[] strSkinArray = strSkinPlayerOwn.Split('/');
        SetNbSkinOwn();
        for (int i = 0; i < strSkinArray.Length-1; i++)
        {
            for (int y = 0; y < listSkinsOrdered.Count; y++)
            {
                if (int.Parse(strSkinArray[i]) == listSkinsOrdered[y].index)
                {
                    listSkinOwned.Add(listSkinsOrdered[y]);
                    break;
                }
            }
        }
    }

    void SetStartSkinOwned()
    {
        string[] strSkinArray = strSkinPlayerOwn.Split('/');
        SetNbSkinOwn();
        for (int i = 0; i < strSkinArray.Length-1; i++)
        {
            for (int y = 0; y < listSkinsOrdered.Count; y++)
            {
                if (int.Parse(strSkinArray[i]) == listSkinsOrdered[y].index)
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
                listSkinsOrdered[i].isNew = true;
                listSkinOwned.Add(listSkinsOrdered[i]);
                strSkinPlayerOwn += listSkinsOrdered[i].index.ToString() + "/";
                nbSkinOwn++;
                ZPlayerPrefs.SetString("strSkinPlayerOwn", strSkinPlayerOwn);
                IncrementCurrentSkinIndex();

                OpenSkinNotif(listSkinsOrdered[i].partType, listSkinsOrdered[i].partSize);
                break;
            }
        }

        pData.chestData.currentSkinIndexToOpen = currentSkinIndexToOpen;
        pData.chestData.strSkinPlayerOwn = strSkinPlayerOwn;
        FireBaseAuthScript.instance.SendPlayerChestData(pData.chestData);
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

    void SortSkinOwned()
    {
        listSkinOwned.Sort((skin1, skin2) => skin1.index.CompareTo(skin2.index));
    }

    public void OpenListSkinTopOwned(int _partSize)
	{
        ShowCaseInfo();
        SortSkinOwned();
        int caseIndex = 0;
        //int caseSpeIndex = nbCases;
        int curIndex;
        for(int i = 0; i < listSkinOwned.Count; i++)
		{
            if (listSkinOwned[i].partType == PartType.TOP)
			{
                if((int)listSkinOwned[i].partSize == _partSize)
				{
                    //if(listSkinOwned[i].index < 54)
                        curIndex = caseIndex;
                    //else
                        //curIndex = caseSpeIndex;

                    listCaseButtonInventory[curIndex].enabled = true;
                    listSpriteInventory[curIndex] = listSkinOwned[i].sprite;
                    listCaseImgInventory[curIndex].sprite = listSkinOwned[i].spriteDisplayed;
                    listCaseInlineInventory[curIndex].color = GetRarityColor(listSkinOwned[i].colorName);

                    if (listSkinOwned[i].isNew)
                    {
                        listCaseTextInventory[curIndex].text = strColorName[(int)listSkinOwned[i].colorName] + " (new)";
                        listSkinOwned[i].isNew = false;
                    }
                    else
                        listCaseTextInventory[curIndex].text = strColorName[(int)listSkinOwned[i].colorName];

                    partSelected = 0;

                    //if (listSkinOwned[i].index < 54)
                        caseIndex++;
                    //else
                        //caseSpeIndex++;
                }
			}
        }

        if(caseIndex < nbCases- 1)
		{           
            for(int i = caseIndex; i < nbCases; i++)
			{
                listCaseButtonInventory[i].enabled = false;
                listSpriteInventory[i] = spTopHidden[_partSize];
                listCaseImgInventory[i].sprite = spTopHidden[_partSize];
                listCaseTextInventory[i].text = "???";
                listCaseInlineInventory[i].color = Color.gray;
            }
        }

        /*if (caseSpeIndex < nbCases + 2)
        {
            for (int i = caseSpeIndex; i < nbCases + 2; i++)
            {
                listCaseButtonInventory[i].enabled = false;
                listSpriteInventory[i] = spTopHidden[_partSize];
                listCaseImgInventory[i].sprite = spTopHidden[_partSize];
                listCaseTextInventory[i].text = "???";
                listCaseInlineInventory[i].color = Color.gray;
            }
        }*/
    }

    public void OpenListSkinBaseOwned(int _partSize)
    {
        ShowCaseInfo();
        SortSkinOwned();
        int caseIndex = 0;
        //int caseSpeIndex = nbCases;
        int curIndex;
        for (int i = 0; i < listSkinOwned.Count; i++)
        {
            if (listSkinOwned[i].partType == PartType.BASE)
            {
                if ((int)listSkinOwned[i].partSize == _partSize)
                {
                    //if (listSkinOwned[i].index < 54)
                        curIndex = caseIndex;
                    //else
                       //curIndex = caseSpeIndex;

                    listCaseButtonInventory[curIndex].enabled = true;
                    listSpriteInventory[curIndex] = listSkinOwned[i].sprite;
                    listCaseImgInventory[curIndex].sprite = listSkinOwned[i].sprite;
                    listCaseInlineInventory[curIndex].color = GetRarityColor(listSkinOwned[i].colorName);

                    if (listSkinOwned[i].isNew)
                    {
                        listCaseTextInventory[curIndex].text = strColorName[(int)listSkinOwned[i].colorName] + " (new)";
                        listSkinOwned[i].isNew = false;
                    }
                    else
                        listCaseTextInventory[curIndex].text = strColorName[(int)listSkinOwned[i].colorName];

                    partSelected = 1;

                    //if (listSkinOwned[i].index < 54)
                        caseIndex++;
                    //else
                        //caseSpeIndex++;
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
                listCaseInlineInventory[i].color = Color.gray;
            }
        }

        /*if (caseSpeIndex < nbCases + 2)
        {
            for (int i = caseSpeIndex; i < nbCases + 2; i++)
            {
                listCaseButtonInventory[i].enabled = false;
                listSpriteInventory[i] = spBodyHidden[_partSize];
                listCaseImgInventory[i].sprite = spBodyHidden[_partSize];
                listCaseTextInventory[i].text = "???";
                listCaseInlineInventory[i].color = Color.gray;
            }
        }*/
    }

    public void OpenListSkinWingsOwned(int _partSize)
    {
        ShowCaseInfo();
        SortSkinOwned();
        int caseIndex = 0;
        //int caseSpeIndex = nbCases;
        int curIndex;
        for (int i = 0; i < listSkinOwned.Count; i++)
        {
            if (listSkinOwned[i].partType == PartType.WINGS)
            {
                if ((int)listSkinOwned[i].partSize == _partSize)
                {
                    //if (listSkinOwned[i].index < 54)
                        curIndex = caseIndex;
                    //else
                        //curIndex = caseSpeIndex;

                    listCaseButtonInventory[curIndex].enabled = true;
                    listSpriteInventory[curIndex] = listSkinOwned[i].sprite;
                    listCaseImgInventory[curIndex].sprite = listSkinOwned[i].spriteDisplayed;
                    listCaseInlineInventory[curIndex].color = GetRarityColor(listSkinOwned[i].colorName);

                    if (listSkinOwned[i].isNew)
                    {
                        listCaseTextInventory[curIndex].text = strColorName[(int)listSkinOwned[i].colorName] + " (new)";
                        listSkinOwned[i].isNew = false;
                    }
                    else
                        listCaseTextInventory[curIndex].text = strColorName[(int)listSkinOwned[i].colorName];

                    partSelected = 2;

                    //if (listSkinOwned[i].index < 54)
                        caseIndex++;
                    //else
                        //caseSpeIndex++;
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
                listCaseInlineInventory[i].color = Color.gray;
            }
        }

        /*if (caseSpeIndex < nbCases + 2)
        {
            for (int i = caseSpeIndex; i < nbCases + 2; i++)
            {
                listCaseButtonInventory[i].enabled = false;
                listSpriteInventory[i] = spWingsHidden[_partSize];
                listCaseImgInventory[i].sprite = spWingsHidden[_partSize];
                listCaseTextInventory[i].text = "???";
                listCaseInlineInventory[i].color = Color.gray;
            }
        }*/
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
                    currentBodyIndex = listSkinOwned[i].index;
                    ZPlayerPrefs.SetString("currentBodyName", listSkinOwned[i].skinName);
                }
                else if (listSkinOwned[i].partType == PartType.TOP)
                {
                    currentTopIndex = listSkinOwned[i].index;
                    ZPlayerPrefs.SetString("currentTopName", listSkinOwned[i].skinName);
                }
                else if (listSkinOwned[i].partType == PartType.WINGS)
                {
                    currentWingsIndex = listSkinOwned[i].index;
                    ZPlayerPrefs.SetString("currentWingsName", listSkinOwned[i].skinName);
                }

                if (FireBaseAuthScript.instance.GetIsLocalPlayerScoreFind())
                    FireBaseAuthScript.instance.SendRocketSkinChanged(listSkinOwned[i].partType, listSkinOwned[i].index);
                break;
            }
        }
    }

    public int GetNbSkinOwn()
    {
        return nbSkinOwn;
    }

    public void SetSpecialSprite(int _index)
    {
        listSkinsOrdered[12].sprite = spTopSpecial[_index];
        listSkinsOrdered[0].sprite = spBodySpecial[_index];
        listSkinsOrdered[24].sprite = spWingsSpecial[_index];
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


