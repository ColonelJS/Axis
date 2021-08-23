using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [Space(8)]
    [Header("Screens rect")]
    [SerializeField] RectTransform mainScreenRect;
    //[SerializeField] GameObject mainScreenElements;
    [SerializeField] RectTransform highscoreRect;
    [SerializeField] GameObject highscoreElements;
    [SerializeField] RectTransform customRect;
    [SerializeField] GameObject customElements;
    [SerializeField] RectTransform spaceRect;
    [SerializeField] RectTransform skyRect;
    [SerializeField] RectTransform earthRect;
    [Space(8)]
    [Header("Canvas scaler")]
    [SerializeField] CanvasScaler menusCanvasScaler;
    [SerializeField] CanvasScaler backgroundCanvasScaler;
    [SerializeField] CanvasScaler playerCanvasScaler;

    void Start()
    {
        SetupCanvasScaler();
        SetupScreenRect();
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

    void SetupCanvasScaler()
	{
        menusCanvasScaler.referenceResolution = GetResolution();
        backgroundCanvasScaler.referenceResolution = GetResolution();
        playerCanvasScaler.referenceResolution = GetResolution();
    }

    void SetupScreenRect()
	{
        mainScreenRect.sizeDelta = new Vector2(Screen.width, Screen.height);
        highscoreRect.sizeDelta = new Vector2(Screen.width, Screen.height);
        customRect.sizeDelta = new Vector2(Screen.width, Screen.height);

        float newScale = GetResolution().y / 2400;
        //mainScreenElements.transform.localScale = new Vector3(newScale, newScale, newScale);
        highscoreElements.transform.localScale = new Vector3(newScale, newScale, newScale);
        customElements.transform.localScale = new Vector3(newScale, newScale, newScale);

        spaceRect.sizeDelta = new Vector2(Screen.width, Screen.height);
        skyRect.sizeDelta = new Vector2(Screen.width, Screen.height);
        //earthRect.sizeDelta = new Vector2(Screen.height/2, Screen.height/2);
    }

    void SetupMainMenu()
	{
        bgUp.transform.localPosition = new Vector3(0, GetResolution().y/2, 0);
        //Debug.Log("pos y : " + )

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
