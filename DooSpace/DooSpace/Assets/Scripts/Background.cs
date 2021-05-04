using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private GameObject baseBackground;
    [SerializeField] private Material material;

    float backgroundSpeed = 100;
    float texOffset = 0;
    float startOffset = 0;
    Vector3 startPosBaseBackground;
    Vector3 endPosBaseBackground;

    void Start()
    {
        startPosBaseBackground = baseBackground.transform.localPosition;
        endPosBaseBackground = startPosBaseBackground + new Vector3(0, -Screen.height, 0);
    }

    void Update()
    {
        if (GameManager.instance.GetGameState() == GameManager.GameState.START)
            AnimateBaseBackground();
        else if (GameManager.instance.GetGameState() == GameManager.GameState.GAME)
            UpdateOffset();
    }

    void AnimateBaseBackground()
	{
        if (baseBackground.transform.localPosition.y > -Screen.height)
        {
            baseBackground.transform.localPosition -= new Vector3(0, backgroundSpeed, 0) * Time.deltaTime;
            backgroundSpeed += 225 * Time.deltaTime;
        }
        else
        {
            baseBackground.transform.localPosition = endPosBaseBackground;
            backgroundSpeed = 100;
            GameManager.instance.SetGameState(GameManager.GameState.GAME);
        }

        material.SetTextureOffset("_MainTex", new Vector2(0, texOffset));
        texOffset += startOffset * Time.deltaTime;
        print("offset : " + startOffset);
        startOffset += 0.0155f * Time.deltaTime;
    }

    void UpdateOffset()
	{
        material.SetTextureOffset("_MainTex", new Vector2(0, texOffset));
        texOffset += 0.08f * Time.deltaTime; //0.075
	}
}
