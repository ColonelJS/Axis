using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Meteorite : GameElements
{
    [SerializeField] private GameObject shape1;
    [SerializeField] private GameObject shape2;
    float cooldownAutodestruct = 2f;
    float randRot;

    private void Start()
	{
        int randSp = Random.Range(0, 2);
        int randRotSide = Random.Range(0, 2);
        if (randRotSide == 0)
            randRot = Random.Range(-85, -35);
        else if (randRotSide == 1)
            randRot = Random.Range(35, 85);

        if (randSp == 0)
        {
            shape1.SetActive(true);
            shape2.SetActive(false);
        }
        if (randSp == 1)
        {
            shape2.SetActive(true);
            shape1.SetActive(false);
        }
    }

	void Update()
    {
        if (GameManager.instance.GetGameState() == GameManager.GameState.GAME || GameManager.instance.GetGameState() == GameManager.GameState.ALIEN_WAVE)
            MoveElement();
    }

    public override void MoveElement()
	{
        if (!CharacterManager.instance.GetHasVortex())
            gameObject.transform.position += new Vector3(0, GameManager.instance.GetScrolingSpeed(), 0) * Time.deltaTime;
        else
        {
            Vector3 toCharacterVector = CharacterManager.instance.GetCharacterPosition() - gameObject.transform.position;
            gameObject.transform.position += new Vector3(0, GameManager.instance.GetScrolingSpeed(), 0) * Time.deltaTime;
            gameObject.transform.position += (toCharacterVector.normalized * CharacterManager.instance.GetVortexAttractionSpeed()/2.5f) * Time.deltaTime;
        }

        gameObject.transform.eulerAngles += new Vector3(0, 0, randRot) * Time.deltaTime;

        if (gameObject.transform.localPosition.y <= -110)
		{
            Destroy(gameObject);
        }
    }
}
