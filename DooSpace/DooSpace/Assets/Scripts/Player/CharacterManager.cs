using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;

    float fuel = 200;

    bool hasShield;
    float cooldownShieldBase = 4f;
    float cooldownShield = 4f;

    float score = 0;
    float scoreAlienBonus = 50f;
    //float cooldownScore = 0.33f;

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
        if (GameManager.instance.GetGameState() == GameManager.GameState.GAME)
            UpdateElements();
    }

    public void MeteoriteCollision()
	{
        RemoveFuel(40);
    }

    void RemoveFuel(float _amount)
	{
        fuel -= _amount;
	}

    public void FuelCollision()
	{
        GameManager.instance.SetIsMiniBoost();
        AddFuel(50);
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
        fuel -= 20 * Time.deltaTime;
    }

    void UpdateShield()
	{
        if (hasShield)
        {
            if (cooldownShield <= 0)
            {
                hasShield = false;
                cooldownShield = cooldownShieldBase;
            }
            else
                cooldownShield -= Time.deltaTime;
        }
    }

    void UpdateScore()
	{
        score -= GameManager.instance.GetScrolingSpeed()/100;
    }

    public bool GetHasShield()
	{
        return hasShield;
	}
}
