using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;

    float fuel = 100;

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
        
    }

    public void MeteoriteCollision()
	{
        print("meteorite collision");
        RemoveFuel(50);
    }

    void RemoveFuel(float _amount)
	{
        fuel -= _amount;
	}
}
