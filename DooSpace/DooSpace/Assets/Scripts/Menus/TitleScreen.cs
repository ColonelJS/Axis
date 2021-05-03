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
        if (animationStart)
            MoveBackgrounds();
    }

    public void StartGame()
	{
        animationStart = true;
	}

    void MoveBackgrounds()
	{


        animationStart = false;
        gameStart = true;
	}

    public bool GetIsGameStart()
	{
        return gameStart;
	}
}
