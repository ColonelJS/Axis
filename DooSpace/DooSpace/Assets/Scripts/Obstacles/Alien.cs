using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    float throwSpeed = 180f;
    bool isThrow = false;
    float randX;
    float randZ;

    void Start()
    {
        
    }

    void Update()
    {
        if (isThrow)
            UpdateThrow();
        else
            if (GameManager.instance.GetGameState() != GameManager.GameState.LOSE)
                MoveElement();
    }

    public void MoveElement()
    {
        gameObject.transform.position += new Vector3(0, GameManager.instance.GetScrolingSpeed(), 0) * Time.deltaTime;
    }

    public void throwAlien()
	{
        isThrow = true;

        randZ = Random.Range(-90, 90);
        randX = Random.Range(-180, 180);
    }

    void UpdateThrow()
	{
        /*float randZ = 0;
        randZ = Random.Range(-90, 90);

        float randX = Random.Range(-180, 180);*/

        /*if (CharacterManager.instance.GetMoveDeltaX() < 0)
            randZ = Random.Range(-80, -10);
        else if (CharacterManager.instance.GetMoveDeltaX() > 0)
            randZ = Random.Range(10, 80);*/

        //gameObject.transform.localRotation = new Quaternion(0, 0, randZ, 0);
        //gameObject.transform.position -= gameObject.transform.up * throwSpeed * Time.deltaTime;
        gameObject.transform.position += new Vector3(randX, throwSpeed, 0) * Time.deltaTime;
    }
}
