using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScreen : MonoBehaviour
{
    public static TransitionScreen instance;

    [SerializeField] private GameObject transitionScreen;
    [SerializeField] private ScoreScreen scoreScreen;

    float transitionScreenSpeed = 1200f;
    bool isTransitionStart = false;
    bool screenReachMiddle = false;
    bool screenReachEnd = false;
    bool isSceneReloaded = false;

	private void Awake()
	{
        //if (instance == null)
            instance = this;
	}

	void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (isTransitionStart)
        {
            if (!screenReachMiddle)
                UpdateTransitionStart();
            else
			{
                if (!isSceneReloaded)
                    ReloadScene();
                else
                {
                    if (!screenReachEnd)
                        UpdateTransitionEnd();
                }
            }
        }
    }

    public void SetTransitionStart()
	{
        isTransitionStart = true;
	}

    void UpdateTransitionStart()
	{
        if (transitionScreen.transform.localPosition.x < 0)
        {
            transitionScreen.transform.localPosition += new Vector3(transitionScreenSpeed, 0, 0) * Time.deltaTime;
        }
        else
        {
            transitionScreen.transform.localPosition = Vector3.zero;
            screenReachMiddle = true;
        }
    }

    void UpdateTransitionEnd()
    {
        if (transitionScreen.transform.localPosition.x < Screen.width + Screen.width/2)
        {
            transitionScreen.transform.localPosition += new Vector3(transitionScreenSpeed, 0, 0) * Time.deltaTime;
            //scoreScreen.SetLocalPos(new Vector3(transitionScreenSpeed, 0, 0));
            //if(scoreScreen != null)
                //scoreScreen.UpdateAnimationReverse();
        }
        else
        {
            transitionScreen.transform.localPosition = new Vector3(Screen.width + Screen.width / 2, 0, 0);
            screenReachEnd = true;
            ResetTransitionScreen();
        }
    }

    void ReloadScene()
	{
        SceneManager.LoadScene("Game");
        isSceneReloaded = true;
    }

    void ResetTransitionScreen()
	{
        isTransitionStart = false;
        screenReachMiddle = false;
        screenReachEnd = false;
        isSceneReloaded = false;
        transitionScreen.transform.localPosition = new Vector3(-Screen.width, 0, 0);
        Destroy(this.gameObject);
    }
}
