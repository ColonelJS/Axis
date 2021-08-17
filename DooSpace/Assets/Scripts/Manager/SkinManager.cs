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
    [SerializeField] private List<Image> listCaseImgInventory;
    [SerializeField] private List<Text> listCaseTextInventory;
    [Space(10)]
    [SerializeField] private Image topModelImg;
    [SerializeField] private Image baseModelImg;
    [SerializeField] private Image wingsModelImg;

    List<Skin> listSkins = new List<Skin>();
    List<Skin> listSkinOwned = new List<Skin>();
    int nbSkin = 4;
    int nbColor = 4;
    int currentSkinIndexToOpen;
    string strSkinPlayerOwn;
    int partSelected;

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
        /*for (int i = 0; i < baseShapeMedium.Count; i++)
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
            listSkins.Add(wingsShapeLarge[i]);*/

        strColorName = new string[nbColor];
        SetStringColorName();

        currentSkinIndexToOpen = PlayerPrefs.GetInt("currentSkinIndexToOpen", 0);

        string strRandomListOrder = "";
        if (!PlayerPrefs.HasKey("randomListOrder"))
        {
            //shuffle
            listSkins.Sort(new sort());
            for (int i = 0; i < nbSkin; i++)
                strRandomListOrder += listSkins[i].index.ToString() + "/";

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
        for (int y = 0; y < nbSkin; y++)
            listSkinOwned.Add(listSkins[y]);
    }

    void Update()
    {
        
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

    public void AddSkinToInventory(int _index)
	{
        for (int i = 0; i < nbSkin; i++)
        {
            if (listSkins[i].index == _index)
            {
                listSkinOwned.Add(listSkins[i]);
                strSkinPlayerOwn += listSkins[i].index.ToString() + "/";
                Debug.Log("str skin player own : " + strSkinPlayerOwn);
                PlayerPrefs.SetString("strSkinPlayerOwn", strSkinPlayerOwn);
                break;
            }
        }
    }

    public void OpenListSkinTopOwned(int _partSize)
	{
        int caseIndex = 0;
        for(int i = 0; i < listSkinOwned.Count; i++)
		{
            if (listSkinOwned[i].partType == PartType.TOP)
			{
                if((int)listSkinOwned[i].partSize == _partSize)
				{
                    listCaseImgInventory[caseIndex].sprite = listSkinOwned[i].sprite;
                    listCaseTextInventory[caseIndex].text = strColorName[(int)listSkinOwned[i].colorName];
                    partSelected = 0;
                    caseIndex++;
                }
			}
        }
	}

    public void OpenListSkinBaseOwned(int _partSize)
    {
        int caseIndex = 0;
        for (int i = 0; i < listSkinOwned.Count; i++)
        {
            if (listSkinOwned[i].partType == PartType.BASE)
            {
                if ((int)listSkinOwned[i].partSize == _partSize)
                {
                    listCaseImgInventory[caseIndex].sprite = listSkinOwned[i].sprite;
                    listCaseTextInventory[caseIndex].text = strColorName[(int)listSkinOwned[i].colorName];
                    partSelected = 1;
                    caseIndex++;
                }
            }
        }
    }

    public void OpenListSkinWingsOwned(int _partSize)
    {
        int caseIndex = 0;
        for (int i = 0; i < listSkinOwned.Count; i++)
        {
            if (listSkinOwned[i].partType == PartType.WINGS)
            {
                if ((int)listSkinOwned[i].partSize == _partSize)
                {
                    listCaseImgInventory[caseIndex].sprite = listSkinOwned[i].sprite;
                    listCaseTextInventory[caseIndex].text = strColorName[(int)listSkinOwned[i].colorName];
                    partSelected = 2;
                    caseIndex++;
                }
            }
        }
    }

    public void AddSelectedSkinToPlayer(int _caseIndex)
	{
        if (partSelected == 0)
            topModelImg.sprite = listCaseImgInventory[_caseIndex].sprite;
        else if(partSelected == 1)
            baseModelImg.sprite = listCaseImgInventory[_caseIndex].sprite;
        else if (partSelected == 2)
            wingsModelImg.sprite = listCaseImgInventory[_caseIndex].sprite;

        //set equal to player model in game
    }
}


