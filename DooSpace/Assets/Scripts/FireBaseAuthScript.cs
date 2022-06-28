using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.UI;
using System;

public struct UserStruct
{
    public string name;
    public byte[] rocketPartId;
    public int score;
    public PlayerData data;
    public UserStruct(byte[] _rocketPartId, string _name, int _score) { rocketPartId = _rocketPartId; name = _name; score = _score; data = SkinManager.instance.GetPlayerData(); }
    public UserStruct(byte[] _rocketPartId, string _name, int _score, PlayerData _pData) { rocketPartId = _rocketPartId; name = _name; score = _score; data = _pData; }
}

public struct PlayerData
{
    public int currentSkinIndexToOpen;
    public string randomListOrder;

    public string strSkinPlayerOwn;
    public int nbSkinOwn;

    public string currentTopName;
    public string currentBodyName;
    public string currentWingsName;

    public PlayerData(int _currentSkinIndexToOpen, string _randomListOrder, string _strSkinPlayerOwn, int _nbSkinOwn, 
        string _currentTopName, string _currentBodyName, string _currentWingsName)
    {
        currentSkinIndexToOpen = _currentSkinIndexToOpen;
        randomListOrder = _randomListOrder;
        strSkinPlayerOwn = _strSkinPlayerOwn;
        nbSkinOwn = _nbSkinOwn;
        currentTopName = _currentTopName;
        currentBodyName = _currentBodyName;
        currentWingsName = _currentWingsName;
    }
}

public class FireBaseAuthScript : MonoBehaviour
{
    public static FireBaseAuthScript instance;
    [SerializeField] private HighscoreManager highscoreManager;

    [SerializeField] private GameObject popUpNewVersion;
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
    List<UserStruct> listScoresStruct = new List<UserStruct>();

    FirebaseAuth auth;
    DatabaseReference databaseRef;
    FirebaseUser localUser;

    int currentlocalPlayerRank = 0;
    UserStruct localUserStruct;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            localUserStruct = new UserStruct();
            auth = FirebaseAuth.DefaultInstance;
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
                Debug.LogError(message: $"failed to connect with {task.Exception}");
                //imgFirebase.color = Color.red;
                return;
            }

            Debug.Log("connected");
            //imgFirebase.color = Color.green;
            databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        });
    }

    public void SendScoreToDatabase(int _score)
    {
        int score = _score;
        byte[] rocketParts = new byte[3];
        rocketParts[0] = 12;
        rocketParts[1] = 0;
        rocketParts[2] = 24;
        UserStruct newUser = new UserStruct(rocketParts, localUser.DisplayName, score, SkinManager.instance.GetPlayerData());
        string toJson = JsonUtility.ToJson(newUser);
        Debug.Log(toJson);

        databaseRef.Child("Users").Child(newUser.name).SetRawJsonValueAsync(toJson).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled) { Debug.LogError("send to database canceled : " + task.Exception); return; };
            if (task.IsFaulted) { Debug.LogError("send to database faild : " + task.Exception); return; };
            if (task.IsCompleted) { Debug.Log("database data send !"); SendPlayerDataToDatabase(); };           
        });
    }

    public void SendPlayerDataToDatabase()
    {
        string toJson = JsonUtility.ToJson(SkinManager.instance.GetPlayerData());
        databaseRef.Child("Users").Child(localUser.DisplayName).Child("data").SetRawJsonValueAsync(toJson).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled) { Debug.LogError("pData send to database canceled : " + task.Exception); return; };
            if (task.IsFaulted) { Debug.LogError("pData send to database faild : " + task.Exception); return; };
            if (task.IsCompleted) { Debug.Log("database pData send !"); };
        });
    }

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
                Debug.Log("game version : " + strVersion);

                if (Application.version != strVersion)
                    popUpNewVersion.SetActive(true);
            }
        });
    }

    public void ReadDatabasePlayerData()
    {
        databaseRef.Child("Users").Child(localUser.DisplayName).Child("data").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled) { Debug.LogError("read playerdata from database canceled : " + task.Exception); return; };
            if (task.IsFaulted) { Debug.LogError("read playerdata from database faild : " + task.Exception); return; };
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                IDictionary dictData = (IDictionary)snapshot.Value;

                int currentSkinIndexToOpen = (int)dictData["currentSkinIndexToOpen"];
                string randomListOrder = dictData["randomListOrder"].ToString();

                string strSkinPlayerOwn = dictData["strSkinPlayerOwn"].ToString();
                int nbSkinOwn = (int)dictData["nbSkinOwn"];

                string currentTopName = dictData["currentTopName"].ToString();
                string currentBodyName = dictData["currentBodyName"].ToString();
                string currentWingsName = dictData["currentWingsName"].ToString();

                SkinManager.instance.LoadDatabasePlayerData(currentSkinIndexToOpen, randomListOrder, strSkinPlayerOwn, nbSkinOwn, currentTopName, currentBodyName, currentWingsName);
            }
        });
    }

    public void ReadFromDatabase()
    {
        databaseRef.Child("Users").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled) { Debug.LogError("read from database canceled : " + task.Exception); return; };
            if (task.IsFaulted) { Debug.LogError("read from database faild : " + task.Exception); return; };
            if (task.IsCompleted)
            {                
                listScoresStruct.Clear();
                Debug.Log("task completed");
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot user in snapshot.Children)
                {
                    UserStruct newUser = new UserStruct();
                    newUser.rocketPartId = new byte[3];

                    Debug.Log("user : " + user.Key);
                    IDictionary dictUser = (IDictionary)user.Value;

                    string name = dictUser["name"].ToString();
                    Debug.Log("name set : " + name);
                    newUser.name = name;
                    Debug.Log("name 2 set : " + newUser.name);

                    int score = int.Parse(dictUser["score"].ToString());
                    Debug.Log("score set : " + score);
                    newUser.score = score;
                    Debug.Log("score 2 set : " + newUser.score);

                    byte[] parts = new byte[3];
                    string strParts = user.Child("rocketPartId").GetRawJsonValue();
                    strParts = strParts.Substring(1, strParts.Length - 2);
                    string[] strPart = strParts.Split(',');
                    for (int i = 0; i < strPart.Length; i++)
                    {
                        Debug.Log("for : " + i);
                        parts[i] = byte.Parse(strPart[i]);
                        Debug.Log("for : " + i + "parsed");
                        newUser.rocketPartId[i] = parts[i];
                        Debug.Log("for : " + i + "part assigned");
                    }

                    if (newUser.name == localUser.DisplayName)
                    {
                        localUserStruct.name = newUser.name;
                        localUserStruct.rocketPartId = newUser.rocketPartId;
                        localUserStruct.score = newUser.score;

                    }

                    listScoresStruct.Add(newUser);
                }

                listScoresStruct.Sort((user1, user2) => user2.score.CompareTo(user1.score));

                localPlayerScoreFind = false;
                for (int i = 0; i < listScoresStruct.Count; i++)
                {
                    if(listScoresStruct[i].name == localUserStruct.name)
                    {
                        //localScoreReadedInDatabase = true;
                        localPlayerScoreFind = true;
                        int rank = i + 1;
                        //currentlocalPlayerScore = localUserStruct.score;
                        currentlocalPlayerRank = rank;
                        highscoreManager.SetLocalPlayerGlobalScore(rank, localUserStruct.name, localUserStruct.rocketPartId, localUserStruct.score);
                        break;
                    }
                }

                if (!localPlayerScoreFind)
                    highscoreManager.OpenLocalScoreNotYetSet();

                highscoreManager.UpdateGlobalScores();
            };
        });

        Debug.Log("end get user...");
    }

    public List<UserStruct> GetUsers()
    {
        return listScoresStruct;
    }

    public bool GetIsLocalPlayerScoreFind()
    {
        return localPlayerScoreFind;
    }

    /*public int GetCurrentPlayerScore()
    {
        return currentlocalPlayerScore;
    }*/

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

    public void ConnectToFireBaseViaGooglePlay(string _authCode)
    {
        authCode = _authCode;
        if (authCode == "")
        {
            Debug.LogError("google auth code empty");
            return;
        }

        Credential credential = PlayGamesAuthProvider.GetCredential(authCode);
        auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                //imgFirebaseWthGoogle.color = Color.green;
                Debug.Log("SignInWithCredentialAsync succes");
                ReadFromDatabase();
                ReadGameVersionFromDatabase();
                if (GetIsLocalPlayerScoreFind())
                    ReadDatabasePlayerData();
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
