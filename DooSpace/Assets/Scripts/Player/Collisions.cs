using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D collision)
	{
        Debug.Log("colliiision 2d");
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (GameManager.instance.GetGameState() == GameManager.GameState.GAME)
        {
            if (!CharacterManager.instance.GetHasShield())
            {
                if (collision.gameObject.tag == "Meteorite")
                {
                    SoundManager.instance.PlaySound("meteorite");
                    CharacterManager.instance.MeteoriteCollision();
                    Destroy(collision.gameObject);
                }
            }
            else
                if (collision.gameObject.tag == "Meteorite")
                    CharacterManager.instance.RemoveShield();

            if (collision.gameObject.tag == "Fuel")
            {
                SoundManager.instance.PlaySound("fuel");
                CharacterManager.instance.FuelCollision();
                Destroy(collision.gameObject);
            }

            if (collision.gameObject.tag == "Shield")
            {
                SoundManager.instance.PlaySound("shield");
                CharacterManager.instance.ShieldCollision();
                Destroy(collision.gameObject);
            }

            if (collision.gameObject.tag == "Alien")
            {
                SoundManager.instance.PlaySound("alien");
                collision.gameObject.GetComponent<Alien>().throwAlien();
                CharacterManager.instance.AlienCollision();
            }
        }
    }
}
