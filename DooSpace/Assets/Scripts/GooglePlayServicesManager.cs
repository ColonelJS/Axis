using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class GooglePlayServicesManager : MonoBehaviour
{
    [SerializeField] private Text textStatus;
    private PlayGamesClientConfiguration clientConfiguration;

    public static PlayGamesPlatform playGame;
    public static string authCode = "";
    private bool isAuthenticating = false;
    float cooldown = 10f;

    private void Start()
    {
        ConfigureGPGS();
        AuthentificateToGPGS(SignInInteractivity.CanPromptOnce, clientConfiguration);
    }

    void Update()
    {
        /*if (playGame == null)
        {
            PlayGamesClientConfiguration clientConfig = new PlayGamesClientConfiguration();
            PlayGamesClientConfiguration.Builder configBuilder = new PlayGamesClientConfiguration.Builder();
            clientConfig = configBuilder.Build();
            PlayGamesPlatform.InitializeInstance(clientConfig);
            PlayGamesPlatform.DebugLogEnabled = true;
            playGame = PlayGamesPlatform.Activate();*/

            /*PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().RequestServerAuthCode(false).Build());
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
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
            cooldown -= Time.deltaTime;*/
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

    private void ConfigureGPGS()
    {
        clientConfiguration = new PlayGamesClientConfiguration.Builder().Build();
    }

    private void AuthentificateToGPGS(SignInInteractivity _interactivity, PlayGamesClientConfiguration _configuration)
    {
        _configuration = clientConfiguration;
        PlayGamesPlatform.InitializeInstance(_configuration);
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(_interactivity, (code) =>
        {
            textStatus.text = "Authentificating...";
            switch (code)
            {
                case SignInStatus.Success:
                    textStatus.text = "Successfully Authentificated";
                    break;
                case SignInStatus.UiSignInRequired:
                    textStatus.text = "UiSignInRequired";
                    break;
                case SignInStatus.DeveloperError:
                    textStatus.text = "DeveloperError";
                    break;
                case SignInStatus.NetworkError:
                    textStatus.text = "NetworkError";
                    break;
                case SignInStatus.InternalError:
                    textStatus.text = "InternalError";
                    break;
                case SignInStatus.Canceled:
                    textStatus.text = "Canceled";
                    break;
                case SignInStatus.AlreadyInProgress:
                    textStatus.text = "AlreadyInProgress";
                    break;
                case SignInStatus.Failed:
                    textStatus.text = "Failed";
                    break;
                case SignInStatus.NotAuthenticated:
                    textStatus.text = "NotAuthenticated";
                    break;
                default:
                    break;
            }
        });
    }

    public void AuthentificateButton()
    {
        AuthentificateToGPGS(SignInInteractivity.CanPromptAlways, clientConfiguration);
    }

    public void SignOut()
    {
        PlayGamesPlatform.Instance.SignOut();
        textStatus.text = "signed out";
    }
}
