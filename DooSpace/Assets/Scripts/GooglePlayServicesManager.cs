using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class GooglePlayServicesManager : MonoBehaviour
{
    public static PlayGamesPlatform playGame;

    [SerializeField] Image imgGoogle;
    [SerializeField] Text txtAuthCode;

    void Start()
    {
        imgGoogle.color = Color.yellow;
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
        //if (!PlayGamesPlatform.Instance.IsAuthenticated())
            //PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            // Continue with Play Games Services
            Debug.Log("successfully logged to play games services");
            imgGoogle.color = Color.green;
        }
        else
        {
            Debug.Log("failed to login play games services, manually auth");
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            //PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
            imgGoogle.color = Color.red;
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
        Social.localUser.Authenticate(success =>
        {
            if (success)
            {
                Debug.Log("successfully logged to play games services");
                imgGoogle.color = Color.green;
                PlayGamesPlatform.Instance.RequestServerSideAccess(false, code => { print("code : " + code); txtAuthCode.text = code; });
            }
            else
            {
                Debug.Log("failed to login play games services :(");
                imgGoogle.color = Color.red;
            }
        });
    }

    public void GetToken()
    {
        PlayGamesPlatform.Instance.RequestServerSideAccess(false, code =>  { print("code : " + code); txtAuthCode.text = code; });
    }
}
