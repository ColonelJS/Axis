using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienWave : MonoBehaviour
{
    [SerializeField] GameObject miniAlien;
    [SerializeField] GameObject fuel;
    [SerializeField] float cooldownSpawnWaveAlien = 0.12f;//0.4
    [SerializeField] float cooldownSpawnWaveAlienRandom = 0.2f;//0.6
    [SerializeField] int alienToSpawn = 50;
    [SerializeField] float spawnGap = 4;
    [Space(8)]
    [SerializeField] private GameObject parentTransform;

    bool isWaveSetup = false;
    bool endWave = false;
    float cooldownEndWave = 1f;
    float randStartPos;
    float spawnPos;
    float baseCooldownSpawnWaveAlien;
    float baseCooldownSpawnWaveAlienRandom;
    string spawnDirection;
    int alienIndex;
    int screenLimit = 20;

    void Start()
    {
        baseCooldownSpawnWaveAlien = cooldownSpawnWaveAlien;
        baseCooldownSpawnWaveAlienRandom = cooldownSpawnWaveAlienRandom;
        cooldownSpawnWaveAlien = 1.8f; // set first cooldown
        cooldownSpawnWaveAlienRandom = 1.86f; // set first cooldown
        spawnPos = randStartPos;
    }

    void Update()
    {
        if (GameManager.instance.GetGameState() == GameManager.GameState.ALIEN_WAVE)
        {
            if (!isWaveSetup)
                SetupAlienWave();
            else
                UpdateAlienWave();

            if(endWave)
			{
                if (cooldownEndWave <= 0)
                {
                    GameManager.instance.SetGameState(GameManager.GameState.GAME);
                    cooldownEndWave = 1f;
                    endWave = false;
                }
                else
                    cooldownEndWave -= Time.deltaTime;
			}
        }
    }

    void SpawnMiniAlien(float _positionX)
    {
        float pos = _positionX;
        GameObject newAlien = Instantiate(miniAlien, new Vector3(pos, 70, 0), new Quaternion(0, 0, 0, 0), parentTransform.transform);
    }

    void SpawnBonusFuel(float _positionX)
	{
        float pos = _positionX;
        GameObject newFuel = Instantiate(fuel, new Vector3(pos, 70, 0), new Quaternion(0, 0, 0, 0), parentTransform.transform);
    }

    void SetupAlienWave()
    {
        randStartPos = Random.Range(-screenLimit, screenLimit);

        int randDirection = Random.Range(0, 2);
        if (randDirection == 0)
            spawnDirection = "right";
        else if (randDirection == 1)
            spawnDirection = "left";

        isWaveSetup = true;
    }

    void UpdateAlienWave()
    {
        if (alienIndex < alienToSpawn)
        {
            if (cooldownSpawnWaveAlienRandom <= 0)
            {
                float randVal;
                if (spawnPos < -screenLimit + 10)
                    randVal = 1;
                else if (spawnPos > screenLimit - 10)
                    randVal = 0;
                else
                    randVal = Random.Range(0, 2);

                float randPos;
                if (randVal == 0)
                    randPos = Random.Range(-screenLimit, spawnPos - 5);
                else
                    randPos = Random.Range(spawnPos + 5, screenLimit);

                SpawnMiniAlien(randPos);

                cooldownSpawnWaveAlienRandom = baseCooldownSpawnWaveAlienRandom;
            }
            else
                cooldownSpawnWaveAlienRandom -= Time.deltaTime;

            if (cooldownSpawnWaveAlien <= 0)
            {
                SpawnMiniAlien(spawnPos);
                cooldownSpawnWaveAlien = baseCooldownSpawnWaveAlien;

                if (spawnDirection == "right")
                    spawnPos += spawnGap;
                else if (spawnDirection == "left")
                    spawnPos -= spawnGap;

                if (spawnPos >= screenLimit || spawnPos <= -screenLimit)
                    SwitchSpawnDirection();

                alienIndex++;
            }
            else
                cooldownSpawnWaveAlien -= Time.deltaTime;           
        }
        else
        {
            SpawnBonusFuel(spawnPos);
            ResetAlienWave();
        }
    }

    void ResetAlienWave()
    {
        alienIndex = 0;
        cooldownSpawnWaveAlien = 1.8f;
        isWaveSetup = false;
        endWave = true;
        CharacterManager.instance.ResetAlienWaveSet();
        GameManager.instance.DeleteAllMeteorite();
    }

    void SwitchSpawnDirection()
	{
        if (spawnDirection == "right")
            spawnDirection = "left";
        else if (spawnDirection == "left")
            spawnDirection = "right";
    }
}
