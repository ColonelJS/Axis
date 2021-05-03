using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameElements : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        //if (GameManager.instance.GetGameState() == GameManager.GameState.GAME)
            //MoveElement();
    }

    public virtual void MoveElement()
	{
        print("move base");
	}
}
