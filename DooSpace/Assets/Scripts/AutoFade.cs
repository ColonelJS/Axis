using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoFade : MonoBehaviour
{
    [SerializeField] float fadeSpeed = 10f;
    [SerializeField] float cooldownToFade = 1.2f;
    [SerializeField] Text text;
    Color color;
    bool isFade = false;

	private void Awake()
	{
        color = text.color;
        SetOpacityMin();
    }

	void Start()
    {

    }

    void Update()
    {
        if (isFade)
            CooldownFade();
    }

    void SetOpacityMax()
	{
        color.a = 0;
        text.color = color;
    }

    public void StartFade()
	{
        SetOpacityMax();
        isFade = true;
    }

    void SetOpacityMin()
	{
        color.a = 255;
        text.color = color;
        isFade = false;
    }

    void CooldownFade()
	{
        if (cooldownToFade <= 0)
        {
            UpdateFade();
        }
        else
            cooldownToFade -= Time.deltaTime;
    }

    void UpdateFade()
	{
        if (color.a > 0)
        {
            color.a -= fadeSpeed * Time.deltaTime;
            text.color = color;
        }
        else
            SetOpacityMin();
    }
}
