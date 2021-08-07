using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    [SerializeField] private Image logoGa;
    private Color logoColor = new Color(255, 255, 255, 255);
    float cooldownLogoLeft = 2f;

    void Start()
    {
        logoGa.color = logoColor;
    }

    void Update()
    {
        if(cooldownLogoLeft <= 0)
		{
            SceneManager.LoadScene("Game");
		}
        else
            cooldownLogoLeft -= Time.deltaTime;
    }
    
    void UpdateLogoColor()
	{

	}
}
