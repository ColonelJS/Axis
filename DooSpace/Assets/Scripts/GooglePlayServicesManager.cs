using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class GooglePlayServicesManager : MonoBehaviour
{
    public static PlayGamesPlatform playGame;
    public static string authCode = "";
    private bool isAuthenticating = false;
    float cooldown = 10f;

    void Update()
    {
        if (playGame == null)
        {
            /*PlayGamesClientConfiguration clientConfig = new PlayGamesClientConfiguration();
            PlayGamesClientConfiguration.Builder configBuilder = new PlayGamesClientConfiguration.Builder();
            clientConfig = configBuilder.Build();
            PlayGamesPlatform.InitializeInstance(clientConfig);
            PlayGamesPlatform.DebugLogEnabled = true;
            playGame = PlayGamesPlatform.Activate();*/

            /*PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().RequestServerAuthCode(false).Build());
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();*/
        }
        if (!Social.localUser.authenticated && authCode == "" && !isAuthenticating)
        {

            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().RequestServerAuthCode(false).Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.Activate();
            isAuthenticating = true;
            
        }

        if (cooldown <= 0)
        {
            LoginToPlayGameServices();
            cooldown = 10;
        }
        else
            cooldown -= Time.deltaTime;
    }

    public void LoginToPlayGameServices()
    {
        Social.localUser.Authenticate((success, message) =>
        {
            if (success)
            {
                authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                Debug.Log("successfully logged to play games services");
            }
            else
            {
                Debug.LogError("failed to login play games services :( " + message + " )");
            }
        });
    }
}
