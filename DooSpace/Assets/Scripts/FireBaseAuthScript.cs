using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.UI;
using System;

[Serializable]
public struct UserStruct
{
    public string name;
    public byte[] rocketPartId;
    public int score;
    public UserStruct(byte[] _rocketPartId, string _name, int _score) { rocketPartId = _rocketPartId; name = _name; score = _score; }
}

[System.Serializable]
public class UserClass
{
    public string name;
    public byte[] rocketPartId;
    public int score;
}

[Serializable]
public class PlayersScore
{
    public User[] user;
}

[Serializable]
public class User
{

    public UserClass userInfo;
}

public class FireBaseAuthScript : MonoBehaviour
{
    [SerializeField] private GameObject loginCanvas;
    [SerializeField] private GameObject signInCanvas;

    [SerializeField] private InputField inputFieldMailSignIn;
    [SerializeField] private InputField inputFieldMdpSignIn;

    [SerializeField] private InputField inputFieldMailLogin;
    [SerializeField] private InputField inputFieldMdpLogin;

    bool isCanvasOpen = false;

    string authCode = "";
    long nbScores = 0;
    string jsonScores = "";
    FirebaseAuth auth;
    DatabaseReference databaseRef;
    [SerializeField] Image imgFirebase;
    [SerializeField] Image imgFirebaseWthGoogle;

    /*struct UserStruct
    {
        public byte[] rocketPartId;
        public string name;
        public int score;
        public UserStruct(byte[] _rocketPartId, string _name, int _score) {rocketPartId = _rocketPartId; name = _name; score = _score; }
    }*/

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
                imgFirebase.color = Color.red;
                return;
            }

            Debug.Log("connected");
            imgFirebase.color = Color.green;
            databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        });
    }

    public void SendScoreToDatabase()
    {
        byte[] rocketParts = new byte[3];
        rocketParts[0] = 12;
        rocketParts[1] = 0;
        rocketParts[2] = 24;
        UserStruct newUser = new UserStruct(rocketParts, "Cjss", 55555);
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
        databaseRef.Child("Users").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled) { Debug.LogError("read from database canceled : " + task.Exception); return; };
            if (task.IsFaulted) { Debug.LogError("read from database faild : " + task.Exception); return; };
            if (task.IsCompleted)
            { 
                Debug.Log("database data readed : " + task.Result.GetRawJsonValue());
                Debug.Log("nombre de scores dans database : " + task.Result.ChildrenCount);//thread
                nbScores = task.Result.ChildrenCount;// sinon = 0
                jsonScores = task.Result.GetRawJsonValue();
                GetUsers();
                //SetupScoresToDraw();
            };
        });
        Debug.Log("read from database after thread lambda");
    }

    void SetupScoresToDraw()
    {
        Debug.Log("setup...");

        Debug.Log("json read : " + jsonScores);
        //newUsers = JsonUtility.FromJson<UserStruct[]>(jsonScores);
        //UserStruct[] newUsers = new UserStruct[nbScores];
        //PlayerScore[] listPlayerScore = new PlayerScore[nbScores];
        /* UserClass[] listUserClass = new UserClass[nbScores];
         for (int i = 0; i < listUserClass.Length; i++)
         {
             Debug.Log("for 1.0 : " + i);
             listUserClass[i] = new UserClass();
             Debug.Log("for 1.1 : " + i);
             listUserClass[i].rocketPartId = new byte[3];
             Debug.Log("for 1.2 : " + i);
             Debug.Log("for 1.1 : " + i);
             listPlayerScore[i].userElements = new UserClass();
             Debug.Log("for 1.2 : " + i);
             listPlayerScore[i].userElements.rocketPartId = new byte[3];
             Debug.Log("for 1.3 : " + i);
         }*/

        PlayersScore playersScore = new PlayersScore();
        playersScore.user = new User[nbScores];

        for (int i = 0; i < playersScore.user.Length; i++)
        {
            playersScore.user[i] = new User();
            Debug.Log("for 1.0 : " + i);
            playersScore.user[i].userInfo = new UserClass();
            Debug.Log("for 1.1 : " + i);
            playersScore.user[i].userInfo.rocketPartId = new byte[3];
            Debug.Log("for 1.2 : " + i);
        }
        Debug.Log("end for 1");

        playersScore = JsonUtility.FromJson<PlayersScore>(jsonScores);

        //JsonUtility.FromJsonOverwrite(jsonScores, playersScore);
        //UserStruct[] newUsers = JsonUtility.FromJson<UserStruct[]>(jsonScores);

        //PlayerScore[] listPlayerScore = JsonUtility.FromJson<PlayerScore[]>(jsonScores);
        Debug.Log("nb user : " + playersScore.user.Length);

        for (int i = 0; i < playersScore.user.Length; i++)
        {
            Debug.Log("for 2 : " + i);
            Debug.Log("user no: " + i + ", name : " + playersScore.user[i].userInfo.name + ", score : " + playersScore.user[i].userInfo.score + ", part0 : " + 
                playersScore.user[i].userInfo.rocketPartId[0] + ", part1 : " + playersScore.user[i].userInfo.rocketPartId[1] + ", part2 : " + playersScore.user[i].userInfo.rocketPartId[2]);
        }    
    }

    public void GetUsers()
    {
        Debug.Log("get user...");

        databaseRef.Child("Users").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
                Debug.Log("task canceled");
            if (task.IsFaulted)
            {
                Debug.Log("task faulted");
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                Debug.Log("task completed");
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot user in snapshot.Children)
                {
                    Debug.Log("user : " + user.Key);
                    IDictionary dictUser = (IDictionary)user.Value;
                    Debug.Log("dict created");

                    var aa = dictUser["name"];
                    Debug.Log("aa created");
                    var bb = dictUser["score"];
                    Debug.Log("bb created");
                    var cc = dictUser["rocketPartId"];
                    Debug.Log("cc created");

                    byte[] parts = new byte[3];
                    Debug.Log("byte created");

                    Debug.Log("name : " + aa);
                    Debug.Log("score : " + bb);
                    Debug.Log("parts : " + cc);

                    parts = (byte[])cc;

                    Debug.Log("parts 0 : " + parts[0]);
                    Debug.Log("parts 1 : " + parts[1]);
                    Debug.Log("parts 2 : " + parts[2]);

                    //parts = dictUser["rocketPartId"];
                    //Debug.Log("parts assigned");
                    Debug.Log("name : " + (string)dictUser["name"]);
                    Debug.Log("user : " + user.Key + ", name : " + dictUser["name"] + ", score : " + dictUser["score"]
                        + ", part0 : " + parts[0] + ", part1 : " + parts[1] + ", part2 : " + parts[2]);
                }
            }
        });
        Debug.Log("end get user...");
    }

    public void CreateUser()
    {
        string email = inputFieldMailSignIn.text;
        string mdp = inputFieldMdpSignIn.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, mdp).ContinueWith(
            task =>
            {
              if (task.IsCanceled) { Debug.LogError("create user failed"); return; };
              if (task.IsFaulted) { Debug.LogError("create user exception : " + task.Exception); return; };

              FirebaseUser newUser = task.Result;
              Debug.LogFormat("user created - Mail: {0}, Mdp: {1}", email, mdp);
            });
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
                imgFirebaseWthGoogle.color = Color.green;
                Debug.LogError("SignInWithCredentialAsync succes");
            }
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync canceled.");
                imgFirebaseWthGoogle.color = Color.yellow;
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync error: " + task.Exception);
                imgFirebaseWthGoogle.color = Color.red;
                return;
            }

            FirebaseUser newUser = task.Result;

            Debug.LogFormat("User signed in successfully: {0} ({1})",newUser.DisplayName, newUser.UserId);
        });

    }

    public void OpenLoginCanvas()
    {
        if (!isCanvasOpen)
        {
            loginCanvas.SetActive(true);
            signInCanvas.SetActive(false);
            isCanvasOpen = true;
        }
        else
            CloseCanvas();
    }

    public void OpenSignInCanvas()
    {
        if (!isCanvasOpen)
        {
            loginCanvas.SetActive(false);
            signInCanvas.SetActive(true);
            isCanvasOpen = true;
        }
        else
            CloseCanvas();
    }

    void CloseCanvas()
    {
        loginCanvas.SetActive(false);
        signInCanvas.SetActive(false);
        isCanvasOpen = false;
    }
}
