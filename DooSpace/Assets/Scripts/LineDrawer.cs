using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] LineRenderer line;
    [SerializeField] GameObject attachPoint;
    [SerializeField] GameObject gear;

    Vector3[] positions = new Vector3[2];

    void Start()
    {
        UpdatePositions();
    }

    void Update()
    {
        
    }

    void UpdatePositions()
	{
        positions[0] = attachPoint.transform.position;
        positions[1] = gear.transform.position;
        line.SetPositions(positions);
    }
}
