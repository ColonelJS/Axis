using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienWave : MonoBehaviour
{
    [SerializeField] GameObject miniAlien;
    [SerializeField] GameObject fuel;
    [SerializeField] float cooldownSpawnWaveAlien = 0.33f;
    [SerializeField] int alienToSpawn = 20;
    [SerializeField] float spawnGap = 3;
    [Space(8)]
    [SerializeField] private GameObject parentTransform;

    bool isWaveSetup = false;
    float randStartPos;
    float spawnPos;
    float baseCooldownSpawnWaveAlien;
    string spawnDirection;
    int alienIndex;
    int screenLimit = 25;

    void Start()
    {
        baseCooldownSpawnWaveAlien = cooldownSpawnWaveAlien;
        cooldownSpawnWaveAlien = 1.8f; // set first cooldown
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
        GameManager.instance.SetGameState(GameManager.GameState.GAME);
        CharacterManager.instance.ResetAlienWaveSet();
    }

    void SwitchSpawnDirection()
	{
        if (spawnDirection == "right")
            spawnDirection = "left";
        else if (spawnDirection == "left")
            spawnDirection = "right";
    }
}
