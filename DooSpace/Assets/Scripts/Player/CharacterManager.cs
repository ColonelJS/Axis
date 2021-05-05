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
    float scoreAlienBonus = 200f;

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
        RemoveFuel(45);
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
        if(fuel > 100)
            fuel -= 36 * Time.deltaTime;
        else
            fuel -= 24 * Time.deltaTime;
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
        score -= GameManager.instance.GetScrolingSpeed()/400;
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
