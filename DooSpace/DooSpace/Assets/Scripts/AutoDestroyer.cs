using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyer : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.gameObject.tag == "Meteorite" || collision.gameObject.tag == "Fuel" || collision.gameObject.tag == "Shield" || collision.gameObject.tag == "Alien")
            Destroy(collision.gameObject);
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
        print("is cillideee");
    }
}
