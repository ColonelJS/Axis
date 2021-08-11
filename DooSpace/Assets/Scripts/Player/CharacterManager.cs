using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;
    [SerializeField] CharacterMovement characterMovement;
    [SerializeField] GameObject shieldImg;
    [SerializeField] GameObject model;
    [SerializeField] VortexEffect vortexEffect;

    float fuel = 200;
    int alienHit = 0;
    int meteoriteHit = 0;

    bool hasShield;
    bool hasVortex = false;
    float cooldownShieldBase;
    float cooldownVortexBase;
    float cooldownShield = 5f;
    float cooldownVortex= 10f;

    float vortexAttractionSpeed = 75f;

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
        vortexEffect.enabled = false;
        cooldownShieldBase = cooldownShield;
        cooldownVortexBase = cooldownVortex;
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
        UpdateVortex();
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

    public void VortexCollision()
    {
        //SetActiveModel(false);
        vortexEffect.enabled = true;
        hasVortex = true;
    }

    void EndVortex()
    {
        vortexEffect.enabled = false;
        hasVortex = false;
    }

    public Vector3 GetCharacterPosition()
	{
        return model.transform.position;
	}

    public bool GetHasVortex()
	{
        return hasVortex;
	}

    public float GetVortexAttractionSpeed()
	{
        return vortexAttractionSpeed;
	}

    void SetActiveModel(bool _state)
	{
        if (_state == false)
            model.SetActive(false);
        else if(_state == true)
            model.SetActive(true);
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

    public void SetFuel(float _amount)
    {
        fuel = _amount;
    }

    public float GetFuel()
	{
        return fuel;
	}

    void UpdateFuel()
	{
        float toRemove = 1.5f * CustomScreen.instance.GetWingLevel();

        float vortexFactor;
        if (!hasVortex)
            vortexFactor = 1;
        else
            vortexFactor = 1.2f;

        if (fuel > 100)
            fuel -= ((33/vortexFactor) - toRemove) * GameManager.instance.GetSpeedFactor() * Time.deltaTime;
        else
            fuel -= ((23/vortexFactor) - toRemove) * GameManager.instance.GetSpeedFactor() * Time.deltaTime;
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

    void UpdateVortex()
	{
        if (hasVortex)
        {
            if (cooldownVortex <= 0)
            {
                EndVortex();
                cooldownVortex = cooldownVortexBase;
            }
            else
                cooldownVortex -= Time.deltaTime;
        }
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
