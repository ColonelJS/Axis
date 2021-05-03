using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject meteorite;
    [SerializeField] private GameObject parentElements;

    //int[] corridor;
    float baseCooldownSpawn = 1f;
    float cooldownSpawn = 1f;

    void Start()
    {
        //corridor = new int[3];
    }

    void Update()
    {
        if (cooldownSpawn <= 0)
            SpawnObstacle();
        else
            cooldownSpawn -= Time.deltaTime;
    }

    void SpawnObstacle()
	{
        float randPos = Random.Range(-30f, 30f);
        float randRot = Random.Range(0f, 360f);
        float randScale = Random.Range(0.5f, 1.5f);
        GameObject newMeteorite = Instantiate(meteorite, new Vector3(randPos, 70, 0), new Quaternion(0, 0, randRot, 0), parentElements.transform);
        newMeteorite.transform.localScale = new Vector3(randScale, randScale, randScale); 
        cooldownSpawn = baseCooldownSpawn;
    }
}
