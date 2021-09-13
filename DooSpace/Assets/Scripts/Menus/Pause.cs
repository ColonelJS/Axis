using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject menuPauseBg;
    [SerializeField] private Slider slider;
    [SerializeField] private Slider settingsSlider;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSourceMusic;
    [SerializeField] private AudioSource audioSourceRocket;
    [Space(10)]
    [SerializeField] private GameObject soundCross;
    [SerializeField] private GameObject soundBar1;
    [SerializeField] private GameObject soundBar2;
    [SerializeField] private GameObject soundBar3;

    float savedSliderValue;
    bool isPaused = false;

    void Start()
    {
        savedSliderValue = PlayerPrefs.GetFloat("volume", 1);
        slider.value = savedSliderValue;
        audioSource.volume = slider.value;
        audioSourceMusic.volume = slider.value /*- slider.value / 3*/;
        audioSourceRocket.volume = slider.value;
        SetSoundSprite();
        menuPauseBg.SetActive(false);
    }

    void Update()
    {
        UpdateVolume();
    }

    public void OpenMenuPause()
	{
        menuPauseBg.SetActive(true);
        audioSourceRocket.Pause();
        audioSourceMusic.Pause();
        Time.timeScale = 0;

        isPaused = true;
    }

    public void CloseMenuPause()
    {
        menuPauseBg.SetActive(false);
        audioSourceRocket.UnPause();
        audioSourceMusic.UnPause();
        Time.timeScale = 1;
        isPaused = false;
    }

    public void SwitchMenuPause()
	{
        if(isPaused)
		{
            CloseMenuPause();
        }
        else
		{
            OpenMenuPause();
        }
	}

    public bool GetIsPause()
	{
        return isPaused;
	}

    void UpdateVolume()
    {
        if (slider.value != savedSliderValue)
        {
            audioSource.volume = slider.value;
            audioSourceMusic.volume = slider.value /*- slider.value / 3*/;
            audioSourceRocket.volume = slider.value;

            savedSliderValue = slider.value;
            settingsSlider.value = slider.value;

            SetSoundSprite();
            PlayerPrefs.SetFloat("volume", slider.value);
        }

        //print("volume : " + audioSourceMusic.volume);
    }

    void SetSoundSprite()
    {
        UnactiveSoundSprites();
        if (slider.value <= 0)
            soundCross.SetActive(true);
        else if (slider.value > 0 && slider.value <= 0.33)
            soundBar1.SetActive(true);
        else if (slider.value > 0.33 && slider.value <= 0.66)
        {
            soundBar1.SetActive(true);
            soundBar2.SetActive(true);
        }
        else if (slider.value > 0.66 && slider.value <= 1)
        {
            soundBar1.SetActive(true);
            soundBar2.SetActive(true);
            soundBar3.SetActive(true);
        }
    }

    void UnactiveSoundSprites()
    {
        soundCross.SetActive(false);
        soundBar1.SetActive(false);
        soundBar2.SetActive(false);
        soundBar3.SetActive(false);
    }
}
