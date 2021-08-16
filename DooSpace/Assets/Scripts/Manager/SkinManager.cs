using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static SkinManager instance;

    [SerializeField] private List<Skin> baseShapeSmall, baseShapeMedium, baseShapeLarge;
    [SerializeField] private List<Skin> topShapeSmall, topShapeMedium, topShapeLarge;
    [SerializeField] private List<Skin> wingsShapeSmall, wingsShapeMedium, wingsShapeLarge;

    List<Skin> listSkins = new List<Skin>();
    int nbSkin = 4;
    int currentSkinIndexToOpen;

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

            //for (int i = 0; i < nbSkin; i++)
                //Debug.Log("skin id : " + listSkins[i].skinName);
        }


    }

    void Update()
    {
        
    }

    public int GetCurrentSkinIndexToOpen()
	{
        return currentSkinIndexToOpen;
	}

    public List<Skin> GetListSkin()
	{
        return listSkins;
	}

    private class sort : IComparer<Skin>
    {
        int IComparer<Skin>.Compare(Skin _skinA, Skin _skinB)
        {
            int index1 = _skinA.id;
            int index2 = _skinB.id;
            return index1.CompareTo(index2);
        }
    }
}


