using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{


    void Start()
    {
        
    }

    void Update()
    {
        MoveElement();
    }

    public void MoveElement()
    {
        gameObject.transform.position += new Vector3(0, GameManager.instance.GetScrolingSpeed(), 0) * Time.deltaTime;
    }
}
