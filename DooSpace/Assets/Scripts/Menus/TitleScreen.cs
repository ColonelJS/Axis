using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private GameObject highscoreMenu;
    [SerializeField] private GameObject customMenu;
    [SerializeField] private GameObject settings;
    [SerializeField] private SettingsManager settingsManager;
    [SerializeField] private Transform gearTransformPoint;
    [SerializeField] private GameObject gear;
    [SerializeField] private Image imgHighscore;
    [SerializeField] private Image imgCustom;
    [SerializeField] private Image imgGear;
    [SerializeField] private Image imgHighscoreArrows;
    [SerializeField] private Image imgCustomArrows;
    [SerializeField] private Image imgGearArrows;
    [SerializeField] private Image imgGearArrows2;
    [SerializeField] private Text startText;
    [SerializeField] private Text titleText;
    [SerializeField] private Image imgButtonStart;
    [SerializeField] private Image imgRibbon;
    [SerializeField] private Image imgNotif;
    //[SerializeField] private Image imgChristmasHat;
    [SerializeField] private GameObject gearButton;
    [SerializeField] private GameObject settingsStripes;
    [SerializeField] private GameObject settingsStripesBack;

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

    bool isHighscoreTrueClose = true;
    bool isCustomTrueClose = true;
    bool isSettingsTrueOpen = false;

    public enum ForceSwipe
	{
        NONE,
        FORWARD,
        BACK
	}

    ForceSwipe forceSwipeHighscore = ForceSwipe.NONE;
    ForceSwipe forceSwipeCustom = ForceSwipe.NONE;
    ForceSwipe forceSwipeSettings = ForceSwipe.NONE;

    float moveBackSpeed = 2000f;
    float forceSwipeSpeed = 1600f;
    float forceSwipeSettingsSpeed = 800f;

    bool isSwitchSound = true;

    void Start()
    {
        settings.transform.position = new Vector3(settings.transform.position.x, ResolutionManager.instance.GetSettingsSizeY(), settings.transform.position.z);
        settings.transform.localPosition = new Vector3(settings.transform.localPosition.x, ResolutionManager.instance.GetSettingsSizeY(), settings.transform.localPosition.z);

        customMenu.transform.localPosition = new Vector3(Screen.width, 0, 0);
    }

    void Update()
    {
        if (GameManager.instance.GetGameState() == GameManager.GameState.MENU && !GameManager.instance.GetIsStartAnimation())
        {
            if(!FireBaseAuthScript.instance.GetIsCurrentlyConnectToDB())
                UpdateMenuSwipe();

            UpdateMenusTrueClose();
            UpdateLogoScreenOpacity();

            if (isHighscoreOpen || isCustomOpen)
            {
                if (isSettingsOpen)
                    forceSwipeSettings = ForceSwipe.BACK;
            }

            UpdateSettingsStipes();
        }
    }

    void UpdateSettingsStipes()
	{
        if (isSettingsOpen)
        {
            if (settingsStripes.activeSelf)
            {
                settingsStripesBack.SetActive(true);
                settingsStripes.SetActive(false);
            }
        }
        else
		{
            if (settingsStripesBack.activeSelf)
            {
                settingsStripesBack.SetActive(false);
                settingsStripes.SetActive(true);
            }
        }
    }

    void UpdateMenuSwipe()
	{
        if (GetSwipe())
        {
            if (!isCustomOpen)
            {
                if (swipeLeft && swipeDelta.x < 0 && isHighscoreTrueClose && !settingsManager.GetIsExtrasOpen())
                    MoveMenuRelativeToSwipe("left");
            }
            else
            {
                if (swipeRight && swipeDelta.x > 0)
                    MoveMenuBackRelativeToSwipe("right");
            }

            if (!isHighscoreOpen)
            {
                if (swipeRight && swipeDelta.x > 0 && isCustomTrueClose && !settingsManager.GetIsExtrasOpen())
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
                if (!settingsManager.GetIsInfoOpen() && !settingsManager.GetIsCreditsOpen() && !settingsManager.GetIsPlayerInfoOpen())
                {
                    if (swipeUp && swipeDelta.y > 0)
                        MoveMenuBackRelativeToSwipe("up");
                }
            }
        }
        else
		{
            if (forceSwipeHighscore == ForceSwipe.NONE && forceSwipeCustom == ForceSwipe.NONE)
            {
                if (!isCustomOpen && isHighscoreTrueClose)
                    MoveBackMenu("right");
                else
                    MoveBackMenuReverse("left");

                if (!isHighscoreOpen && isCustomTrueClose)
                    MoveBackMenu("left");
                else
                    MoveBackMenuReverse("right");
            }

            if (forceSwipeSettings == ForceSwipe.NONE)
            {
                if (!isSettingsOpen)
                {
                    MoveBackMenu("up");
                }
                else
                {
                    MoveBackMenuReverse("down");
                }
            }
        }

        UpdateMusic();
        SetGearFollowSettings();
        UpdateForceSwipe();
    }

    void UpdateMenusTrueClose()
	{
        if (customMenu.transform.localPosition.x >= Screen.width)
            isCustomTrueClose = true;
        else
            isCustomTrueClose = false;

        if (highscoreMenu.transform.localPosition.x <= 0)
            isHighscoreTrueClose = true;
        else
            isHighscoreTrueClose = false;

        if (settings.transform.localPosition.y <= 0)
            isSettingsTrueOpen = true;
        else
            isSettingsTrueOpen = false;
    }

    public bool GetIsSettingsTrueOpen()
	{
        return isSettingsTrueOpen;
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
                //SoundManager.instance.StopMusic();
                //SoundManager.instance.PlayMusic("highscore");
            }
            else if (isCustomOpen)
            {
                SoundManager.instance.StopMusic();
                SoundManager.instance.PlayMusic("custom");
            }
            isSwitchSound = false;
        }
    }

    void UpdateLogoScreenOpacity()
	{
        if (isHighscoreTrueClose)
        {
            float customProgress = 1 - (Mathf.Abs(customMenu.transform.localPosition.x - Screen.width) / Screen.width);
            Color newColor = new Color(imgCustom.color.r, imgCustom.color.g, imgCustom.color.b, imgCustom.color.a);
            newColor.a = customProgress;
            imgCustom.color = newColor;
            imgCustomArrows.color = newColor;
            imgHighscore.color = newColor;
            imgHighscoreArrows.color = newColor;
            imgGear.color = newColor;
            imgGearArrows.color = newColor;
            imgGearArrows2.color = newColor;
            imgRibbon.color = new Color(imgRibbon.color.r, imgRibbon.color.g, imgRibbon.color.b, newColor.a / 2);
            imgNotif.color = newColor;
            imgButtonStart.color = newColor;
            //imgChristmasHat.color = newColor;
            titleText.color = new Color(titleText.color.r, titleText.color.g, titleText.color.b, newColor.a);
            startText.color = new Color(startText.color.r, startText.color.g, startText.color.b, newColor.a);
        }

        if (isCustomTrueClose)
        {
            float highscoreProgress = highscoreMenu.transform.localPosition.x * 1 / Screen.width;
            Color newHColor = new Color(imgCustom.color.r, imgCustom.color.g, imgCustom.color.b, imgCustom.color.a);
            newHColor.a = 1 - highscoreProgress;
            imgHighscore.color = newHColor;
            imgHighscoreArrows.color = newHColor;
            imgCustom.color = newHColor;
            imgCustomArrows.color = newHColor;
            imgGear.color = newHColor;
            imgGearArrows.color = newHColor;
            imgGearArrows2.color = newHColor;
            imgRibbon.color = new Color(imgRibbon.color.r, imgRibbon.color.g, imgRibbon.color.b, newHColor.a/2);
            imgNotif.color = newHColor;
            imgButtonStart.color = newHColor;
            //imgChristmasHat.color = newHColor;
            titleText.color = new Color(titleText.color.r, titleText.color.g, titleText.color.b, newHColor.a);
            startText.color = new Color(startText.color.r, startText.color.g, startText.color.b, newHColor.a);
        }
    }

    void MoveSettingsRelativeToOther(float _swipe)
	{
        settings.transform.localPosition += new Vector3(0, Mathf.Abs(_swipe) / 1.33f, 0) * Time.deltaTime;
        if (settings.transform.localPosition.y >= ResolutionManager.instance.GetSettingsSizeY())
        {
            settings.transform.localPosition = new Vector3(settings.transform.localPosition.x, ResolutionManager.instance.GetSettingsSizeY(), settings.transform.localPosition.z);
            isSettingsOpen = false;
        }
    }

    void MoveMenuRelativeToSwipe(string _direction)
	{
        float swipe = Input.touches[0].deltaPosition.x * 75;
        float swipeY = Input.touches[0].deltaPosition.y * 75;
        if (_direction == "left")
		{
            if (customMenu.transform.localPosition.x > 0 && Input.touches.Length > 0)
            {
                customMenu.transform.localPosition += new Vector3(swipe, 0, 0) * Time.deltaTime;
                if (isSettingsOpen)
                    MoveSettingsRelativeToOther(swipe);
            }
            else
            {
                customMenu.transform.localPosition = new Vector3(0, customMenu.transform.localPosition.y, customMenu.transform.localPosition.z);
                isCustomOpen = true;
                isSwitchSound = true;
            }
        }
        if (_direction == "right")
        {
            if (highscoreMenu.transform.localPosition.x < Screen.width && Input.touches.Length > 0)
            {
                highscoreMenu.transform.localPosition += new Vector3(swipe, 0, 0) * Time.deltaTime;
                if(isSettingsOpen)
                    MoveSettingsRelativeToOther(swipe);
            }
            else
            {
                highscoreMenu.transform.localPosition = new Vector3(Screen.width, highscoreMenu.transform.localPosition.y, highscoreMenu.transform.localPosition.z);
                isHighscoreOpen = true;
                //isSwitchSound = true;
            }
        }

        if (_direction == "down")
        {
            if (settings.transform.localPosition.y > 0 && Input.touches.Length > 0)
                settings.transform.localPosition += new Vector3(0, swipeY, 0) * Time.deltaTime;
            else
            {
                settings.transform.localPosition = new Vector3(settings.transform.localPosition.x, 0, settings.transform.localPosition.z);
                isSettingsOpen = true;
            }
        }
    }

    void MoveMenuBackRelativeToSwipe(string _direction)
    {
        float swipe = Input.touches[0].deltaPosition.x * 75;
        float swipeY = Input.touches[0].deltaPosition.y * 75;
        if (_direction == "right")
        {
            if (customMenu.transform.localPosition.x < Screen.width && Input.touches.Length > 0)
            {
                customMenu.transform.localPosition += new Vector3(swipe, 0, 0) * Time.deltaTime;
            }
            else
            {
                customMenu.transform.localPosition = new Vector3(Screen.width, customMenu.transform.localPosition.y, customMenu.transform.localPosition.z);
                isCustomOpen = false;
                isSwitchSound = true;
            }
        }
        if (_direction == "left")
        {
            if (highscoreMenu.transform.localPosition.x > 0 && Input.touches.Length > 0)
            {
                highscoreMenu.transform.localPosition += new Vector3(swipe, 0, 0) * Time.deltaTime;
            }
            else
            {
                highscoreMenu.transform.localPosition = new Vector3(0, highscoreMenu.transform.localPosition.y, highscoreMenu.transform.localPosition.z);
                isHighscoreOpen = false;
                //isSwitchSound = true;
            }
        }

        if (_direction == "up")
        {
            if (settings.transform.localPosition.y < ResolutionManager.instance.GetSettingsSizeY() && Input.touches.Length > 0)
                settings.transform.localPosition += new Vector3(0, swipeY, 0) * Time.deltaTime;
            else
            {
                settings.transform.localPosition = new Vector3(settings.transform.localPosition.x, ResolutionManager.instance.GetSettingsSizeY(), settings.transform.localPosition.z);
                isSettingsOpen = false;
            }
        }
    }

    void MoveBackMenu(string _direction)
	{
        if (_direction == "left")
        {
            if (highscoreMenu.transform.localPosition.x > 0 && highscoreMenu.transform.localPosition.x < Screen.width / 2)
            {
                highscoreMenu.transform.localPosition -= new Vector3(moveBackSpeed, 0, 0) * Time.deltaTime;
            }
            else if (highscoreMenu.transform.localPosition.x > Screen.width / 2 && highscoreMenu.transform.localPosition.x < Screen.width)
            {
                highscoreMenu.transform.localPosition += new Vector3(moveBackSpeed, 0, 0) * Time.deltaTime;
            }
            else if (highscoreMenu.transform.localPosition.x < 0)
            {
                highscoreMenu.transform.localPosition = new Vector3(0, highscoreMenu.transform.localPosition.y, highscoreMenu.transform.localPosition.z);
            }
            else if (highscoreMenu.transform.localPosition.x > Screen.width)
            {
                highscoreMenu.transform.localPosition = new Vector3(Screen.width, highscoreMenu.transform.localPosition.y, highscoreMenu.transform.localPosition.z);
                isHighscoreOpen = true;
                //isSwitchSound = true;
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
            if (settings.transform.localPosition.y < ResolutionManager.instance.GetSettingsSizeY() && settings.transform.localPosition.y > ResolutionManager.instance.GetSettingsSizeY()/2)
            {
                settings.transform.localPosition += new Vector3(0, moveBackSpeed / 2, 0) * Time.deltaTime;
            }
            else if (settings.transform.localPosition.y < ResolutionManager.instance.GetSettingsSizeY()/2 && settings.transform.localPosition.y > 0)
            {
                settings.transform.localPosition -= new Vector3(0, moveBackSpeed / 2, 0) * Time.deltaTime;
            }
            else if (settings.transform.localPosition.y > ResolutionManager.instance.GetSettingsSizeY())
            {
                settings.transform.localPosition = new Vector3(settings.transform.localPosition.x, ResolutionManager.instance.GetSettingsSizeY(), settings.transform.localPosition.z);
            }
            else if (settings.transform.localPosition.y < 0)
            {
                settings.transform.localPosition = new Vector3(settings.transform.localPosition.x, 0, settings.transform.localPosition.z);
                isSettingsOpen = true;
            }
        }
    }

    void MoveBackMenuReverse(string _direction)
    {
        if (_direction == "right")
        {
            if (highscoreMenu.transform.localPosition.x < Screen.width && highscoreMenu.transform.localPosition.x > Screen.width / 2)
            {
                highscoreMenu.transform.localPosition += new Vector3(moveBackSpeed, 0, 0) * Time.deltaTime;
            }
            else if (highscoreMenu.transform.localPosition.x < Screen.width / 2 && highscoreMenu.transform.localPosition.x > 0)
            {
                highscoreMenu.transform.localPosition -= new Vector3(moveBackSpeed, 0, 0) * Time.deltaTime;
            }
            else if (highscoreMenu.transform.localPosition.x > Screen.width)
            {
                highscoreMenu.transform.localPosition = new Vector3(Screen.width, highscoreMenu.transform.localPosition.y, highscoreMenu.transform.localPosition.z);
            }
            else if (highscoreMenu.transform.localPosition.x < 0)
            {
                highscoreMenu.transform.localPosition = new Vector3(0, highscoreMenu.transform.localPosition.y, highscoreMenu.transform.localPosition.z);
                isHighscoreOpen = false;
                //isSwitchSound = true;
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
            if (settings.transform.localPosition.y > 0 && settings.transform.localPosition.y < ResolutionManager.instance.GetSettingsSizeY()/2)
            {
                settings.transform.localPosition -= new Vector3(0, moveBackSpeed / 2, 0) * Time.deltaTime;
            }
            else if (settings.transform.localPosition.y > ResolutionManager.instance.GetSettingsSizeY()/2 && settings.transform.localPosition.y < ResolutionManager.instance.GetSettingsSizeY())
            {
                settings.transform.localPosition += new Vector3(0, moveBackSpeed / 2, 0) * Time.deltaTime;
            }
            else if (settings.transform.localPosition.y < 0)
            {
                settings.transform.localPosition = new Vector3(settings.transform.localPosition.x, 0, settings.transform.localPosition.z);
            }
            else if (settings.transform.localPosition.y > ResolutionManager.instance.GetSettingsSizeY())
            {
                settings.transform.localPosition = new Vector3(settings.transform.localPosition.x, ResolutionManager.instance.GetSettingsSizeY(), settings.transform.localPosition.z);
                isSettingsOpen = false;
            }
        }
    }

    public void ForceSwipeMenu(string _menuToSwipe)
	{
        if (_menuToSwipe == "highscore")
        {
            if(isCustomTrueClose)
                forceSwipeHighscore = ForceSwipe.FORWARD;
        }
        else if (_menuToSwipe == "custom")
        {
            if (isHighscoreTrueClose)
                forceSwipeCustom = ForceSwipe.FORWARD;
        }

        if (isSettingsOpen)
            forceSwipeSettings = ForceSwipe.BACK;
    }

    public void SwitchForceSwipeSettings()
	{
        if (isHighscoreTrueClose && isCustomTrueClose)
        {
            if (isSettingsOpen)
                forceSwipeSettings = ForceSwipe.BACK;
            else
                forceSwipeSettings = ForceSwipe.FORWARD;
        }
    }

    public void ForceSwipeMenuBack(string _menuToSwipe)
    {
        if (_menuToSwipe == "highscore")
        {
            forceSwipeHighscore = ForceSwipe.BACK;
        }
        else if (_menuToSwipe == "custom")
        {
            forceSwipeCustom = ForceSwipe.BACK;
        }
    }

    void UpdateForceSwipe()
	{
        if(forceSwipeHighscore == ForceSwipe.FORWARD)
		{
            if (highscoreMenu.transform.localPosition.x < Screen.width)
            {
                highscoreMenu.transform.localPosition += new Vector3(forceSwipeSpeed, 0, 0) * Time.deltaTime;
            }
            else
            {
                highscoreMenu.transform.localPosition = new Vector3(Screen.width, highscoreMenu.transform.localPosition.y, highscoreMenu.transform.localPosition.z);
                isHighscoreOpen = true;
                //isSwitchSound = true;
                forceSwipeHighscore = ForceSwipe.NONE;
            }
        }
        else if (forceSwipeHighscore == ForceSwipe.BACK)
        {
            if (highscoreMenu.transform.localPosition.x > 0)
            {
                highscoreMenu.transform.localPosition -= new Vector3(forceSwipeSpeed, 0, 0) * Time.deltaTime;
            }
            else
            {
                highscoreMenu.transform.localPosition = new Vector3(0, highscoreMenu.transform.localPosition.y, highscoreMenu.transform.localPosition.z);
                isHighscoreOpen = false;
                //isSwitchSound = true;
                forceSwipeHighscore = ForceSwipe.NONE;
            }
        }

        if (forceSwipeCustom == ForceSwipe.FORWARD)
        {
            if (customMenu.transform.localPosition.x > 0)
            {
                customMenu.transform.localPosition -= new Vector3(forceSwipeSpeed, 0, 0) * Time.deltaTime;
            }
            else
            {
                customMenu.transform.localPosition = new Vector3(0, customMenu.transform.localPosition.y, customMenu.transform.localPosition.z);
                isCustomOpen = true;
                isSwitchSound = true;
                forceSwipeCustom = ForceSwipe.NONE;
            }
        }
        else if (forceSwipeCustom == ForceSwipe.BACK)
        {
            if (customMenu.transform.localPosition.x < Screen.width)
            {
                customMenu.transform.localPosition += new Vector3(forceSwipeSpeed, 0, 0) * Time.deltaTime;
            }
            else
            {
                customMenu.transform.localPosition = new Vector3(Screen.width, customMenu.transform.localPosition.y, customMenu.transform.localPosition.z);
                isCustomOpen = false;
                isSwitchSound = true;
                forceSwipeCustom = ForceSwipe.NONE;
            }
        }

        if (forceSwipeSettings == ForceSwipe.FORWARD)
        {
            if (settings.transform.localPosition.y > 0)
                settings.transform.localPosition -= new Vector3(0, forceSwipeSettingsSpeed, 0) * Time.deltaTime;
            else
            {
                settings.transform.localPosition = new Vector3(settings.transform.localPosition.x, 0, settings.transform.localPosition.z);
                isSettingsOpen = true;
                forceSwipeSettings = ForceSwipe.NONE;
                settingsStripesBack.SetActive(true);
                settingsStripes.SetActive(false);
            }
        }
        else if (forceSwipeSettings == ForceSwipe.BACK)
        {
            if (settings.transform.localPosition.y < ResolutionManager.instance.GetSettingsSizeY())
                settings.transform.localPosition += new Vector3(0, forceSwipeSettingsSpeed, 0) * Time.deltaTime;
            else
            {
                settings.transform.localPosition = new Vector3(settings.transform.localPosition.x, ResolutionManager.instance.GetSettingsSizeY(), settings.transform.localPosition.z);
                isSettingsOpen = false;
                forceSwipeSettings = ForceSwipe.NONE;
                settingsStripesBack.SetActive(false);
                settingsStripes.SetActive(true);
            }
        }

        if(isCustomOpen)
            gearButton.SetActive(false);
        else
            gearButton.SetActive(true);
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
        if (!isHighscoreTrueClose)
            return true;
        else if (!isCustomTrueClose)
            return true;
        else
            return false;
    }
}
