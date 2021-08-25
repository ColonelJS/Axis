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
    int miniAlienHit = 0;
    int meteoriteHit = 0;

    bool hasShield;
    bool hasVortex = false;
    float cooldownShieldBase;
    float cooldownVortexBase;
    float cooldownShield = 5f;
    float cooldownVortex= 8f;

    float vortexAttractionSpeed = 75f;

    float score = 0;
    float moneyAlienBonus = 80f;
    float scoreMiniAlienBonus = 20f;
    bool alienWaveSet = false;
    int alienNextWaveIndex = 1;

    int playerChestLevel;

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
        playerChestLevel = PlayerPrefs.GetInt("playerChestLevel", 0);
    }

    void Update()
    {
        if (GameManager.instance.GetGameState() == GameManager.GameState.GAME || GameManager.instance.GetGameState() == GameManager.GameState.ALIEN_WAVE)
            UpdateElements();
    }

    public void MeteoriteCollision()
	{

        int toRemove = 45 - 3 * CustomScreen.instance.GetBumperLevel();
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
        //score += scoreAlienBonus;
        alienHit++;
    }

    public void MiniAlienCollision()
    {
        //score += scoreAlienBonus;
        miniAlienHit++;
    }

    public void VortexCollision()
    {
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

    public int GetNbMiniAlienHit()
    {
        return miniAlienHit;
    }

    public int GetNbMeteoriteHit()
    {
        return meteoriteHit;
    }

    public int GetAlienBonusMoney()
    {
        return (int)moneyAlienBonus;
    }

    public int GetMiniAlienBonusScore()
    {
        return (int)scoreMiniAlienBonus;
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
        float toRemove = 1.2f * CustomScreen.instance.GetWingLevel();

        float vortexFactor;
        if (!hasVortex)
            vortexFactor = 1;
        else
            vortexFactor = 1.2f;

        if (GameManager.instance.GetGameState() != GameManager.GameState.ALIEN_WAVE)
        {
            if (fuel > 100)
                fuel -= ((33 / vortexFactor) - toRemove) * GameManager.instance.GetSpeedFactor() * Time.deltaTime;
            else
                fuel -= ((23 / vortexFactor) - toRemove) * GameManager.instance.GetSpeedFactor() * Time.deltaTime;
        }
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

        if (!alienWaveSet)
        {
            if (score >= alienNextWaveIndex * 2000)
			{
                GameManager.instance.SetGameState(GameManager.GameState.ALIEN_WAVE);
                alienNextWaveIndex++;
                alienWaveSet = true;
			}
        }
    }

    public void ResetAlienWaveSet()
	{
        alienWaveSet = false;
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

    public int GetPlayerChestLevel()
	{
        return playerChestLevel;
	}

    public void IncrementPlayerChestLevel()
	{
        playerChestLevel++;
        PlayerPrefs.SetInt("playerChestLevel", playerChestLevel);
	}
}
