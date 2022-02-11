using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class GooglePlayServicesManager : MonoBehaviour
{
    public static PlayGamesPlatform playGame;

    void Start()
    {
        if(playGame == null)
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

            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().RequestServerAuthCode(false).Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.Activate();
        }

        if (!Social.localUser.authenticated)
            LoginToPlayGameServices();
    }

    public void LoginToPlayGameServices()
    {
        Social.localUser.Authenticate(success =>
        {
            if (success)
                Debug.Log("successfully logged to play games services");
            else
                Debug.Log("failed to login play games services :(");
        });
    }
}
