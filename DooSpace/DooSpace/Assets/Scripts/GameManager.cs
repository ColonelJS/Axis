using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    bool gameStart = false;

    float scrolingSpeed = 40f;
    float scrolingSpeedBase = 40f;
    float scrollingSpeedMax = 80f;
    float scrollingSpeedFactor = 50f;
    float loseAcceleration = 1f;
    float speedFactor = 1f;

    bool isMiniBoost = false;
    float miniBoostBase = 45f;
    float miniBoost = 45f;

    bool playerLose = false;

    public enum GameState
	{
        MENU,
        START,
        GAME,
        LOSE,
        SCORE
	} GameState gameState = new GameState();

	private void Awake()
	{
        if (instance == null)
            instance = this;
	}

	void Start()
    {
        gameState = GameState.MENU;
    }

    void Update()
    {
        UpdateScrollingSpeed();
        GetGameEnd();
    }

    public void SetGameStart()
	{
        gameStart = true;
        gameState = GameState.START;
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
        if (CharacterManager.instance.GetFuel() <= 100)
            scrolingSpeed = scrolingSpeedBase * (CharacterManager.instance.GetFuel() / scrollingSpeedFactor);
        else
            scrolingSpeed = scrollingSpeedMax;

        UpdateMiniBoost();
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
            scrolingSpeed = -0.01f;
        }

        if (playerLose)
        {
            scrolingSpeed -= 34 * loseAcceleration * Time.deltaTime;
            loseAcceleration += 3f * Time.deltaTime;
            print("scroling speed : " + scrolingSpeed);
        }
	}

    public bool GetPlayerlose()
    {
        return playerLose;
    }

    public void SetScoreScreen()
	{

	}
}
