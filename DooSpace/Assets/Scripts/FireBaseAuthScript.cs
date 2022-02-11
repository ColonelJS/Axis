using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine.UI;

public class FireBaseAuthScript : MonoBehaviour
{
    [SerializeField] private GameObject loginCanvas;
    [SerializeField] private GameObject signInCanvas;

    [SerializeField] private InputField inputFieldMailSignIn;
    [SerializeField] private InputField inputFieldMdpSignIn;

    [SerializeField] private InputField inputFieldMailLogin;
    [SerializeField] private InputField inputFieldMdpLogin;

    bool isCanvasOpen = false;

    FirebaseAuth auth;
    DatabaseReference databaseRef;

    struct UserStruct
    {
        public byte[] rocketPartId;
        public string name;
        public int score;
        public UserStruct(byte[] _rocketPartId, string _name, int _score) {rocketPartId = _rocketPartId; name = _name; score = _score; }
    }

    private void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(
        task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError(message: $"failed to connect with {task.Exception}");
                return;
            }

            Debug.Log("connected");
            databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        });
    }

    public void SendToDatabase()
    {
        byte[] rocketParts = new byte[3];
        rocketParts[0] = 12;
        rocketParts[1] = 0;
        rocketParts[2] = 24;
        UserStruct newUser = new UserStruct(rocketParts, "Cjs", 12348);
        string toJson = JsonUtility.ToJson(newUser);
        Debug.Log(toJson);
        databaseRef.Child("Users").Child(newUser.name).SetRawJsonValueAsync(toJson).ContinueWith(
        task =>
        {
            Debug.Log("send...");
            if (task.IsCanceled) { Debug.LogError("send to database canceled : " + task.Exception); return; };
            if (task.IsFaulted) { Debug.LogError("send to database faild : " + task.Exception); return; };
            if (task.IsCompleted) { Debug.Log("database data send !"); };           
        });
    }

    public void ReadFromDatabase()
    {
        databaseRef.Child("Users").GetValueAsync().ContinueWith(
            task => 
            {
                Debug.Log("read...");
                if (task.IsCanceled) { Debug.LogError("read from database canceled : " + task.Exception); return; };
                if (task.IsFaulted) { Debug.LogError("read from database faild : " + task.Exception); return; };
                if (task.IsCompleted) 
                { 
                    DataSnapshot snapshop = task.Result;
                    Debug.Log("database data read : " + snapshop.ToString());
                };
            });
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

    public void CreateUserWithGplay()
    {

    }

    public void ConnectToGoogelPlay()
    {

    }

    public void ConnectToFireBaseViaGoogelPlay()
    {

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
