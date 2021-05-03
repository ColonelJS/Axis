using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    bool gameStart = false;
    float scrolingSpeed = 25f;
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
}
