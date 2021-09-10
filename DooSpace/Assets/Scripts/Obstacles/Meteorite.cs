using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Meteorite : GameElements
{
    [SerializeField] private GameObject shape1;
    [SerializeField] private GameObject shape2;
    [Space(10)]
    [SerializeField] private GameObject shape1_p1;
    [SerializeField] private GameObject shape1_p2;
    [SerializeField] private GameObject shape1_p3;
    [SerializeField] private GameObject shape1_p4;
    [SerializeField] private GameObject shape1_p5;
    [Space(10)]
    [SerializeField] private GameObject shape2_p1;
    [SerializeField] private GameObject shape2_p2;
    [SerializeField] private GameObject shape2_p3;
    [SerializeField] private GameObject shape2_p4;
    [SerializeField] private GameObject shape2_p5;

    List<GameObject> listShape1Parts = new List<GameObject>();
    List<GameObject> listShape2Parts = new List<GameObject>();
    float randRot;
    bool isBreak = false;
    bool isBreakEnd = false;
    int shapeActive = 0;
    float breakSpeed = 50f;
    float fadeSpeed = 3f;
    private void Start()
	{
        int randSp = Random.Range(0, 2);
        int randRotSide = Random.Range(0, 2);
        if (randRotSide == 0)
            randRot = Random.Range(-85, -35);
        else if (randRotSide == 1)
            randRot = Random.Range(35, 85);

        listShape1Parts.Add(shape1_p1);
        listShape1Parts.Add(shape1_p2);
        listShape1Parts.Add(shape1_p3);
        listShape1Parts.Add(shape1_p4);
        listShape1Parts.Add(shape1_p5);

        listShape2Parts.Add(shape2_p1);
        listShape2Parts.Add(shape2_p2);
        listShape2Parts.Add(shape2_p3);
        listShape2Parts.Add(shape2_p4);
        listShape2Parts.Add(shape2_p5);

        if (randSp == 0)
        {
            shape1.SetActive(true);
            shape2.SetActive(false);
            shapeActive = 1;
        }
        if (randSp == 1)
        {
            shape2.SetActive(true);
            shape1.SetActive(false);
            shapeActive = 2;
        }
    }

	void Update()
    {
        if (GameManager.instance.GetGameState() == GameManager.GameState.GAME || GameManager.instance.GetGameState() == GameManager.GameState.ALIEN_WAVE)
            MoveElement();

        if(isBreak)
		{
            if(shapeActive == 1)
			{
                Color newColor = listShape1Parts[0].GetComponent<SpriteRenderer>().color;
                newColor.a -= fadeSpeed * Time.deltaTime;
                if (newColor.a <= 0)
                    isBreakEnd = true;

                for (int i = 0; i < 5; i++)
                {
                    if (i % 2 == 0)
                        listShape1Parts[i].transform.eulerAngles += new Vector3(0, 0, 1) * breakSpeed * Time.deltaTime;
                    else
                        listShape1Parts[i].transform.eulerAngles += new Vector3(0, 0, -1) * breakSpeed * Time.deltaTime;

                    listShape1Parts[i].GetComponent<SpriteRenderer>().color = newColor;
                }

                shape1_p1.transform.position += new Vector3(1, -0.5f, 0) * breakSpeed * Time.deltaTime;
                shape1_p2.transform.position += new Vector3(1.5f, 0, 0) * breakSpeed * Time.deltaTime;
                shape1_p3.transform.position += new Vector3(1, 1.2f, 0) * breakSpeed * Time.deltaTime;

                shape1_p4.transform.position += new Vector3(-1, 1.2f, 0) * breakSpeed * Time.deltaTime;
                shape1_p5.transform.position += new Vector3(-1, -1, 0) * breakSpeed * Time.deltaTime;
            }

            if (shapeActive == 2)
            {
                Color newColor = listShape2Parts[0].GetComponent<SpriteRenderer>().color;
                newColor.a -= fadeSpeed * Time.deltaTime;
                if (newColor.a <= 0)
                    isBreakEnd = true;

                for (int i = 0; i < 5; i++)
                {
                    if (i % 2 == 0)
                        listShape2Parts[i].transform.eulerAngles += new Vector3(0, 0, 1) * breakSpeed * Time.deltaTime;
                    else
                        listShape2Parts[i].transform.eulerAngles += new Vector3(0, 0, -1) * breakSpeed * Time.deltaTime;

                    listShape2Parts[i].GetComponent<SpriteRenderer>().color = newColor;
                }

                shape2_p1.transform.position += new Vector3(1, -0.5f, 0) * breakSpeed * Time.deltaTime;
                shape2_p2.transform.position += new Vector3(1.5f, 0, 0) * breakSpeed * Time.deltaTime;
                shape2_p3.transform.position += new Vector3(1, 1.2f, 0) * breakSpeed * Time.deltaTime;

                shape2_p4.transform.position += new Vector3(-1, 1.2f, 0) * breakSpeed * Time.deltaTime;
                shape2_p5.transform.position += new Vector3(-1, -1, 0) * breakSpeed * Time.deltaTime;
            }
        }
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

        if (gameObject.transform.localPosition.y <= -110 || isBreakEnd)
		{
            Destroy(gameObject);
        }
    }

    public void StartBreaked()
	{
        isBreak = true;
	}
}
