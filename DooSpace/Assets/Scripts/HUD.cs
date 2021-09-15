using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD instance;

    [SerializeField] private Image fuelBar;
    [SerializeField] private Image fuelBarSurcharge;
    [SerializeField] private Text scoreText;
    [SerializeField] private RectTransform progressBarRect;
    [SerializeField] private RectTransform playerPos;
    [SerializeField] private GameObject hudBg;

    private void Awake()
	{
        if (instance == null)
            instance = this;
	}

	void Start()
    {
        
    }

    void Update()
    {
        if (GameManager.instance.GetGameState() == GameManager.GameState.GAME || GameManager.instance.GetGameState() == GameManager.GameState.ALIEN_WAVE)
        {
            UpdateFuelBar();
            UpdateScore();
            UpdateWaveProgress();
        }
    }

    public void UpdateFuelBar()
	{
        float fuel;
        float surcharge;
        if (CharacterManager.instance.GetFuel() > 100)
        {
            fuel = 100;
            surcharge = CharacterManager.instance.GetFuel() - fuel;
        }
        else
        {
            fuel = CharacterManager.instance.GetFuel();
            surcharge = 0;
        }

        fuelBar.fillAmount = fuel / 100;
        fuelBarSurcharge.fillAmount = surcharge / 100;
    }

    void UpdateScore()
	{
        scoreText.text = ((int)CharacterManager.instance.GetScore()).ToString() + "m";      
    }

    void UpdateWaveProgress()
	{
        float progress = CharacterManager.instance.GetScore() * progressBarRect.rect.width / CharacterManager.instance.GetScoreNeededToAlienWave();
        progress -= progressBarRect.rect.width * CharacterManager.instance.GetCurrentAlienWaveIndex();
        playerPos.anchoredPosition = new Vector2(progress, playerPos.anchoredPosition.y);
    }

    public void CloseHUD()
	{
        hudBg.SetActive(false);
	}
}
