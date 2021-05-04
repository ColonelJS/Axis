using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject meteorite;
    [SerializeField] private GameObject fuel;
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject alien;
    [SerializeField] private GameObject parentElements;

    //int[] corridor;
    float baseCooldownSpawn = 2f;
    float cooldownSpawn = 2f;
    bool spawnFuel = false;
    bool spawnBonus = false;
    float cooldownFuel = 4f;
    bool canSpawnFuel = true;
    bool canSpawnBonus = true;

    int indexSpawnBonus = 0;

    void Start()
    {
        //corridor = new int[3];
    }

    void Update()
    {
        if (GameManager.instance.GetGameState() == GameManager.GameState.GAME)
            UpdateSpawn();
    }

    void SpawnObstacle()
	{
        float randPos = Random.Range(-35f, 35f);
        float randRot = Random.Range(0f, 360f);
        float randScale = Random.Range(1.3f, 2.6f);
        GameObject newMeteorite = Instantiate(meteorite, new Vector3(randPos, 70, 0), new Quaternion(0, 0, randRot, 0), parentElements.transform);
        newMeteorite.transform.localScale = new Vector3(randScale, randScale, randScale); 
        cooldownSpawn = baseCooldownSpawn;
        spawnFuel = true;
    }

	private void GetToSpawnFuel()
	{
        if (cooldownFuel <= 0)
        {
            spawnFuel = true;
            cooldownFuel = 3f;
        }
        else
            cooldownFuel -= Time.deltaTime;
    }

    void SpawnFuel()
    {
        if (canSpawnFuel)
        {
            float randPos = Random.Range(-30f, 30f);
            //float randRot = Random.Range(0f, 360f);
            GameObject newFuel = Instantiate(fuel, new Vector3(randPos, 70, 0), new Quaternion(0, 0, 0, 0), parentElements.transform);
            spawnFuel = false;

            if (indexSpawnBonus >= 3)
            {
                spawnBonus = true;
            }
            else
                indexSpawnBonus++;
        }
        canSpawnFuel = !canSpawnFuel;
    }

    void SpawnBonus()
	{
        int randBonus = 0;
        float randItem = Random.Range(1f, 100f);
        if (randItem <= 50)
            randBonus = 1;
        else if(randItem > 50 && randItem <= 100) //80
            randBonus = 2;

        if (randBonus == 1)
            SpawnAlien();
        if (randBonus == 2)
            SpawnShield();

        indexSpawnBonus = 0;
	}

    void SpawnShield()
	{
        float randPos = Random.Range(-30f, 30f);
        GameObject newShield = Instantiate(shield, new Vector3(randPos, 70, 0), new Quaternion(0, 0, 0, 0), parentElements.transform);
        spawnBonus = false;
    }

    void SpawnAlien()
    {
        float randPos = Random.Range(-30f, 30f);
        GameObject newShield = Instantiate(alien, new Vector3(randPos, 70, 0), new Quaternion(0, 0, 0, 0), parentElements.transform);
        spawnBonus = false;
    }

    void UpdateSpawn()
	{
        if (cooldownSpawn <= 0)
            SpawnObstacle();
        else
        {
            if (cooldownSpawn < baseCooldownSpawn / 2 && spawnFuel)
            {
                SpawnFuel();
            }
            else
            {
                if (cooldownSpawn < baseCooldownSpawn / 4 && spawnBonus)
                    SpawnBonus();
                else
                    cooldownSpawn -= Time.deltaTime;
            }
        }
    }
}
