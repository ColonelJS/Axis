using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
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

    //FirebaseApp app;
    FirebaseAuth auth; 

    
    private void Awake()
    {
        // auth = FirebaseAuth.GetAuth(app);
       auth = FirebaseAuth.DefaultInstance;
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
