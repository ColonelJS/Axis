using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpChristmas : MonoBehaviour
{
    public static PopUpChristmas instance;

    [SerializeField] private GameObject textToWink;
    [SerializeField] float cooldownWink = 1.5f;
    [SerializeField] float winkTime = 0.5f;
    [SerializeField] GameObject imgLock;
    float cooldownWinkBase;
    float winkTimeBase;

	private void Awake()
	{
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);                 
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
	}

	void Start()
    {
        cooldownWinkBase = cooldownWink;
        winkTimeBase = winkTime;
    }

    
    void Update()
    {
        if (cooldownWink <= 0)
        {
            textToWink.SetActive(false);

            if (winkTime <= 0)
			{
                textToWink.SetActive(true);
                winkTime = winkTimeBase;
                cooldownWink = cooldownWinkBase;
            }
            else
                winkTime -= Time.deltaTime;
        }
        else
            cooldownWink -= Time.deltaTime;
    }

    public void ClosePopUp()
	{
        Destroy(imgLock);
        gameObject.SetActive(false);
	}
}
