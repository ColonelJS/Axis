using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject menuUp;
    [SerializeField] private GameObject buttonStart;
    [SerializeField] private GameObject ribbon;
    //[SerializeField] private GameObject menuDown;
    [SerializeField] private GameObject hud;
    [SerializeField] private TitleScreen titleScreen;
    [SerializeField] private ObstacleSpawner obstacleSpawner;

    bool gameStart = false;
    bool isStartAnimation = false;
    bool ribbonEndMoving = false;
    float menuAnimationSpeed = 1500;//750

    float scrolingSpeed = 40f;
    float scrolingSpeedBase = 40f;
    float scrollingSpeedMax = 85f;
    float scrollingSpeedFactor = 50f;
    float loseAcceleration = 1f;
    float speedFactor = 1f;
    float speedFactorMax = 1.8f;
    float startPosMenu;
    float startRibbonPos;

    bool isMiniBoost = false;
    float miniBoostBase = 48f;
    float miniBoost = 48f;

    bool playerLose = false;
    bool isDoubleCoin = false;
    bool isRevive = false;

    bool rocketSoundPlay = false;

    public enum GameState
	{
        MENU,
        START,
        GAME,
        LOSE,
        SCORE, 
        REVIVE, 
        ALIEN_WAVE
	} GameState gameState = new GameState();

	private void Awake()
	{
        if (instance == null)
            instance = this;
	}

	void Start()
    {
        gameState = GameState.MENU;
        startPosMenu = menuUp.transform.localPosition.y;
        startRibbonPos = ribbon.transform.localPosition.x;
    }

    void Update()
    {
        if (isStartAnimation)
        {
            if (ribbonEndMoving)
                UpdateStartAnimation();
            else
                UpdateRibbon();
        }

        UpdateScrollingSpeed();
        GetGameEnd();

        if (gameState == GameState.START || gameState == GameState.GAME || gameState == GameState.LOSE)
            hud.SetActive(true);
        else
            hud.SetActive(false);

        if (gameState == GameState.GAME || gameState == GameState.START)
        {
            if (!rocketSoundPlay)
            {
                SoundManager.instance.StopMusic();
                SoundManager.instance.PlayMusic("gameplay");
                SoundManager.instance.PlayRocket();
                rocketSoundPlay = true;
            }
        }
    }

    public void SetGameStart()
	{
        gameStart = true;
        gameState = GameState.START;
    }

    public void StartGameAnimation()
	{
        if(!titleScreen.GetIsMenusOpen())
            isStartAnimation = true;
	}

    void UpdateRibbon()
	{
        if (ribbon.transform.localPosition.x < startRibbonPos + Screen.width)
            ribbon.transform.localPosition += new Vector3(menuAnimationSpeed, 0, 0) * Time.deltaTime;
        else
        {
            ribbon.transform.localPosition = new Vector3(startRibbonPos + Screen.width, ribbon.transform.localPosition.y, ribbon.transform.localPosition.z);
            ribbonEndMoving = true;
        }
    }

    void UpdateStartAnimation()
	{
        bool upEnd = false;
        bool downEnd = false;

        if (menuUp.transform.localPosition.y < startPosMenu + Screen.height)
        {
            menuUp.transform.localPosition += new Vector3(0, menuAnimationSpeed, 0) * Time.deltaTime;
            buttonStart.transform.localPosition += new Vector3(0, menuAnimationSpeed, 0) * Time.deltaTime;
        }
        else
        {
            menuUp.transform.localPosition = new Vector3(menuUp.transform.localPosition.x, startPosMenu + Screen.height, menuUp.transform.localPosition.z);
            upEnd = true;
        }

        /*if (menuDown.transform.localPosition.y > -Screen.height/2)
            menuDown.transform.localPosition -= new Vector3(0, menuAnimationSpeed, 0) * Time.deltaTime;
        else
        {
            menuDown.transform.localPosition = new Vector3(menuDown.transform.localPosition.x, -Screen.height/2, menuDown.transform.localPosition.z);
            downEnd = true;
        }*/

        if(upEnd /*&& downEnd*/)
		{
            isStartAnimation = false;
            SetGameStart();
        }
    }

    public bool GetIsStartAnimation()
	{
        return isStartAnimation;
	}

    public bool GetIsGameStart()
    {
        return gameStart;
    }

    public GameState GetGameState()
	{
        return gameState;
	}

    public void SetGameState(GameState _state)
	{
        gameState = _state;
	}

    public float GetScrolingSpeed()
	{
        return -scrolingSpeed;
	}

    void UpdateScrollingSpeed()
    {
        float motorSpeedFactor = 1 + 0.25f * CustomScreen.instance.GetFuelLevel();

        if (CharacterManager.instance.GetFuel() <= 100)
            scrolingSpeed = scrolingSpeedBase * (CharacterManager.instance.GetFuel() / scrollingSpeedFactor) * speedFactor * motorSpeedFactor;
        else
        {
            scrolingSpeed = scrollingSpeedMax * speedFactor * motorSpeedFactor;
        }

        if (gameState == GameState.GAME)
            UpdateSpeedFactor();

        UpdateMiniBoost();
    }

    void UpdateSpeedFactor()
	{
        if (speedFactor < speedFactorMax)
            speedFactor += 0.0038f * Time.deltaTime; //0.0035
        else
            speedFactor = speedFactorMax;

        //print("speed factor : " + speedFactor);
	}

    public float GetSpeedFactor()
	{
        return speedFactor;
	}

    void UpdateMiniBoost()
	{
        if(isMiniBoost)
		{
            if (miniBoost > 0)
            {
                scrolingSpeed = scrolingSpeed + miniBoost;
                miniBoost -= Time.deltaTime * 15f;
            }
            else
                isMiniBoost = false;
		}
	}

    public void SetIsMiniBoost()
	{
        miniBoost = miniBoostBase;
        isMiniBoost = true;
	}

    void GetGameEnd()
	{
        if (scrolingSpeed <= 0 && !playerLose)
        {
            playerLose = true;
            SetGameState(GameState.LOSE);
            SoundManager.instance.StopMusic();
            //SoundManager.instance.PauseMusic();
            SoundManager.instance.StopRocket();
            SoundManager.instance.PlaySound("fall");
            scrolingSpeed = -0.01f;
        }

        if (playerLose)
        {
            scrolingSpeed -= 34 * loseAcceleration * Time.deltaTime;
            loseAcceleration += 3f * Time.deltaTime;
            //print("scroling speed : " + scrolingSpeed);
        }
	}

    public void ResetGameEnd()
	{
        scrolingSpeed = 1;
        loseAcceleration = 1f;
        playerLose = false;
        rocketSoundPlay = false;
    }

    public void SetDoubleCoinReward()
	{
        isDoubleCoin = true;
	}

    public bool GetDoubleCoinReward()
	{
        return isDoubleCoin;
	}

    public void SetReviveReward(bool _value)
    {
        isRevive = _value;
    }

    public bool GetReviveReward()
    {
        return isRevive;
    }

    public bool GetPlayerlose()
    {
        return playerLose;
    }

    public void DeleteAllMeteorite()
	{
        obstacleSpawner.SetIsDestroyMeteorite();
    }
}
