using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;
    [SerializeField] CharacterMovement characterMovement;
    [SerializeField] GameObject shieldImg;

    float fuel = 200;
    int alienHit = 0;
    int meteoriteHit = 0;
    bool hasShield;
    float cooldownShieldBase = 4f;
    float cooldownShield = 4f;

    float score = 0;
    float scoreAlienBonus = 120f;

    //int bumperLevel = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        shieldImg.SetActive(false);
    }

    void Update()
    {
        if (GameManager.instance.GetGameState() == GameManager.GameState.GAME)
            UpdateElements();
    }

    public void MeteoriteCollision()
	{

        int toRemove = 45 - 4 * CustomScreen.instance.GetBumperLevel();
        print("meteorite hit, to remove : " + toRemove);
        RemoveFuel(toRemove);
        if (fuel < 0)
            fuel = 0;
        meteoriteHit++;
    }

    void RemoveFuel(float _amount)
	{
        fuel -= _amount;
	}

    public void FuelCollision()
	{
        GameManager.instance.SetIsMiniBoost();
        AddFuel(55);
    }

    void UpdateElements()
	{
        UpdateFuel();
        UpdateShield();
        UpdateScore();
    }

    public void ShieldCollision()
	{
        hasShield = true;
	}

    public void AlienCollision()
    {
        score += scoreAlienBonus;
        alienHit++;
    }

    public int GetNbAlienHit()
	{
        return alienHit;
	}

    public int GetNbMeteoriteHit()
    {
        return meteoriteHit;
    }

    public int GetAlienBonusScore()
    {
        return (int)scoreAlienBonus;
    }

    public float GetScore()
	{
        return score;
	}

    public void RemoveShield()
	{
        hasShield = false;
    }

    void AddFuel(float _amount)
	{
        fuel += _amount;
    }

    public float GetFuel()
	{
        return fuel;
	}

    void UpdateFuel()
	{
        float toRemove = 1.5f * CustomScreen.instance.GetWingLevel();

        if(fuel > 100)
            fuel -= (33-toRemove) * GameManager.instance.GetSpeedFactor() * Time.deltaTime;
        else
            fuel -= (23-toRemove) * GameManager.instance.GetSpeedFactor() * Time.deltaTime;
    }

    void UpdateShield()
	{
        if (hasShield)
        {
            shieldImg.SetActive(true);
            if (cooldownShield <= 0)
            {
                hasShield = false;
                cooldownShield = cooldownShieldBase;
            }
            else
                cooldownShield -= Time.deltaTime;
        }
        else
            shieldImg.SetActive(false);
    }

    void UpdateScore()
	{
        score -= GameManager.instance.GetScrolingSpeed()/400 * 60 * Time.deltaTime;
    }

    public bool GetHasShield()
	{
        return hasShield;
	}

    public float GetCharacterPosX()
	{
        return gameObject.transform.position.x;
	}

    public float GetMoveDeltaX()
	{
        return characterMovement.GetMoveDeltaX();
	}
}
