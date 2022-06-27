using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.Android;
using UnityEngine.UI;

public class GooglePlayServicesManager : MonoBehaviour
{
    public static GooglePlayServicesManager instance;
    public PlayGamesPlatform playGame;

    //[SerializeField] Image imgGoogle;
    //[SerializeField] Image imgAuthState;
    //[SerializeField] Text txtAuthCode;
    [SerializeField] FireBaseAuthScript firebaseAuthScript;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        //imgGoogle.color = Color.yellow;
        PlayGamesPlatform.Activate();
        //imgGoogle.color = Color.blue;
        if (!GetIsConnectedToGPGS())
            LoginToPlayGameServices();
        else
        {
            //imgAuthState.color = Color.green;
            TryConnectToFireBaseViaGooglePlay();
        }

    }

    /*private void Update()
    {
        imgAuthState.color = Color.yellow;
        if (PlayGamesPlatform.Instance.IsAuthenticated())
            imgAuthState.color = Color.green;
        else
            imgAuthState.color = Color.red;
    }*/

    public void ReportSucces(string _succesID, float _percent)
    {      
        PlayGamesPlatform.Instance.ReportProgress(_succesID, _percent, succes =>
        {
            if (succes)
                Debug.Log("succes : " + _succesID + ", reported : " + _percent);
            else
                Debug.Log("succes : " + _succesID + ", error to report : " + _percent);
        });
    }

    public void incrementSucces(string _succesID, int _steps)
    {
        PlayGamesPlatform.Instance.IncrementAchievement(_succesID, _steps, succes =>
        {
            if (succes)
                Debug.Log("succes : " + _succesID + ", reported : " + _steps);
            else
                Debug.Log("succes : " + _succesID + ", error to report : " + _steps);
        });
    }

    void TryConnectToFireBaseViaGooglePlay()
    {
        //imgAuthState.color = Color.green;
        PlayGamesPlatform.Instance.RequestServerSideAccess(false, code =>
        {
            print("code : " + code);
            //txtAuthCode.text = code; 
            firebaseAuthScript.ConnectToFireBaseViaGooglePlay(code); //send code to firebaseManager & connect with it
        });
    }

    void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            Debug.Log("successfully logged to play games services");
            //imgGoogle.color = Color.green;
        }
        if (status == SignInStatus.Canceled)
        {
            Debug.Log("manual SignInStatus.Canceled");
            //imgGoogle.color = Color.red;
        }
        if (status == SignInStatus.InternalError)
        {
            Debug.Log("manual SignInStatus.InternalError");
            //imgGoogle.color = Color.magenta;
        }
    }

    public void ManuallyConnect()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    public void LoginToPlayGameServices()
    {
        //imgGoogle.color = Color.blue;
        PlayGamesPlatform.Instance.Authenticate(success =>
        {
            if (success == SignInStatus.Success)
            {
                Debug.Log("SignInStatus.Success");
                //imgGoogle.color = Color.green;
                TryConnectToFireBaseViaGooglePlay();
            }
            if (success == SignInStatus.Canceled)
            {
                Debug.Log("SignInStatus.Canceled");
                //imgGoogle.color = Color.red;
                ManuallyConnect();
            }
            if (success == SignInStatus.InternalError)
            {
                Debug.Log("SignInStatus.InternalError");
                //imgGoogle.color = Color.magenta;
                ManuallyConnect();
            }
        });
    }

    public bool GetIsConnectedToGPGS()
    {
        return PlayGamesPlatform.Instance.IsAuthenticated();
    }
}
