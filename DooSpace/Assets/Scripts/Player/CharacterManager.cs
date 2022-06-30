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
    [SerializeField] HUD hud;

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
    float moneyAlienBonus = 50f;
    float scoreMiniAlienBonus = 10f;
    float scoreNeededToAlienWave = 2000f;
    bool alienWaveSet = false;
    int alienNextWaveIndex = 1;
    bool scoreToGoogleUpdated = false;

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
        playerChestLevel = ZPlayerPrefs.GetInt("playerChestLevel", 3);
        ZPlayerPrefs.SetInt("playerChestLevel", playerChestLevel);
    }

    void Update()
    {
        if (GameManager.instance.GetGameState() == GameManager.GameState.GAME || GameManager.instance.GetGameState() == GameManager.GameState.ALIEN_WAVE)
            UpdateElements();
    }

    public void MeteoriteCollision()
	{
        float toRemove = 49 - 1.85f * CustomScreen.instance.GetBumperLevel(); //49 -> 2.25
        RemoveFuel(toRemove);
        if (fuel < 0)
            fuel = 0;
        meteoriteHit++;
        hud.UpdateFuelBar();
        if (meteoriteHit == 5)
            GooglePlayServicesManager.instance.ReportSucces("CgkI6LzEr7kGEAIQEQ", 100f); //ACHIEVEMENT 7 / i will survive
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
        alienHit++;
    }

    public void MiniAlienCollision()
    {
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
        if (fuel > 200f)
            fuel = 200f;

        float toRemove = 1f * CustomScreen.instance.GetWingLevel();

        float vortexFactor;
        if (!hasVortex)
            vortexFactor = 1;
        else
            vortexFactor = 1.33f;

        if (GameManager.instance.GetGameState() != GameManager.GameState.ALIEN_WAVE)
        {
            if (fuel > 100)
                fuel -= ((35f / vortexFactor) - toRemove) * Time.deltaTime;
            else
                fuel -= ((25f / vortexFactor) - toRemove) * Time.deltaTime;
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
            if (score >= alienNextWaveIndex * scoreNeededToAlienWave)
			{
                GameManager.instance.SetGameState(GameManager.GameState.ALIEN_WAVE);
                alienNextWaveIndex++;
                alienWaveSet = true;
                if(alienNextWaveIndex == 6)
                    GooglePlayServicesManager.instance.ReportSucces("CgkI6LzEr7kGEAIQEg", 100f); //ACHIEVEMENT 8 / get away
            }
        }

        if (((int)score % 50) == 0)
        {
            if (!scoreToGoogleUpdated)
            {
                Debug.Log("UPDATE TRAVELER ACHIEVEMENT : " + (int)score);
                //traveller
                GooglePlayServicesManager.instance.incrementSucces("CgkI6LzEr7kGEAIQAw", 1); //ACHIEVEMENT 1  ///succes steps : 200 -> 10.000 / 50
                GooglePlayServicesManager.instance.incrementSucces("CgkI6LzEr7kGEAIQBA", 1); //ACHIEVEMENT 1.2 ///succes steps : 1000 -> 50.000 / 50
                GooglePlayServicesManager.instance.incrementSucces("CgkI6LzEr7kGEAIQBQ", 1); //ACHIEVEMENT 1.3 ///succes steps : 2000 -> 100.000 / 50
                scoreToGoogleUpdated = true;
            }
        }
        else
            scoreToGoogleUpdated = false;
    }

    float GetCurveNextAlienWave()
	{
        float a = 2;
        float b = 1800;
        float c = 2f;
        int x = (alienNextWaveIndex * 10);

        float curve = a * Mathf.Pow(x, c) + b + a;
        return curve;
	}

    public int GetCurrentAlienWaveIndex()
	{
        return alienNextWaveIndex-1;
    }

    public float GetScoreNeededToAlienWave()
	{
        return scoreNeededToAlienWave;
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
        ZPlayerPrefs.SetInt("playerChestLevel", playerChestLevel);
	}
}
