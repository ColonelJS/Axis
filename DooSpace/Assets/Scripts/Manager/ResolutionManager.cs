using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    [Header("Main screen")]
    [SerializeField] GameObject bgUp;
    [SerializeField] RectTransform bgUpRectTransform;
    [SerializeField] RectTransform bgDownRectTransform;
    [Space(8)]
    [Header("Settings")]
    [SerializeField] GameObject bgInfo;
    [SerializeField] RectTransform bgInfoRectTransform;
    [SerializeField] RectTransform bgInfoScrollViewRectTransform;
    [Space(6)]
    [SerializeField] GameObject bgCredits;
    [SerializeField] RectTransform bgCreditsRectTransform;
    [SerializeField] RectTransform bgCreditsScrollViewRectTransform;

    void Start()
    {
        SetupMainMenu();
        SetupSettings();
    }

    void Update()
    {
        
    }

    Vector2 GetResolution()
	{
        Vector2 res;
        res.x = Screen.width;  //.currentResolution.width;
        res.y = Screen.height; //.currentResolution.height;
        return res;
	}

    void SetupMainMenu()
	{
        bgUp.transform.position = new Vector3(bgUp.transform.position.x, GetResolution().y/2, bgUp.transform.position.z);

        bgUpRectTransform.sizeDelta = new Vector2(GetResolution().x, GetResolution().y / 2);
        bgDownRectTransform.sizeDelta = new Vector2(GetResolution().x, GetResolution().y / 2);
    }

    void SetupSettings()
	{
        bgInfo.transform.position = new Vector3(bgInfo.transform.position.x, 200, bgInfo.transform.position.z);
        bgCredits.transform.position = bgInfo.transform.position;

        bgInfoRectTransform.sizeDelta = GetResolution();
        bgCreditsRectTransform.sizeDelta = GetResolution();

        bgInfoScrollViewRectTransform.sizeDelta = new Vector2(GetResolution().x, GetResolution().y / 1.4235f);
        bgCreditsScrollViewRectTransform.sizeDelta = bgInfoScrollViewRectTransform.sizeDelta;

        bgInfoScrollViewRectTransform.localPosition = new Vector3(bgInfoScrollViewRectTransform.localPosition.x, 
            GetResolution().y + (GetResolution().y * (-1326f) / 2400f), bgInfoScrollViewRectTransform.localPosition.z);

        bgCreditsScrollViewRectTransform.localPosition = bgInfoScrollViewRectTransform.localPosition;
    }
}
