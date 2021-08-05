using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    void Start()
    {
        SetupMenuResolution();
    }

    void Update()
    {
        
    }

    Vector2 GetResolution()
	{
        Vector2 res;
        res.x = (float)Screen.currentResolution.width;
        res.y = (float)Screen.currentResolution.height;
        return res;
	}

    void SetupMenuResolution()
	{

	}
}
