using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions : MonoBehaviour
{
    [SerializeField] private GameObject gainText;
    [SerializeField] private Color32 gainColor;
    [SerializeField] private Color32 scoreGainColor;
    [SerializeField] private Color32 loseColor;

    int nbShieldHit = 0;
    int nbVortexHit = 0;

    float cooldownMeteorite = 2f;
    void Update()
    {
        if(cooldownMeteorite > 0)
            cooldownMeteorite -= Time.deltaTime;

        if(nbShieldHit == 5 && nbVortexHit == 5)
            GooglePlayServicesManager.instance.ReportSucces("CgkI6LzEr7kGEAIQFA", 100f); //ACHIEVEMENT 10 / untouchable
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (GameManager.instance.GetGameState() == GameManager.GameState.GAME || GameManager.instance.GetGameState() == GameManager.GameState.ALIEN_WAVE)
        {
            if (!CharacterManager.instance.GetHasShield())
            {
                if (!CharacterManager.instance.GetHasVortex())
                {
                    if (collision.gameObject.tag == "Meteorite" && cooldownMeteorite <= 0)
                    {
                        SoundManager.instance.PlaySound("meteorite");
                        CharacterManager.instance.MeteoriteCollision();
                        collision.gameObject.GetComponentInParent<Meteorite>().StartBreaked();
                        gainText.GetComponent<AutoFade>().StartFade("-100pts", loseColor);
                        cooldownMeteorite = 2f;
                    }
                }
            }
            /*else
            if (collision.gameObject.tag == "Meteorite")
            {
                CharacterManager.instance.RemoveShield();
                cooldownMeteorite = 2f;
            }*/

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
                nbShieldHit++;
            }

            if (collision.gameObject.tag == "Alien")
            {
                SoundManager.instance.PlaySound("alien");
                collision.gameObject.GetComponent<Alien>().throwAlien();
                CharacterManager.instance.AlienCollision();
                gainText.GetComponent<AutoFade>().StartFade("+" + CharacterManager.instance.GetAlienBonusMoney().ToString() + "$", gainColor);

                //king of the world
                GooglePlayServicesManager.instance.incrementSucces("CgkI6LzEr7kGEAIQCQ", 1); //ACHIEVEMENT 3  ///succes steps : 10
                GooglePlayServicesManager.instance.incrementSucces("CgkI6LzEr7kGEAIQCg", 1); //ACHIEVEMENT 3.2 ///succes steps : 100
                GooglePlayServicesManager.instance.incrementSucces("CgkI6LzEr7kGEAIQCw", 1); //ACHIEVEMENT 3.3 ///succes steps : 1000
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
                nbVortexHit++;
            }
        }
    }
}
