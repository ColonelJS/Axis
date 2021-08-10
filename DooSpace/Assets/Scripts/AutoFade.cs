using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoFade : MonoBehaviour
{
    [SerializeField] float fadeSpeed = 0.0015f;
    [SerializeField] float cooldownToFade = 2f;
    [SerializeField] Text text;
    Color color;
    Color gainColor;
    Color loseColor;
    bool isFade = false;
    float startCooldown;

	private void Awake()
	{
        color = text.color;
        startCooldown = cooldownToFade;
        SetOpacityMin();
    }

	void Start()
    {
        gainColor = Color.yellow;
        loseColor = Color.red;
    }

    void Update()
    {
        if (isFade)
            CooldownFade();
    }

    void SetOpacityMax()
	{
        color.a = 1;
        text.color = color;
        cooldownToFade = startCooldown;
    }

    public void StartFade()
	{
        SetOpacityMax();
        isFade = true;
    }

    public void StartFade(string _textStr, Color _color)
    {
        text.text = _textStr;
        color = _color;
        SetOpacityMax();
        isFade = true;
    }

    void SetOpacityMin()
	{
        color.a = 0;
        cooldownToFade = startCooldown;
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
        Debug.Log("gain text opacity : " + text.color.a);
        if (color.a > 0)
        {
            color.a -= fadeSpeed * Time.deltaTime;
            text.color = color;
        }
        else
            SetOpacityMin();
    }
}
