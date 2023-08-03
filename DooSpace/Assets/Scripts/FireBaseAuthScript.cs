using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.UI;
using System;
using UnityEngine.SocialPlatforms.Impl;

public struct UserStruct
{
    public PlayerScore score;
    public PlayerData data;

    public UserStruct(PlayerScore _score) { score = _score; data = SkinManager.instance.GetPlayerData(); }
    public UserStruct(PlayerScore _score, PlayerData _pData) { score = _score; data = _pData; }
}

public struct PlayerScore
{
    public string name;
    public RocketPartsStruct rocketPartId;
    public int score;
    public bool hasPass;

    public PlayerScore(RocketPartsStruct _rocketPartId, string _name, int _score, bool _hasPass)
    {
        rocketPartId = _rocketPartId;
        name = _name;
        score = _score;
        hasPass = _hasPass;
    } 
}

public struct PlayerData
{
    public string randomListOrder;
    public ChestData chestData;
    public int money;
    public int bumperLevel;
    public int wingLevel;

    public PlayerData(string _randomListOrder, ChestData _chestData, int _currentMoney, int _currentBumperLevel, int _currentWingsLevel)
    {
        randomListOrder = _randomListOrder;
        chestData = _chestData;
        money = _currentMoney;
        bumperLevel = _currentBumperLevel;
        wingLevel = _currentWingsLevel;
    }
}

public struct ChestData
{
    public int currentSkinIndexToOpen;
    public string strSkinPlayerOwn;
    public ChestData(int _currentSkinIndexToOpen, string _strSkinPlayerOwn) 
    { currentSkinIndexToOpen = _currentSkinIndexToOpen; strSkinPlayerOwn = _strSkinPlayerOwn; }
}

public struct RocketPartsStruct
{
    public byte _0;
    public byte _1;
    public byte _2;
}

public class FireBaseAuthScript : MonoBehaviour
{
    public static FireBaseAuthScript instance;
    [SerializeField] private HighscoreManager highscoreManager;

    [SerializeField] private GameObject popUpNewVersion;
    [SerializeField] private GameObject connexionToDBLoadingScreen;
    [SerializeField] private Button buttonClosePopUpNewVersion;
    //[SerializeField] private GameObject loginCanvas;
    //[SerializeField] private GameObject signInCanvas;

    //[SerializeField] private InputField inputFieldMailSignIn;
    //[SerializeField] private InputField inputFieldMdpSignIn;

    //[SerializeField] private InputField inputFieldMailLogin;
    //[SerializeField] private InputField inputFieldMdpLogin;
    //[SerializeField] Image imgFirebase;
    //[SerializeField] Image imgFirebaseWthGoogle;

    //bool isCanvasOpen = false;

    string authCode = "";

    bool isConnected = false;
    bool localPlayerScoreFind = false;
    bool isCurrentConnectToFB = false;
    List<UserStruct> listScoresStruct = new List<UserStruct>();

    FirebaseAuth auth;
    DatabaseReference databaseRef;
    FirebaseUser localUser;

    int currentlocalPlayerRank = 0;
    //byte[] rocketParts;
    UserStruct localUserStruct;
    RocketPartsStruct rocketPartsStruct;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            localUserStruct = new UserStruct();
            localUserStruct.score = new PlayerScore();
            auth = FirebaseAuth.DefaultInstance;
            //rocketParts = new byte[3];
            rocketPartsStruct = new RocketPartsStruct();
            CheckAndFixFirebaseDependenciesThread();
        }
    }

    void CheckAndFixFirebaseDependenciesThread()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(
        task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError(message: $"failed to Firebase check and fix dependencies async with exception :  {task.Exception}");
                //imgFirebase.color = Color.red;
                return;
            }

            Debug.Log("Firebase check and fix dependencies async succeded");
            //imgFirebase.color = Color.green;
            databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        });
    }

    //////////////////////////////////////////////////////////// <SEND> /////////////////////////////////////////////////////////////////
    #region SEND
    public void SendScoreToDatabase(int _score)
    {
        int score = _score;
        //rocketParts[0] = Convert.ToByte(SkinManager.instance.GetCurrentTopIndex());
        //rocketParts[1] = Convert.ToByte(SkinManager.instance.GetCurrentBodyIndex());
        //rocketParts[2] = Convert.ToByte(SkinManager.instance.GetCurrentWingsIndex());
        rocketPartsStruct._0 = Convert.ToByte(SkinManager.instance.GetCurrentTopIndex());
        rocketPartsStruct._1 = Convert.ToByte(SkinManager.instance.GetCurrentBodyIndex());
        rocketPartsStruct._2 = Convert.ToByte(SkinManager.instance.GetCurrentWingsIndex());

        PlayerScore newScore = new PlayerScore(rocketPartsStruct, localUser.DisplayName, score, GameManager.instance.GetPlayerHasSupportPass());
        string toJson = JsonUtility.ToJson(newScore);
        databaseRef.Child("Users").Child(localUser.UserId).Child("score").SetRawJsonValueAsync(toJson).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled) { Debug.LogError("send score to database canceled : " + task.Exception); return; };
            if (task.IsFaulted) { Debug.LogError("send score to database faild : " + task.Exception); return; };
            if (task.IsCompleted) { Debug.Log("database score send !"); SendRocketsParts(); };
        });
    }

    public void SendPlayerDataToDatabase()
    {
        string toJson = JsonUtility.ToJson(SkinManager.instance.GetPlayerData());
        databaseRef.Child("Users").Child(localUser.UserId).Child("data").SetRawJsonValueAsync(toJson).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled) { Debug.LogError("pData send to database canceled : " + task.Exception); return; };
            if (task.IsFaulted) { Debug.LogError("pData send to database faild : " + task.Exception); return; };
            if (task.IsCompleted)
            {
                Debug.Log("database pData send !");
                if (!localPlayerScoreFind)
                    SendPlayerChestDataToDatabase();
            };
        });
    }

    public void SendPlayerChestDataToDatabase()
    {
        string toJson = JsonUtility.ToJson(SkinManager.instance.GetPlayerData().chestData);
        databaseRef.Child("Users").Child(localUser.UserId).Child("data").Child("chestData").SetRawJsonValueAsync(toJson).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled) { Debug.LogError("pData send to database canceled : " + task.Exception); return; };
            if (task.IsFaulted) { Debug.LogError("pData send to database faild : " + task.Exception); return; };
            if (task.IsCompleted) { Debug.Log("database pChestData send !"); };
        });
    }

    void SendRocketsParts()
    {
        string toJson = JsonUtility.ToJson(rocketPartsStruct);
        databaseRef.Child("Users").Child(localUser.UserId).Child("score").Child("rocketPartId").SetRawJsonValueAsync(toJson).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled) { Debug.LogError("send rockets parts to database canceled : " + task.Exception); return; };
            if (task.IsFaulted) { Debug.LogError("send rockets parts to database faild : " + task.Exception); return; };
            if (task.IsCompleted) { Debug.Log("database rockets parts send !"); };
        });
    }

    public void SendRocketSkinChanged(SkinManager.PartType _partType, int _index)
    {
        byte bIndex = Convert.ToByte(_index);
        switch (_partType)
        {
            case SkinManager.PartType.TOP:
                //rocketParts[0] = bIndex;
                rocketPartsStruct._0 = bIndex;
                break;
            case SkinManager.PartType.BASE:
                //rocketParts[1] = bIndex;
                rocketPartsStruct._1 = bIndex;
                break;
            case SkinManager.PartType.WINGS:
                //rocketParts[2] = bIndex;
                rocketPartsStruct._2 = bIndex;
                break;
            default:
                break;
        }

        string toJson = JsonUtility.ToJson(rocketPartsStruct);
        databaseRef.Child("Users").Child(localUser.UserId).Child("score").Child("rocketPartId").SetRawJsonValueAsync(toJson).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled) { Debug.LogError("send rockets parts to database canceled : " + task.Exception); return; };
            if (task.IsFaulted) { Debug.LogError("send rockets parts to database faild : " + task.Exception); return; };
            if (task.IsCompleted) { Debug.Log("database rockets parts send !"); };
        });
    }

    public void SendPlayerChestData(ChestData _chestData)
    {
        string toJson = JsonUtility.ToJson(_chestData);
        databaseRef.Child("Users").Child(localUser.UserId).Child("data").Child("chestData").SetRawJsonValueAsync(toJson).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled) { Debug.LogError("send _chestData to database canceled : " + task.Exception); return; };
            if (task.IsFaulted) { Debug.LogError("send _chestData parts to database faild : " + task.Exception); return; };
            if (task.IsCompleted) { Debug.Log("database _chestData parts send !"); };
        });
    }

    public void SendSeasonPassValueData(bool _hasSeasonPass)
    {
        databaseRef.Child("Users").Child(localUser.UserId).Child("score").Child("hasPass").SetValueAsync(_hasSeasonPass);
    }

    public void SendSeasonPassChestsLeftData(int _nbChestsLeft)
    {
        databaseRef.Child("Users").Child(localUser.UserId).Child("data").Child("spChestsLeft").SetValueAsync(_nbChestsLeft);
    }

    public void SendPlayerMoneyData(int _newMoney)
    {
        databaseRef.Child("Users").Child(localUser.UserId).Child("data").Child("money").SetValueAsync(_newMoney);
    }

    public void SendPlayerBumperLevelData(int _newBumperLevel)
    {
        databaseRef.Child("Users").Child(localUser.UserId).Child("data").Child("bumperLevel").SetValueAsync(_newBumperLevel);
    }

    public void SendPlayerWingLevelData(int _newWingLevel)
    {
        databaseRef.Child("Users").Child(localUser.UserId).Child("data").Child("wingLevel").SetValueAsync(_newWingLevel);
    }

    #endregion
    //////////////////////////////////////////////////////////// <!SEND> /////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////// <READ> /////////////////////////////////////////////////////////////////
    #region READ
    public void ReadGameVersionFromDatabase()
    {
        databaseRef.Child("Update").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled) { Debug.LogError("read game version from database canceled : " + task.Exception); return; };
            if (task.IsFaulted) { Debug.LogError("read game version from database faild : " + task.Exception); return; };
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                IDictionary listResult = (IDictionary)snapshot.Value;

                string strVersion = listResult["Version"].ToString();
                bool isMandatory = Convert.ToBoolean(listResult["IsMandatory"]);
                Debug.Log("game version : " + strVersion);

                if (Application.version != strVersion)
                {
                    popUpNewVersion.SetActive(true);
                    SetPopUpnewVersionMandatory(isMandatory);
                }
                else
                {
                    if(popUpNewVersion.activeSelf)
                        popUpNewVersion.SetActive(false);
                }
            }
        });
    }

    public void ReadFromDatabase(bool _readDataToo)
    {
        databaseRef.Child("Users").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled) { Debug.LogError("read from database canceled : " + task.Exception); return; };
            if (task.IsFaulted) { Debug.LogError("read from database faild : " + task.Exception); return; };
            if (task.IsCompleted)
            {
                listScoresStruct.Clear();
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot user in snapshot.Children)
                {
                    UserStruct newUser = new UserStruct();
                    newUser.score = new PlayerScore();
                    newUser.score.rocketPartId = new RocketPartsStruct();

                    IDictionary dictUser = (IDictionary)user.Child("score").Value;

                    newUser.score.name = dictUser["name"].ToString();
                    newUser.score.score = int.Parse(dictUser["score"].ToString());
                    newUser.score.hasPass = bool.Parse(dictUser["hasPass"].ToString());

                    string strParts = user.Child("score").Child("rocketPartId").GetRawJsonValue();
                    strParts = strParts.Substring(1, strParts.Length - 2);
                    string[] strPart = strParts.Split(',');
                    int index = 0;
                    for (int i = 0; i < strPart.Length; i++)
                    {
                        string curPart = strPart[i].Substring(5, strPart[i].Length - 5);
                        if(index == 0)
                            newUser.score.rocketPartId._0 = byte.Parse(curPart);
                        if (index == 1)
                            newUser.score.rocketPartId._1 = byte.Parse(curPart);
                        if (index == 2)
                            newUser.score.rocketPartId._2 = byte.Parse(curPart);
                        index++;
                    }

                    if (newUser.score.name == localUser.DisplayName)
                    {
                        localUserStruct.score.name = newUser.score.name;
                        localUserStruct.score.rocketPartId = newUser.score.rocketPartId;
                        rocketPartsStruct = localUserStruct.score.rocketPartId;
                        SkinManager.instance.SetCurrentSkinParts(localUserStruct.score.rocketPartId);
                        localUserStruct.score.score = newUser.score.score;
                        localUserStruct.score.hasPass = newUser.score.hasPass;
                        //SeasonPassManager.instance.SetupSeasonPass(localUserStruct.score.hasPass);

                        if (_readDataToo)
                        {
                            IDictionary dictUserData = (IDictionary)user.Child("data").Value;

                            string randomListOrder = dictUserData["randomListOrder"].ToString();
                            int newMoney = int.Parse(dictUserData["money"].ToString());
                            int newBumperLevel = int.Parse(dictUserData["bumperLevel"].ToString());
                            int newWingLevel = int.Parse(dictUserData["wingLevel"].ToString());

                            IDictionary dictUserChestData = (IDictionary)user.Child("data").Child("chestData").Value;

                            int currentSkinIndexToOpen = int.Parse(dictUserChestData["currentSkinIndexToOpen"].ToString());
                            string strSkinPlayerOwn = dictUserChestData["strSkinPlayerOwn"].ToString();

                            SkinManager.instance.LoadDatabasePlayerData(currentSkinIndexToOpen, randomListOrder, strSkinPlayerOwn, newMoney, newBumperLevel, newWingLevel);
                        }
                    }

                    listScoresStruct.Add(newUser);
                }

                listScoresStruct.Sort((user1, user2) => user2.score.score.CompareTo(user1.score.score));

                localPlayerScoreFind = false;
                for (int i = 0; i < listScoresStruct.Count; i++)
                {
                    if(listScoresStruct[i].score.name == localUserStruct.score.name)
                    {
                        localPlayerScoreFind = true;
                        currentlocalPlayerRank = i + 1;

                        highscoreManager.SetLocalPlayerGlobalScore(currentlocalPlayerRank, localUserStruct.score.name, localUserStruct.score.rocketPartId, localUserStruct.score.score, localUserStruct.score.hasPass);
                        break;
                    }
                }

                if (!localPlayerScoreFind)
                    highscoreManager.OpenLocalScoreNotYetSet();

                highscoreManager.UpdateGlobalScores();
            };
        });
    }
    #endregion
    //////////////////////////////////////////////////////////// <!READ> /////////////////////////////////////////////////////////////////

    public void ClosePopUpNewVersion()
    {
        popUpNewVersion.SetActive(false);
    }

    public bool GetPopUpNewVersionOpen()
    {
        return popUpNewVersion.activeSelf;
    }

    void SetPopUpnewVersionMandatory(bool _isMandatory)
    {
        buttonClosePopUpNewVersion.interactable = !_isMandatory;
    }

    public List<UserStruct> GetUsers()
    {
        return listScoresStruct;
    }

    public bool GetIsLocalPlayerScoreFind()
    {
        return localPlayerScoreFind;
    }

    public UserStruct GetCurrentPlayer()
    {
        return localUserStruct;
    }

    public int GetCurrentPlayerRank()
    {
        return currentlocalPlayerRank;
    }

    public void CreateUser()
    {
        /*string email = inputFieldMailSignIn.text;
        string mdp = inputFieldMdpSignIn.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, mdp).ContinueWith(
            task =>
            {
              if (task.IsCanceled) { Debug.LogError("create user failed"); return; };
              if (task.IsFaulted) { Debug.LogError("create user exception : " + task.Exception); return; };

              FirebaseUser newUser = task.Result;
              Debug.LogFormat("user created - Mail: {0}, Mdp: {1}", email, mdp);
            });*/
    }

    public bool GetIsCurrentlyConnectToDB()
    {
        return isCurrentConnectToFB;
    }

    public void ConnectToFireBaseViaGooglePlay(string _authCode)
    {
        authCode = _authCode;
        if (authCode == "")
        {
            Debug.LogError("google auth code empty");
            return;
        }

        Credential credential = PlayGamesAuthProvider.GetCredential(authCode);
        connexionToDBLoadingScreen.SetActive(true);
        isCurrentConnectToFB = true;
        auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task => {
            connexionToDBLoadingScreen.SetActive(false);
            isCurrentConnectToFB = false;
            if (task.IsCompleted)
            {
                //imgFirebaseWthGoogle.color = Color.green;
                Debug.Log("SignInWithCredentialAsync succes");
                ReadFromDatabase(true);
                ReadGameVersionFromDatabase();
            }
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync canceled.");
                isConnected = false;
                //imgFirebaseWthGoogle.color = Color.yellow;
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync error: " + task.Exception);
                isConnected = false;
                //imgFirebaseWthGoogle.color = Color.red;
                return;
            }

            localUser = task.Result;
            //print("phone number : " + localUser.PhoneNumber);
            isConnected = true;
            Debug.LogFormat("User signed in successfully: {0} ({1})", localUser.DisplayName, localUser.UserId);
        });
    }

    public bool GetIsConnectedToFireBase()
    {
        return isConnected;
    }

    public bool GetIsConnectedToGPGSAndFirebase()
    {
        if (GooglePlayServicesManager.instance.GetIsConnectedToGPGS())
        {
            if (isConnected)
                return true;
            else
            {
                Debug.LogError("not connected to firebase database");
                return false;
            }
        }
        else
        {
            Debug.LogError("not connected to gpgs");
            return false;
        }
    }

    public void OpenLoginCanvas()
    {
        /*if (!isCanvasOpen)
        {
            loginCanvas.SetActive(true);
            signInCanvas.SetActive(false);
            isCanvasOpen = true;
        }
        else
            CloseCanvas();*/
    }

    public void OpenSignInCanvas()
    {
        /*if (!isCanvasOpen)
        {
            loginCanvas.SetActive(false);
            signInCanvas.SetActive(true);
            isCanvasOpen = true;
        }
        else
            CloseCanvas();*/
    }

    void CloseCanvas()
    {
        /*loginCanvas.SetActive(false);
        signInCanvas.SetActive(false);
        isCanvasOpen = false;*/
    }
}
