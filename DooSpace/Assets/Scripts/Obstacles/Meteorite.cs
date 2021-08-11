using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Meteorite : GameElements
{
    [SerializeField] private Image img;
    [SerializeField] private Sprite sp1;
    [SerializeField] private Sprite sp2;

	private void Start()
	{
        int randSp = Random.Range(0, 2);
        if (randSp == 0)
            img.sprite = sp1;
        if (randSp == 1)
            img.sprite = sp2;
    }

	void Update()
    {
        if (GameManager.instance.GetGameState() == GameManager.GameState.GAME)
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
    }
}
