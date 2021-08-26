using System.Collections;
using System.Collections.Generic;
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

    List<Skin> listSkins = new List<Skin>();
    List<Skin> listSkinOwned = new List<Skin>();
    List<Sprite> listSpriteInventory = new List<Sprite>();
    int nbSkin = 36;
    int nbColor = 4;
    int nbSkinOwn;
    int nbCases = 6;
    int currentSkinIndexToOpen;
    string strSkinPlayerOwn;
    int partSelected;

    string currentTopName;
    string currentBodyName;
    string currentWingsName;

    public enum PartType { TOP, BASE, WINGS };
    public enum PartSize { SMALL, MEDIUM, LARGE };
    public enum ColorName { Axis, eightys, Metal, Thanos };
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

        strColorName = new string[nbColor];
        SetStringColorName();

        currentSkinIndexToOpen = PlayerPrefs.GetInt("currentSkinIndexToOpen", 0);
        strSkinPlayerOwn = PlayerPrefs.GetString("strSkinPlayerOwn", "");
        nbSkinOwn = PlayerPrefs.GetInt("nbSkinOwn", 0);

        currentTopName = PlayerPrefs.GetString("currentTopName", "Top-small Axis");
        currentBodyName = PlayerPrefs.GetString("currentBodyName", "Body-small Axis");
        currentWingsName = PlayerPrefs.GetString("currentWingsName", "Wings-small Axis");

        for (int i = 0; i < listSkins.Count; i++)
		{
            if(currentTopName == listSkins[i].skinName)
			{
                Debug.Log("current top name :" + listSkins[i].skinName);
                topModelImg.sprite = listSkins[i].sprite;
                topModelImgPlayer.sprite = listSkins[i].sprite;
            }

            if (currentBodyName == listSkins[i].skinName)
            {
                Debug.Log("current body name :" + listSkins[i].skinName);
                baseModelImg.sprite = listSkins[i].sprite;
                baseModelImgPlayer.sprite = listSkins[i].sprite;
            }

            if (currentWingsName == listSkins[i].skinName)
            {
                Debug.Log("current wings name :" + listSkins[i].skinName);
                wingsModelImg.sprite = listSkins[i].sprite;
                wingsModelImgPlayer.sprite = listSkins[i].sprite;
            }
        }

        string strRandomListOrder = "";
        if (!PlayerPrefs.HasKey("randomListOrder"))
        {
            //shuffle
            listSkins.Sort(new sort());
            for (int i = 0; i < nbSkin; i++)
                strRandomListOrder += listSkins[i].index.ToString() + "/";

            Debug.Log("new random list order : " + strRandomListOrder);
            PlayerPrefs.SetString("randomListOrder", strRandomListOrder);
        }
        else
        {
            List<Skin> tempListSkin = new List<Skin>();
            for (int y = 0; y < nbSkin; y++)
                tempListSkin.Add(listSkins[y]);
            listSkins.Clear();

            strRandomListOrder = PlayerPrefs.GetString("randomListOrder");
            for (int i = 0; i < nbSkin; i++)
            {
                int charIndex = strRandomListOrder.IndexOf('/');
                int currentSkinIndex = int.Parse(strRandomListOrder.Substring(0, charIndex));
                strRandomListOrder = strRandomListOrder.Substring(charIndex+1);

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

        //temp
        //for (int y = 0; y < nbSkin; y++)
            //listSkinOwned.Add(listSkins[y]);

        if(nbSkinOwn > 0)
            SetStartSkinOwned();

        for(int i = 0; i < listCaseImgInventory.Count; i++)
		{
            listSpriteInventory.Add(listCaseImgInventory[i].sprite);
		}

        HideCaseInfo();
    }

    void Update()
    {
        
    }

    void HideCaseInfo()
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

    void SetStringColorName()
	{
        strColorName[(int)ColorName.Axis] = "Axis";
        strColorName[(int)ColorName.eightys] = "80's";
        strColorName[(int)ColorName.Metal] = "Metal";
        strColorName[(int)ColorName.Thanos] = "Thanos";
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

    private class sort : IComparer<Skin>
    {
        int IComparer<Skin>.Compare(Skin _skinA, Skin _skinB)
        {
            int id1 = _skinA.id;
            int id2 = _skinB.id;
            return id1.CompareTo(id2);
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

            for (int y = 0; y < listSkins.Count; y++)
			{
                if (currentSkinIndex == listSkins[y].index)
                {
                    listSkinOwned.Add(listSkins[y]);
                    break;
                }
            }
        }
    }

    public void AddSkinToInventory(int _index)
	{
        for (int i = 0; i < nbSkin; i++)
        {
            if (listSkins[i].index == _index)
            {
                listSkinOwned.Add(listSkins[i]);
                strSkinPlayerOwn += listSkins[i].index.ToString() + "/";
                nbSkinOwn++;
                PlayerPrefs.SetInt("nbSkinOwn", nbSkinOwn);
                Debug.Log("str skin player own : " + strSkinPlayerOwn);
                PlayerPrefs.SetString("strSkinPlayerOwn", strSkinPlayerOwn);
                break;
            }
        }
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
                    listCaseImgInventory[caseIndex].sprite = listSkinOwned[i].spriteDisplayed;//displayed
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
                listCaseTextInventory[i].text = "????";
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
                listCaseTextInventory[i].text = "????";
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
                    listCaseImgInventory[caseIndex].sprite = listSkinOwned[i].spriteDisplayed;//displayed
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
                listCaseTextInventory[i].text = "????";
            }
        }
    }

    public void AddSelectedSkinToPlayer(int _caseIndex)
	{
        if (partSelected == 0)
        {
            //topModelImg.sprite = listCaseImgInventory[_caseIndex].sprite;
            //topModelImgPlayer.sprite = listCaseImgInventory[_caseIndex].sprite;
            topModelImg.sprite = listSpriteInventory[_caseIndex];
            topModelImgPlayer.sprite = listSpriteInventory[_caseIndex];
        }
        else if (partSelected == 1)
        {
            //baseModelImg.sprite = listCaseImgInventory[_caseIndex].sprite;
            //baseModelImgPlayer.sprite = listCaseImgInventory[_caseIndex].sprite;
            baseModelImg.sprite = listSpriteInventory[_caseIndex];
            baseModelImgPlayer.sprite = listSpriteInventory[_caseIndex];
        }
        else if (partSelected == 2)
        {
            //wingsModelImg.sprite = listCaseImgInventory[_caseIndex].sprite;
            //wingsModelImgPlayer.sprite = listCaseImgInventory[_caseIndex].sprite;
            wingsModelImg.sprite = listSpriteInventory[_caseIndex];
            wingsModelImgPlayer.sprite = listSpriteInventory[_caseIndex];
        }

        for (int i = 0; i < listSkins.Count; i++)
        {
            if (/*listCaseImgInventory[_caseIndex].sprite*/listSpriteInventory[_caseIndex] == listSkins[i].sprite)
            {
                if (listSkins[i].partType == PartType.BASE)
                {
                    PlayerPrefs.SetString("currentBodyName", listSkins[i].skinName);
                    break;
                }
                else if (listSkins[i].partType == PartType.TOP)
                {
                    PlayerPrefs.SetString("currentTopName", listSkins[i].skinName);
                    break;
                }
                else if (listSkins[i].partType == PartType.WINGS)
                {
                    PlayerPrefs.SetString("currentWingsName", listSkins[i].skinName);
                    break;
                }
            }
        }
    }
}


