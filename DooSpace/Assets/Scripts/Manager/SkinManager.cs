using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    //[SerializeField] private List<Sprite> baseShapes, topShapes, wingsShapes; //small medium large

    [SerializeField] private List<Sprite> baseShapeSmall, baseShapeMedium, baseShapeLarge;
    [SerializeField] private List<Sprite> topShapeSmall, topShapeMedium, topShapeLarge;
    [SerializeField] private List<Sprite> wingsShapeSmall, wingsShapeMedium, wingsShapeLarge;

    //List<List<Sprite>> listBaseShape = new List<List<Sprite>>();
    Dictionary<string, List<Sprite>> listBaseShape = new Dictionary<string, List<Sprite>>();

    void Start()
    {
        listBaseShape.Add("baseSmall", baseShapeSmall);
        listBaseShape.Add("baseMedium", baseShapeMedium);
        listBaseShape.Add("baseLarge", baseShapeLarge);

        listBaseShape.Add("topSmall", topShapeSmall);
        listBaseShape.Add("topMedium", topShapeMedium);
        listBaseShape.Add("topLarge", topShapeLarge);

        listBaseShape.Add("wingsSmall", wingsShapeSmall);
        listBaseShape.Add("wingsMedium", wingsShapeMedium);
        listBaseShape.Add("wingsLarge", wingsShapeLarge);
    }

    void Update()
    {
        
    }
}
