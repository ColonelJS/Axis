using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private GameObject sliderGo;
    [SerializeField] private Slider slider;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSourceMusic;

    [SerializeField] private AudioClip clickPlay;
    [SerializeField] private AudioClip buyUpgrade;
    [SerializeField] private AudioClip openInfo;
    [SerializeField] private AudioClip fuel;
    [SerializeField] private AudioClip meteorite;
    [SerializeField] private AudioClip shield;
    [SerializeField] private AudioClip alien;
    [SerializeField] private AudioClip fall;
    [SerializeField] private AudioClip openEnterName;

    [SerializeField] private AudioClip rocket;
    [SerializeField] private AudioClip mainMenu;
    [SerializeField] private AudioClip highscore;
    [SerializeField] private AudioClip custom;
    [Space(10)]
    [SerializeField] private GameObject soundCross;
    [SerializeField] private GameObject soundBar1;
    [SerializeField] private GameObject soundBar2;
    [SerializeField] private GameObject soundBar3;

    Dictionary<string, AudioClip> listSound = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> listMusic = new Dictionary<string, AudioClip>();
    float savedSliderValue;
    private void Awake()
	{
        if (instance == null)
            instance = this;
	}

	void Start()
    {
        savedSliderValue = PlayerPrefs.GetFloat("volume", 1);
        slider.value = savedSliderValue;
        audioSource.volume = slider.value;
        audioSourceMusic.volume = slider.value - slider.value / 3;
        SetSoundSprite();

        AddSoundsToList();
    }

    void Update()
    {
        UpdateVolume();
    }

    void AddSoundsToList()
	{
        listSound.Add("clickPlay", clickPlay);
        listSound.Add("buyUpgrade", buyUpgrade);
        listSound.Add("openInfo", openInfo);
        listSound.Add("fuel", fuel);
        listSound.Add("meteorite", meteorite);
        listSound.Add("shield", shield);
        listSound.Add("alien", alien);
        listSound.Add("fall", fall);
        listSound.Add("openEnterName", openEnterName);

        listMusic.Add("rocket", rocket);
        listMusic.Add("mainMenu", mainMenu);
        listMusic.Add("highscore", highscore);
        listMusic.Add("custom", custom);
    }

    public void PlaySound(string _soundName)
	{
        audioSource.clip = listSound[_soundName];
        audioSource.Play();
	}

    public void PlayMusic(string _musicName)
    {
        audioSourceMusic.clip = listMusic[_musicName];
        audioSourceMusic.Play();
    }

    public void StopMusic()
    {
        audioSourceMusic.Stop();
    }

    public bool GetSliderOpen()
	{
        if (sliderGo.activeSelf)
            return true;
        else
            return false;
	}

    public void CloseSlider()
	{
        sliderGo.SetActive(false);
	}

    void UpdateVolume()
	{
        if(slider.value != savedSliderValue)
		{
            audioSource.volume = slider.value;
            audioSourceMusic.volume = slider.value - slider.value/3;

            savedSliderValue = slider.value;

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

    public void SwitchSliderState()
	{
        if (sliderGo.activeSelf)
            sliderGo.SetActive(false);
        else
            sliderGo.SetActive(true);
    }
}
