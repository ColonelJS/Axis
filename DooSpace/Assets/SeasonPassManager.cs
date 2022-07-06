using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class SeasonPassManager : MonoBehaviour
{
    public string seasonPass = "com.GameAcademy.Axis.seasonPass";

    public void OnPurchaseComplete(Product product)
    {
        Debug.Log("purchase season pass successfull : " + product.definition.id);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError("Purchase failed : " + failureReason);
    }

}
