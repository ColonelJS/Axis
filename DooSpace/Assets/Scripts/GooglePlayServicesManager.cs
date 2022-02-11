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
            PlayGamesClientConfiguration clientConfig = new PlayGamesClientConfiguration();
            PlayGamesClientConfiguration.Builder configBuilder = new PlayGamesClientConfiguration.Builder();
            clientConfig = configBuilder.Build();
            PlayGamesPlatform.InitializeInstance(clientConfig);
            PlayGamesPlatform.DebugLogEnabled = true;
            playGame = PlayGamesPlatform.Activate();
        }

        if (!Social.Active.localUser.authenticated)
            LoginToPlayGameServices();
    }

    public void LoginToPlayGameServices()
    {
        Social.Active.localUser.Authenticate(success =>
        {
            if (success)
                Debug.Log("successfully logged to play games services");
            else
                Debug.Log("failed to login play games services :(");
        });
    }
}
