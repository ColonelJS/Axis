using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyer : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.gameObject.tag == "Meteorite" || collision.gameObject.tag == "Fuel" || 
            collision.gameObject.tag == "Shield" || collision.gameObject.tag == "Alien" || collision.gameObject.tag == "Vortex")
            Destroy(collision.gameObject);
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
        print("is cillideee");
    }
}
