using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsPopUp : MonoBehaviour
{
    [SerializeField] private GameObject popUp;

    void Start()
    {
        ClosePopUp();
    }

    void Update()
    {
        
    }

    public void OpenPopUp()
    {
        Debug.LogError("OPEN POP UP CALL");
        popUp.SetActive(true);
    }

    public void ClosePopUp()
    {
        Debug.LogError("CLOSE POP UP CALL");
        popUp.SetActive(false);
    }

    public void WatchDoubleCoinAd()
    {
        AdManager.instance.UserChoseToWatchAd(AdManager.instance.doubleCoinsAd);
    }
    public void WatchReviveAd()
    {
        AdManager.instance.UserChoseToWatchAd(AdManager.instance.reviveAd);
    }

}
