using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    float throwSpeed = 20f;
    bool isThrow = false;

    void Start()
    {
        
    }

    void Update()
    {
        MoveElement();
        if (isThrow)
            UpdateThrow();
    }

    public void MoveElement()
    {
        gameObject.transform.position += new Vector3(0, GameManager.instance.GetScrolingSpeed(), 0) * Time.deltaTime;
    }

    public void throwAlien()
	{
        isThrow = true;
    }

    void UpdateThrow()
	{
        float randZ = Random.Range(-90, 90);
        gameObject.transform.rotation = new Quaternion(0, 0, randZ, 0);
        gameObject.transform.position += gameObject.transform.forward * throwSpeed * Time.deltaTime;
    }
}
