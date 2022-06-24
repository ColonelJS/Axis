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
    public UserStruct(byte[] _rocketPartId, string _name, int _score) { rocketPartId = _rocketPartId; name = _name; score = _score; }
}

public class FireBaseAuthScript : MonoBehaviour
{
    [SerializeField] private HighscoreManager highscoreManager;
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
    List<UserStruct> listScoresStruct = new List<UserStruct>();

    FirebaseAuth auth;
    DatabaseReference databaseRef;
    FirebaseUser localUser;

    private void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;
        CheckAndFixFirebaseDependenciesThread();
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
        UserStruct newUser = new UserStruct(rocketParts, localUser.DisplayName, score);
        string toJson = JsonUtility.ToJson(newUser);
        Debug.Log(toJson);

        databaseRef.Child("Users").Child(newUser.name).SetRawJsonValueAsync(toJson).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled) { Debug.LogError("send to database canceled : " + task.Exception); return; };
            if (task.IsFaulted) { Debug.LogError("send to database faild : " + task.Exception); return; };
            if (task.IsCompleted) { Debug.Log("database data send !"); };           
        });
    }

    public void ReadFromDatabase()
    {
        Debug.Log("get user...");

        databaseRef.Child("Users").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled) { Debug.LogError("read from database canceled : " + task.Exception); return; };
            if (task.IsFaulted) { Debug.LogError("read from database faild : " + task.Exception); return; };
            if (task.IsCompleted)
            {
                UserStruct localUserStruct = new UserStruct();
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

                bool localPlayerScoreFind = false;
                for (int i = 0; i < listScoresStruct.Count; i++)
                {
                    if(listScoresStruct[i].name == localUserStruct.name)
                    {
                        localPlayerScoreFind = true;
                        highscoreManager.SetLocalPlayerGlobalScore(i+1, localUserStruct.name, localUserStruct.rocketPartId, localUserStruct.score);
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
            }
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync canceled.");
                //imgFirebaseWthGoogle.color = Color.yellow;
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync error: " + task.Exception);
                //imgFirebaseWthGoogle.color = Color.red;
                return;
            }

            localUser = task.Result;
            print("phone number : " + localUser.PhoneNumber);
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
