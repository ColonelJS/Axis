using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class GooglePlayServicesManager : MonoBehaviour
{
    [SerializeField] private FireBaseAuthScript firebaseManager;
    [SerializeField] private Text textStatus;
    private PlayGamesClientConfiguration clientConfiguration;

    bool isAuthentificated = false;
    static string authCode = "";

    private void Start()
    {
        ConfigureGPGS();
        AuthentificateToGPGS(SignInInteractivity.CanPromptOnce, clientConfiguration);
    }

    void Update()
    {

    }

    private void ConfigureGPGS()
    {
        clientConfiguration = new PlayGamesClientConfiguration.Builder().RequestServerAuthCode(false).Build(); //error with request added
    }

    private void AuthentificateToGPGS(SignInInteractivity _interactivity, PlayGamesClientConfiguration _configuration)
    {
        _configuration = clientConfiguration;
        PlayGamesPlatform.InitializeInstance(_configuration);
        PlayGamesPlatform.Activate();

        /*Social.localUser.Authenticate(succes =>
        {
            if(succes)
            {
                authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                textStatus.text = "Success, code : " + authCode;
                isAuthentificated = true;
                firebaseManager.SetIsTryToAuth();
                return;
            }
        });*/

        PlayGamesPlatform.Instance.Authenticate(_interactivity, (code) =>
        {
            textStatus.text = "Authentificating to gpgs...";
            switch (code)
            {
                case SignInStatus.Success:

                    authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                    textStatus.text = "Success, code : " + authCode;
                    isAuthentificated = true;

                    firebaseManager.SetIsTryToAuth();

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

    public string GetServerAuthCode()
    {
        if (authCode == "")
            return authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
        else
            return authCode;
    }

    public bool GetIsAuthentificated()
    {
        return isAuthentificated;
    }

    public string GetAuthCode()
    {
        return authCode;
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
