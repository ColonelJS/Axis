using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    /*[SerializeField] private List<Sprite> baseShapeSmall, baseShapeMedium, baseShapeLarge;
    [SerializeField] private List<Sprite> topShapeSmall, topShapeMedium, topShapeLarge;
    [SerializeField] private List<Sprite> wingsShapeSmall, wingsShapeMedium, wingsShapeLarge;
    Dictionary<string, List<Sprite>> listBaseShape = new Dictionary<string, List<Sprite>>();*/

    [SerializeField] private List<Skin> baseShapeSmall, baseShapeMedium, baseShapeLarge;
    [SerializeField] private List<Skin> topShapeSmall, topShapeMedium, topShapeLarge;
    [SerializeField] private List<Skin> wingsShapeSmall, wingsShapeMedium, wingsShapeLarge;

    List<Skin> listSkins = new List<Skin>();
    int nbSkin = 4;
    //private Random rng = new Random();

    void Start()
    {
        #region old
        /*listBaseShape.Add("baseSmall", baseShapeSmall);
        listBaseShape.Add("baseMedium", baseShapeMedium);
        listBaseShape.Add("baseLarge", baseShapeLarge);

        listBaseShape.Add("topSmall", topShapeSmall);
        listBaseShape.Add("topMedium", topShapeMedium);
        listBaseShape.Add("topLarge", topShapeLarge);

        listBaseShape.Add("wingsSmall", wingsShapeSmall);
        listBaseShape.Add("wingsMedium", wingsShapeMedium);
        listBaseShape.Add("wingsLarge", wingsShapeLarge);*/
        #endregion

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

        listSkins.Sort(new sort());
        for (int i = 0; i < nbSkin; i++)
            Debug.Log("skin id : " + listSkins[i].index);

        string strRandomListOrder;

        if (!PlayerPrefs.HasKey("randomListOrder"))
        {
            //shuffle
            for (int i = 0; i < nbSkin; i++)
            {
                //strRandomListOrder +=

            }
        }
        else
            strRandomListOrder = PlayerPrefs.GetString("randomListOrder");
    }

    void Update()
    {
        
    }

    private class sort : IComparer<Skin>
    {
        int IComparer<Skin>.Compare(Skin _objA, Skin _objB)
        {
            int t1 = _objA.index;
            int t2 = _objB.index;
            return t1.CompareTo(t2);
        }
    }
}


