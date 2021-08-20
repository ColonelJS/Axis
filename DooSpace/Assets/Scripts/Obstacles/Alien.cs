using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    float throwSpeed = 160f;
    bool isThrow = false;
    float randX;
    float randZ;
    int randRot;
    float rotation = 0f;
    float rotationSpeed = 225;
    float cooldownAutodestruct = 3f;

    void Update()
    {
        if (isThrow)
            UpdateThrow();
        else
            if (GameManager.instance.GetGameState() == GameManager.GameState.GAME || GameManager.instance.GetGameState() == GameManager.GameState.ALIEN_WAVE)
                MoveElement();
    }

    public void MoveElement()
    {
        if (!CharacterManager.instance.GetHasVortex())
            gameObject.transform.position += new Vector3(0, GameManager.instance.GetScrolingSpeed(), 0) * Time.deltaTime;
        else
        {
            Vector3 toCharacterVector = CharacterManager.instance.GetCharacterPosition() - gameObject.transform.position;
            gameObject.transform.position += new Vector3(0, GameManager.instance.GetScrolingSpeed(), 0) * Time.deltaTime;
            gameObject.transform.position += (toCharacterVector.normalized * CharacterManager.instance.GetVortexAttractionSpeed()) * Time.deltaTime;
        }
    }

    public void throwAlien()
	{
        isThrow = true;

        randZ = Random.Range(-90, 90);
        randX = Random.Range(-180, 180);
        randRot = Random.Range(0, 2);
    }

    void UpdateThrow()
	{
        gameObject.transform.position += new Vector3(randX, throwSpeed, 0) * Time.deltaTime;

        if (randRot == 0)
            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.rotation.x, gameObject.transform.rotation.y, -rotation);
        else if (randRot == 1)
            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.rotation.x, gameObject.transform.rotation.y, rotation);
        rotation += rotationSpeed * Time.deltaTime;

        if (cooldownAutodestruct <= 0)
            Destroy(this.gameObject);
        else
            cooldownAutodestruct -= Time.deltaTime;
    }
}
