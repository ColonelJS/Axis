using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private GameObject highscoreMenu;
    [SerializeField] private GameObject customMenu;
    [SerializeField] private GameObject settings;
    [SerializeField] private SettingsManager settingsManager;
    [SerializeField] private Transform gearTransformPoint;
    [SerializeField] private GameObject gear;

    bool isDraging = false;
    Vector2 startTouch;
    Vector2 swipeDelta;
    bool swipeLeft = false;
    bool swipeRight = false;
    bool swipeUp = false;
    bool swipeDown = false;

    bool isHighscoreOpen = false;
    bool isCustomOpen = false;
    bool isSettingsOpen = false;

    float moveBackSpeed = 2000f;
    float swipeSpeed = 15f;

    float startSettingPosY;

    bool isSwitchSound = true;

    void Start()
    {
        startSettingPosY = 0;    //-(2400 - Screen.height);
        settings.transform.position = new Vector3(settings.transform.position.x, startSettingPosY + 200, settings.transform.position.z);
        settings.transform.localPosition = new Vector3(settings.transform.localPosition.x, startSettingPosY + 200, settings.transform.localPosition.z);
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
                if (swipeLeft && swipeDelta.x < 0 && !isHighscoreOpen && !isSettingsOpen)
                    MoveMenuRelativeToSwipe("left");
            }
            else
            {
                if (swipeRight && swipeDelta.x > 0)
                    MoveMenuBackRelativeToSwipe("right");
            }

            if (!isHighscoreOpen)
            {
                if (swipeRight && swipeDelta.x > 0 && !isCustomOpen && !isSettingsOpen)
                    MoveMenuRelativeToSwipe("right");
            }
            else
            {
                if (swipeLeft && swipeDelta.x < 0)
                    MoveMenuBackRelativeToSwipe("left");
            }

            if(!isSettingsOpen)
			{
                if (!isHighscoreOpen && !isCustomOpen)
                {
                    if (swipeDown && swipeDelta.y < 0)
                        MoveMenuRelativeToSwipe("down");
                }
            }
            else
			{
                if (!settingsManager.GetIsInfoOpen() && !settingsManager.GetIsCreditsOpen())
                {
                    Debug.Log("info && credits open");
                    if (swipeUp && swipeDelta.y > 0)
                        MoveMenuBackRelativeToSwipe("up");
                }
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

            if (!isSettingsOpen)
            {
                MoveBackMenu("up");
            }
            else
            {
                MoveBackMenuReverse("down");
            }
        }

        UpdateMusic();
        SetGearFollowSettings();
    }

    void SetGearFollowSettings()
	{
        gear.transform.position = gearTransformPoint.position;
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
        float swipeY = Input.touches[0].deltaPosition.y * 75;
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

        if (_direction == "down")
        {
            if (settings.transform.localPosition.y > startSettingPosY && Input.touches.Length > 0)
                settings.transform.localPosition += new Vector3(0, swipeY, 0) * Time.deltaTime;//25 (Input.touches[0].deltaPosition.x / 50)
            else
            {
                settings.transform.localPosition = new Vector3(settings.transform.localPosition.x, startSettingPosY, settings.transform.localPosition.z);
                isSettingsOpen = true;
                //isSwitchSound = true;
            }
        }
    }

    void MoveMenuBackRelativeToSwipe(string _direction)
    {
        //float swipe = swipeDelta.x * swipeSpeed;
        float swipe = Input.touches[0].deltaPosition.x * 75;
        float swipeY = Input.touches[0].deltaPosition.y * 75;
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

        if (_direction == "up")
        {
            if (settings.transform.localPosition.y < startSettingPosY + 200 && Input.touches.Length > 0)
                settings.transform.localPosition += new Vector3(0, swipeY, 0) * Time.deltaTime;
            else
            {
                settings.transform.localPosition = new Vector3(settings.transform.localPosition.x, startSettingPosY + 200, settings.transform.localPosition.z);
                isSettingsOpen = false;
                //isSwitchSound = true;
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

        if (_direction == "up")
        {
            if (settings.transform.localPosition.y < startSettingPosY + 200 && settings.transform.localPosition.y > startSettingPosY + (200 / 2))
            {
                settings.transform.localPosition += new Vector3(0, moveBackSpeed/2, 0) * Time.deltaTime;
            }
            else if (settings.transform.localPosition.y < startSettingPosY + 200 / 2 && settings.transform.localPosition.y > 0)
            {
                settings.transform.localPosition -= new Vector3(0, moveBackSpeed/2, 0) * Time.deltaTime;
            }
            else if (settings.transform.localPosition.y > startSettingPosY + 200)
            {
                settings.transform.localPosition = new Vector3(settings.transform.localPosition.x, startSettingPosY + 200, settings.transform.localPosition.z);
            }
            else if (settings.transform.localPosition.y < startSettingPosY + 0)
            {
                settings.transform.localPosition = new Vector3(settings.transform.localPosition.x, startSettingPosY + 0, settings.transform.localPosition.z);
                isSettingsOpen = true;
                //isSwitchSound = true;
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

        if (_direction == "down")
        {
            Debug.Log("move back reverse to down");
            if (settings.transform.localPosition.y > startSettingPosY + 0 && settings.transform.localPosition.y < startSettingPosY + (200 / 2))
            {
                settings.transform.localPosition -= new Vector3(0, moveBackSpeed/2, 0) * Time.deltaTime;
            }
            else if (settings.transform.localPosition.y > startSettingPosY + (200 / 2) && settings.transform.localPosition.y < startSettingPosY + 200)
            {
                settings.transform.localPosition += new Vector3(0, moveBackSpeed/2, 0) * Time.deltaTime;
            }
            else if (settings.transform.localPosition.y < startSettingPosY + 0)
            {
                settings.transform.localPosition = new Vector3(settings.transform.localPosition.x, startSettingPosY, settings.transform.localPosition.z);
            }
            else if (settings.transform.localPosition.y > startSettingPosY + 200)
            {
                settings.transform.localPosition = new Vector3(settings.transform.localPosition.x, startSettingPosY + 200, settings.transform.localPosition.z);
                isSettingsOpen = false;
                //isSwitchSound = true;
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
                swipeUp = false;
                swipeDown = false;
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
        else if(Mathf.Abs(y) > Mathf.Abs(x))
		{
            if (y < 0)
                swipeDown = true;
            else
                swipeUp = true;
            return true;
		}
        return false;
    }

    public bool GetIsMenusOpen()
	{
        if (isHighscoreOpen)
            return true;
        else if (isCustomOpen)
            return true;
        else
            return false;
    }
}
