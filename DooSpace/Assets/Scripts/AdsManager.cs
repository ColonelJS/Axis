using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static AdsManager instance;

    [SerializeField] private GameObject popUp;

	private void Awake()
	{
        if (instance == null)
            instance = this;
	}

	void Start()
    {
        popUp.SetActive(false);
    }

    void Update()
    {
        
    }

    public void OpenPopUp()
	{
        popUp.SetActive(true);
    }
}
