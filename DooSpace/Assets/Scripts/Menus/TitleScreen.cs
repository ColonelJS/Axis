using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private GameObject highscoreMenu;
    [SerializeField] private GameObject customMenu;

    bool isDraging = false;
    Vector2 startTouch;
    Vector2 swipeDelta;
    bool swipeLeft = false;
    bool swipeRight = false;

    bool isHighscoreOpen = false;
    bool isCustomOpen = false;

    float moveBackSpeed = 2000f;
    float swipeSpeed = 15f;

    bool isSwitchSound = true;

    void Start()
    {
        
    }

    void Update()
    {
        if(GameManager.instance.GetGameState() == GameManager.GameState.MENU && !SoundManager.instance.GetSliderOpen())
            UpdateMenuSwipe();
    }

    void UpdateMenuSwipe()
	{
        if (GetSwipe())
        {
            if (!isCustomOpen)
            {
                if (swipeLeft && swipeDelta.x < 0 && !isHighscoreOpen)
                    MoveMenuRelativeToSwipe("left");
            }
            else
            {
                if (swipeRight && swipeDelta.x > 0)
                    MoveMenuBackRelativeToSwipe("right");
            }

            if (!isHighscoreOpen)
            {
                if (swipeRight && swipeDelta.x > 0 && !isCustomOpen)
                    MoveMenuRelativeToSwipe("right");
            }
            else
            {
                if (swipeLeft && swipeDelta.x < 0)
                    MoveMenuBackRelativeToSwipe("left");
            }
        }
        else
		{
            if (!isCustomOpen)
                MoveBackMenu("right");
            else
                MoveBackMenuReverse("left");

            if (!isHighscoreOpen)
                MoveBackMenu("left");
            else
                MoveBackMenuReverse("right");
        }

        UpdateMusic();

    }

    void UpdateMusic()
	{
        if (isSwitchSound)
        {
            if (!isHighscoreOpen && !isCustomOpen)
            {
                SoundManager.instance.StopMusic();
                SoundManager.instance.PlayMusic("mainMenu");
            }
            else if (isHighscoreOpen)
            {
                SoundManager.instance.StopMusic();
                SoundManager.instance.PlayMusic("highscore");
            }
            else if (isCustomOpen)
            {
                SoundManager.instance.StopMusic();
                SoundManager.instance.PlayMusic("custom");
            }
            isSwitchSound = false;
        }
    }

    void MoveMenuRelativeToSwipe(string _direction)
	{
        //float swipe = swipeDelta.x * swipeSpeed;
        float swipe = Input.touches[0].deltaPosition.x * 75;
        if (_direction == "left")
		{
            if (customMenu.transform.localPosition.x > 0 && Input.touches.Length > 0)
                customMenu.transform.localPosition += new Vector3(swipe, 0, 0) * Time.deltaTime;//25 (Input.touches[0].deltaPosition.x / 50)
            else
            {
                customMenu.transform.localPosition = new Vector3(0, customMenu.transform.localPosition.y, customMenu.transform.localPosition.z);
                isCustomOpen = true;
                isSwitchSound = true;
            }
        }
        if (_direction == "right")
        {
            if (highscoreMenu.transform.localPosition.x < 0 && Input.touches.Length > 0)
                highscoreMenu.transform.localPosition += new Vector3(swipe, 0, 0) * Time.deltaTime;
            else
            {
                highscoreMenu.transform.localPosition = new Vector3(0, highscoreMenu.transform.localPosition.y, highscoreMenu.transform.localPosition.z);
                isHighscoreOpen = true;
                isSwitchSound = true;
            }
        }
    }

    void MoveMenuBackRelativeToSwipe(string _direction)
    {
        //float swipe = swipeDelta.x * swipeSpeed;
        float swipe = Input.touches[0].deltaPosition.x * 75;
        if (_direction == "right")
        {
            if (customMenu.transform.localPosition.x < Screen.width && Input.touches.Length > 0)
                customMenu.transform.localPosition += new Vector3(swipe, 0, 0) * Time.deltaTime;
            else
            {
                customMenu.transform.localPosition = new Vector3(Screen.width, customMenu.transform.localPosition.y, customMenu.transform.localPosition.z);
                isCustomOpen = false;
                isSwitchSound = true;
            }
        }
        if (_direction == "left")
        {
            if (highscoreMenu.transform.localPosition.x > -Screen.width && Input.touches.Length > 0)
                highscoreMenu.transform.localPosition += new Vector3(swipe, 0, 0) * Time.deltaTime;
            else
            {
                highscoreMenu.transform.localPosition = new Vector3(-Screen.width, highscoreMenu.transform.localPosition.y, highscoreMenu.transform.localPosition.z);
                isHighscoreOpen = false;
                isSwitchSound = true;
            }
        }
    }

    void MoveBackMenu(string _direction)
	{
        if (_direction == "left")
        {
            if (highscoreMenu.transform.localPosition.x > -Screen.width && highscoreMenu.transform.localPosition.x < -Screen.width / 2)
            {
                highscoreMenu.transform.localPosition -= new Vector3(moveBackSpeed, 0, 0) * Time.deltaTime;
            }
            else if (highscoreMenu.transform.localPosition.x > -Screen.width / 2 && highscoreMenu.transform.localPosition.x < 0)
            {
                highscoreMenu.transform.localPosition += new Vector3(moveBackSpeed, 0, 0) * Time.deltaTime;
            }
            else if (highscoreMenu.transform.localPosition.x < -Screen.width)
            {
                highscoreMenu.transform.localPosition = new Vector3(-Screen.width, highscoreMenu.transform.localPosition.y, highscoreMenu.transform.localPosition.z);
            }
            else if (highscoreMenu.transform.localPosition.x > 0)
            {
                highscoreMenu.transform.localPosition = new Vector3(0, highscoreMenu.transform.localPosition.y, highscoreMenu.transform.localPosition.z);
                isHighscoreOpen = true;
                isSwitchSound = true;
            }
        }

        if (_direction == "right")
        {
            if (customMenu.transform.localPosition.x < Screen.width && customMenu.transform.localPosition.x > Screen.width/2)
            {
                customMenu.transform.localPosition += new Vector3(moveBackSpeed, 0, 0) * Time.deltaTime;
            }
            else if(customMenu.transform.localPosition.x < Screen.width / 2 && customMenu.transform.localPosition.x > 0)
			{
                customMenu.transform.localPosition -= new Vector3(moveBackSpeed, 0, 0) * Time.deltaTime;
            }
            else if(customMenu.transform.localPosition.x > Screen.width)
            {
                customMenu.transform.localPosition = new Vector3(Screen.width, customMenu.transform.localPosition.y, customMenu.transform.localPosition.z);
            }
            else if(customMenu.transform.localPosition.x < 0)
			{
                customMenu.transform.localPosition = new Vector3(0, customMenu.transform.localPosition.y, customMenu.transform.localPosition.z);
                isCustomOpen = true;
                isSwitchSound = true;
            }
        }
    }

    void MoveBackMenuReverse(string _direction)
    {
        if (_direction == "right")
        {
            if (highscoreMenu.transform.localPosition.x < 0 && highscoreMenu.transform.localPosition.x > -Screen.width/2)
            {
                highscoreMenu.transform.localPosition += new Vector3(moveBackSpeed, 0, 0) * Time.deltaTime;
            }
            else if (highscoreMenu.transform.localPosition.x < -Screen.width/2 && highscoreMenu.transform.localPosition.x > -Screen.width)
            {
                highscoreMenu.transform.localPosition -= new Vector3(moveBackSpeed, 0, 0) * Time.deltaTime;
            }
            else if(highscoreMenu.transform.localPosition.x > 0)
            {
                highscoreMenu.transform.localPosition = new Vector3(0, highscoreMenu.transform.localPosition.y, highscoreMenu.transform.localPosition.z);
            }
            else if (highscoreMenu.transform.localPosition.x < -Screen.width)
            {
                highscoreMenu.transform.localPosition = new Vector3(-Screen.width, highscoreMenu.transform.localPosition.y, highscoreMenu.transform.localPosition.z);
                isHighscoreOpen = false;
                isSwitchSound = true;
            }
        }

        if (_direction == "left")
        {
            if (customMenu.transform.localPosition.x > 0 && customMenu.transform.localPosition.x < Screen.width/2)
            {
                customMenu.transform.localPosition -= new Vector3(moveBackSpeed, 0, 0) * Time.deltaTime;
            }
            else if (customMenu.transform.localPosition.x > Screen.width / 2 && customMenu.transform.localPosition.x < Screen.width)
            {
                customMenu.transform.localPosition += new Vector3(moveBackSpeed, 0, 0) * Time.deltaTime;
            }
            else if(customMenu.transform.localPosition.x < 0)
            {
                customMenu.transform.localPosition = new Vector3(0, customMenu.transform.localPosition.y, customMenu.transform.localPosition.z);
            }
            else if (customMenu.transform.localPosition.x > Screen.width)
            {
                customMenu.transform.localPosition = new Vector3(Screen.width, customMenu.transform.localPosition.y, customMenu.transform.localPosition.z);
                isCustomOpen = false;
                isSwitchSound = true;
            }
        }
    }

    bool GetSwipe()
	{
        if (Input.touches.Length > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                isDraging = true;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                isDraging = false;
                swipeLeft = false;
                swipeRight = false;
                return false;
            }

        }

        swipeDelta = Vector2.zero;
        if (isDraging)
        {
            if (Input.touches.Length > 0)
                swipeDelta = Input.touches[0].position - startTouch;
        }

        float x = swipeDelta.x;
        float y = swipeDelta.y;
        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            //Left or right
            if (x < 0)
                swipeLeft = true;
            else
                swipeRight = true;
            return true;
        }
        return false;
    }
}
