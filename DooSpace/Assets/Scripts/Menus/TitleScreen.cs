using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private GameObject bgUp;
    [SerializeField] private GameObject bgDown;

    bool animationStart = false;
    bool gameStart = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (GameManager.instance.GetGameState() == GameManager.GameState.START)
            MoveBackgrounds();
    }

    public void StartGame()
	{
        animationStart = true;
	}

    void MoveBackgrounds()
	{
        bgUp.SetActive(false);
        bgDown.SetActive(false);
        animationStart = false;
        GameManager.instance.SetGameStart();
	}
}
