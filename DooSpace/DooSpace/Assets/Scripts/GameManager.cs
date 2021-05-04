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

    bool isMiniBoost = false;
    float miniBoostBase = 40f;
    float miniBoost = 40f;

    public enum GameState
	{
        MENU,
        START,
        GAME,
        END
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
        print("scrolling speed : " + scrolingSpeed);
        print("mini boost : " + miniBoost);
        if(isMiniBoost)
		{
            if (miniBoost > 0)
            {
                scrolingSpeed = scrolingSpeed + miniBoost;
                miniBoost -= Time.deltaTime * 15f;
            }
            else
            {
                //miniBoost = miniBoostBase;
                isMiniBoost = false;
            }
		}
	}

    public void SetIsMiniBoost()
	{
        miniBoost = miniBoostBase;
        isMiniBoost = true;
	}
}
