using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScreen : MonoBehaviour
{
    [SerializeField] private GameObject transitionScreen;

    float transitionScreenSpeed = 1200f;
    bool isTransitionStart = false;
    bool screenReachMiddle = false;
    bool screenReachEnd = false;
    bool isSceneReloaded = false;

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
        if (transitionScreen.transform.localPosition.x < Screen.width)
        {
            transitionScreen.transform.localPosition += new Vector3(transitionScreenSpeed, 0, 0) * Time.deltaTime;
        }
        else
        {
            transitionScreen.transform.localPosition = new Vector3(Screen.width, 0, 0);
            screenReachEnd = true;
        }
    }

    void ReloadScene()
	{
        SceneManager.LoadScene("Game");
        isSceneReloaded = true;
    }
}
