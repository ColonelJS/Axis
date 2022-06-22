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
    public PlayGamesPlatform playGame;

    [SerializeField] Image imgGoogle;
    [SerializeField] Image imgAuthState;
    [SerializeField] Text txtAuthCode;
    [SerializeField] FireBaseAuthScript firebaseAuthScript;

    string sAuthCode = "";

    void Start()
    {
        imgGoogle.color = Color.yellow;
        //playGame = PlayGamesPlatform.Activate();
        PlayGamesPlatform.Activate();
        imgGoogle.color = Color.blue;
        //if(playGame == null)
        //{
        /*PlayGamesClientConfiguration clientConfig = new PlayGamesClientConfiguration();
        PlayGamesClientConfiguration.Builder configBuilder = new PlayGamesClientConfiguration.Builder();
        clientConfig = configBuilder.Build();
        PlayGamesPlatform.InitializeInstance(clientConfig);
        PlayGamesPlatform.DebugLogEnabled = true;
        playGame = PlayGamesPlatform.Activate();*/

        /*PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().RequestServerAuthCode(false).Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();*/

        /*PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().RequestServerAuthCode(false).Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();*/

        //PlayGamesPlatform.Activate();
        //PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        //}



        if (!Social.localUser.authenticated)
            LoginToPlayGameServices();
    }

    private void Update()
    {
        imgAuthState.color = Color.yellow;
        if (PlayGamesPlatform.Instance.IsAuthenticated())
            imgAuthState.color = Color.green;
        else
            imgAuthState.color = Color.red;
    }

    void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            Debug.Log("successfully logged to play games services");
            imgGoogle.color = Color.green;
        }
        if (status == SignInStatus.Canceled)
        {
            Debug.Log("manual SignInStatus.Canceled");
            imgGoogle.color = Color.red;
        }
        if (status == SignInStatus.InternalError)
        {
            Debug.Log("manual SignInStatus.InternalError");
            imgGoogle.color = Color.magenta;
        }
    }

    public void ManuallyConnect()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        //PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
        //LoginToPlayGameServices();
    }

    public void LoginToPlayGameServices()
    {
        imgGoogle.color = Color.blue;
        PlayGamesPlatform.Instance.Authenticate(success =>
        {
            if (success == SignInStatus.Success)
            {
                Debug.Log("SignInStatus.Success");
                imgGoogle.color = Color.green;
                PlayGamesPlatform.Instance.RequestServerSideAccess(false, code => { print("code : " + code); txtAuthCode.text = code; firebaseAuthScript.ConnectToFireBaseViaGooglePlay(code); });               
            }
            if (success == SignInStatus.Canceled)
            {
                Debug.Log("SignInStatus.Canceled");
                imgGoogle.color = Color.red;
                ManuallyConnect();
            }
            if (success == SignInStatus.InternalError)
            {
                Debug.Log("SignInStatus.InternalError");
                imgGoogle.color = Color.magenta;
                ManuallyConnect();
            }
        });
        

        /*Social.localUser.Authenticate(success =>
        {
            if (success)
            {
                Debug.Log("successfully logged to play games services a");
                imgGoogle.color = Color.green;
                PlayGamesPlatform.Instance.RequestServerSideAccess(false, code => { print("code : " + code); txtAuthCode.text = code; });
            }
            else
            {
                Debug.Log("failed to login play games services :(");
                imgGoogle.color = Color.red;
            }
        });*/
    }

    public void GetToken()
    {
        LoginToPlayGameServices();
        print("username : " + Social.Active.localUser.userName);
        txtAuthCode.text = Social.Active.localUser.userName;

        var code = "";
        PlayGamesPlatform.Instance.RequestServerSideAccess(false, code => 
        {
            print("code : " + code);
            txtAuthCode.text = code;
        });
    }

    public string GetAuthCode()
    {
        if (sAuthCode != "")
            return sAuthCode;
        else
            return "auth code is empty";
    }
}
