using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions : MonoBehaviour
{
    [SerializeField] private GameObject gainText;
    [SerializeField] private Color32 gainColor;
    [SerializeField] private Color32 scoreGainColor;
    [SerializeField] private Color32 loseColor;

    void Update()
    {

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (GameManager.instance.GetGameState() == GameManager.GameState.GAME || GameManager.instance.GetGameState() == GameManager.GameState.ALIEN_WAVE)
        {
            if (!CharacterManager.instance.GetHasShield())
            {
                if (!CharacterManager.instance.GetHasVortex())
                {
                    if (collision.gameObject.tag == "Meteorite")
                    {
                        SoundManager.instance.PlaySound("meteorite");
                        CharacterManager.instance.MeteoriteCollision();
                        collision.gameObject.GetComponentInParent<Meteorite>().StartBreaked();
                        gainText.GetComponent<AutoFade>().StartFade("-100pts", loseColor);
                        //Destroy(collision.gameObject);
                    }
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
                gainText.GetComponent<AutoFade>().StartFade("+" + CharacterManager.instance.GetAlienBonusMoney().ToString() + "$", gainColor);
            }

            if (collision.gameObject.tag == "MiniAlien")
            {
                SoundManager.instance.PlaySound("alien");
                collision.gameObject.GetComponent<Alien>().throwAlien();
                CharacterManager.instance.MiniAlienCollision();
                gainText.GetComponent<AutoFade>().StartFade("+" + CharacterManager.instance.GetMiniAlienBonusScore().ToString() + "pts", scoreGainColor);
            }

            if (collision.gameObject.tag == "Vortex")
            {
                SoundManager.instance.PlaySound("shield");
                CharacterManager.instance.VortexCollision();
                Destroy(collision.gameObject);
            }
        }
    }
}
