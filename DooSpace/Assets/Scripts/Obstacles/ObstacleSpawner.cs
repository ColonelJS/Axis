using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject meteorite;
    [SerializeField] private GameObject fuel;
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject alien;
    [SerializeField] private GameObject vortex;
    [SerializeField] private GameObject parentElements;

    float baseCooldownSpawn = 1.8f;
    float cooldownSpawn = 1.8f;
    bool spawnFuel = false;
    bool spawnBonus = false;
    float cooldownFuel = 4f;
    bool canSpawnFuel = true;
    bool isDestroyMeteorite = false;

    int indexSpawnBonus = 0;
    int screenLimit = 25;

    List<GameObject> listMeteoriteGo;

    void Start()
    {
        listMeteoriteGo = new List<GameObject>();

    }

    void Update()
    {
        if (GameManager.instance.GetGameState() == GameManager.GameState.GAME)
            UpdateSpawn();

        if(isDestroyMeteorite)
		{
            DeleteAllMeteorite();
            isDestroyMeteorite = false;
		}
    }

    void SpawnObstacle()
	{
        float randPos = Random.Range(-30f, 30f);
        float randRot = Random.Range(0, 361);
        float randScale = Random.Range(1.35f, 2.1f);

        GameObject newMeteorite = Instantiate(meteorite, new Vector3(randPos, 70, 0), new Quaternion(), parentElements.transform);
        newMeteorite.transform.localScale = new Vector3(randScale, randScale, randScale);
        newMeteorite.transform.eulerAngles = new Vector3(0, 0, randRot);

        listMeteoriteGo.Add(newMeteorite);

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

    void DeleteAllMeteorite()
	{
        for(int i = 0; i < listMeteoriteGo.Count; i++)
		{
            Destroy(listMeteoriteGo[i]);
		}
        listMeteoriteGo.Clear();
	}

    public void SetIsDestroyMeteorite()
	{
        isDestroyMeteorite = true;
    }

    void SpawnFuel()
    {
        if (canSpawnFuel)
        {
            float randPos = Random.Range(-screenLimit, screenLimit);
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
        if (randItem <= 66)
            randBonus = 1;
        else if(randItem > 66 && randItem <= 91)
            randBonus = 2;
        else if(randItem > 91 && randItem <= 100)
            randBonus = 3;

        if (randBonus == 1)
            SpawnAlien();
        if (randBonus == 2)
            SpawnShield();
        if (randBonus == 3)
        {
            if(!CharacterManager.instance.GetHasVortex())
                SpawnVortex();
            else
                SpawnAlien();
        }

        indexSpawnBonus = 0;
	}

    void SpawnVortex()
	{
        float randPos = Random.Range(-screenLimit, screenLimit);
        GameObject newVortex = Instantiate(vortex, new Vector3(randPos, 70, 0), new Quaternion(0, 0, 0, 0), parentElements.transform);
        spawnBonus = false;
    }

    void SpawnShield()
	{
        float randPos = Random.Range(-screenLimit, screenLimit);
        GameObject newShield = Instantiate(shield, new Vector3(randPos, 70, 0), new Quaternion(0, 0, 0, 0), parentElements.transform);
        spawnBonus = false;
    }

    void SpawnAlien()
    {
        float randPos = Random.Range(-screenLimit, screenLimit);
        GameObject newAlien = Instantiate(alien, new Vector3(randPos, 70, 0), new Quaternion(0, 0, 0, 0), parentElements.transform);
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
