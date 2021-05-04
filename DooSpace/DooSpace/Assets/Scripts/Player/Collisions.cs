using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
        Debug.Log("colliiision 2d");
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (!CharacterManager.instance.GetHasShield())
        {
            if (collision.gameObject.tag == "Meteorite")
                CharacterManager.instance.MeteoriteCollision();
        }
        else
            CharacterManager.instance.RemoveShield();

        if (collision.gameObject.tag == "Fuel")
            CharacterManager.instance.FuelCollision();

        if (collision.gameObject.tag == "Shield")
            CharacterManager.instance.ShieldCollision();

        if (collision.gameObject.tag == "Alien")
        {
            collision.gameObject.GetComponent<Alien>().throwAlien();
            CharacterManager.instance.AlienCollision();
        }
    }
}
