using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Purchasing;
//using Unity.Services.Core;
using System;

public class SeasonPassManager : MonoBehaviour//, IStoreListener
{
    public static SeasonPassManager instance;

    [SerializeField] private GameObject passPage;
    [SerializeField] private GameObject passButton;
    [SerializeField] private GameObject chestLeftButton;
    [SerializeField] private GameObject passNotif;
    [SerializeField] private GameObject passPopUp;
    [SerializeField] private GameObject chestLeftNotif;
    [HideInInspector]
    private static string supportPass = "com.gameacademy.axis.supportpass";

    bool isPassPageOpen = false;
    bool isPlayerRewarded = false;
    int chestToOpenLeft = 0;

    //IStoreListener storeActionResult;
    //IStoreController storeController;

    /*async void Awake()
    {
        if (instance == null)
        {
            instance = this;
            try
            {
                await UnityServices.InitializeAsync();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }*/

    /*void IStoreListener.OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        /*Product product = storeController.products.WithID(supportPass);
        if (product != null && product.availableToPurchase)
        {
            storeController.InitiatePurchase(product);
        }
    }*/

    /*void IStoreListener.OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError("error init gaming qservices : " + error.ToString());
    }

    void IStoreListener.OnInitializeFailed(InitializationFailureReason reason, string error)
    {
        Debug.LogError("error init gaming qservices : " + error.ToString());
    }

    PurchaseProcessingResult IStoreListener.ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        if (String.Equals(purchaseEvent.purchasedProduct.definition.id, supportPass, StringComparison.Ordinal))
        {
            Debug.Log("Purchased pass");
        }
        return PurchaseProcessingResult.Complete;
    }

    private void Start()
    {
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(supportPass, ProductType.NonConsumable);
        UnityPurchasing.Initialize(this, builder);
            
        SetupSeasonPass(ZPlayerPrefs.GetInt("hasPass", 0) == 1);
    }*/

    public void SetupSeasonPass(bool _hasPass)
    {
        //get firebase if is player already rewarded & chest to open left
        if(_hasPass)
        {
            ZPlayerPrefs.SetInt("hasPass", 1);
            //isPlayerRewarded = true;
            passButton.SetActive(false);
            if (chestToOpenLeft != 0)
            {
                chestLeftButton.SetActive(true);
                if (PlayerPrefs.GetInt("passChestOpened", 0) == 0)
                {
                    chestLeftNotif.SetActive(true);
                }
            }
        }
        else
        {
            ZPlayerPrefs.SetInt("hasPass", 0);
            passButton.SetActive(true);
            if (PlayerPrefs.GetInt("passPageOpened", 0) == 0)
            {
                passNotif.SetActive(true);
                passPopUp.SetActive(true);
                isPassPageOpen = true;
            }
        }
    }

    /*public void OnPurchaseComplete(Product product)
    {
        Debug.Log("purchase season pass successfull : " + product.definition.id);
        GivePlayerPassRewards();
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError("Purchase failed : " + failureReason);
    }*/

    public void OpenPassPage()
    {
        passPage.SetActive(true);
        isPassPageOpen = true;
        if (passNotif.activeSelf)
        {
            passNotif.SetActive(false);
            PlayerPrefs.SetInt("passPageOpened", 1);
        }

        if (passPopUp.activeSelf)
            passPopUp.SetActive(false);
    }

    public void GivePlayerPassRewards()
    {
        //isPlayerRewarded = true;
        //ZPlayerPrefs.SetInt("hasPass", 1);
        GameManager.instance.SetPlayerHasPass();
        FireBaseAuthScript.instance.SendSeasonPassValueData(true);

        //chests
        chestToOpenLeft = 8;

        //skins

        //money
        //int newMoney = CustomScreen.instance.GetPlayerMoney() + 20000;
        //CustomScreen.instance.SetNewMoney(newMoney);

        //ads

        //golden rank name

        //FireBaseAuthScript.instance.SendSeasonPassValueData(isPlayerRewarded);
        //FireBaseAuthScript.instance.SendSeasonPassChestsLeftData(chestToOpenLeft);


        SetupSeasonPass(true);
        ClosePassPage();
        //open gg page
    }

    public void ClosePassPage()
    {
        passPage.SetActive(false);
        isPassPageOpen = false;
    }

    public bool GetIsPassPageOpen()
    {
        return isPassPageOpen;
    }

    public void SetPassPageOpenFalse()
    {
        isPassPageOpen = false;
    }
}
