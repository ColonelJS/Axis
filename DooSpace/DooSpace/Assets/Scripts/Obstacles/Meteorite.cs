using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite : GameElements
{

    void Start()
    {
        
    }

    void Update()
    {
        if (GameManager.instance.GetGameState() == GameManager.GameState.GAME)
            MoveElement();
    }

    public override void MoveElement()
	{
        gameObject.transform.position += new Vector3(0, GameManager.instance.GetScrolingSpeed(), 0) * Time.deltaTime;
	}
}
