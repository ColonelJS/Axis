using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyer : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.gameObject.tag == "Meteorite" || collision.gameObject.tag == "Fuel" || 
            collision.gameObject.tag == "Shield" || collision.gameObject.tag == "Vortex" || 
            collision.gameObject.tag == "Alien" || collision.gameObject.tag == "MiniAlien")
            Destroy(collision.gameObject);
    }
}
