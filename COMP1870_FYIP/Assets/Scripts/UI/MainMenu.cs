using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainSection, settingsSection, audioSection, controlsSection, creditsSection;
    [SerializeField] Slider masterSlider, musicSlider, sfxSlider;

    private const string MASTER_VOL_PARAM = "Master";
    private const string MUSIC_VOL_PARAM = "Music";
    private const string SFX_VOL_PARAM = "SFX";

    private void Awake()
    {
        masterSlider.onValueChanged.AddListener(delegate { MasterSliderChanged(); });
        musicSlider.onValueChanged.AddListener(delegate { MusicSliderChanged(); });
        sfxSlider.onValueChanged.AddListener(delegate { SFXSliderChanged(); });

    }

    private void MasterSliderChanged()
    {
        AudioMixerManager.instance.SetMasterVol(masterSlider.value);
    }
    private void MusicSliderChanged()
    {
        AudioMixerManager.instance.SetMusicVol(musicSlider.value);
    }
    private void SFXSliderChanged()
    {
        AudioMixerManager.instance.SetSFXVol(sfxSlider.value);
    }



    void Start()
    {
        AudioMixerManager.instance.LoadVolSettings();

        mainSection.SetActive(true);
        settingsSection.SetActive(false);
        audioSection.SetActive(false);
        controlsSection.SetActive(false);
        creditsSection.SetActive(false);

        masterSlider.value = PlayerPrefs.GetFloat(MASTER_VOL_PARAM);
        musicSlider.value = PlayerPrefs.GetFloat(MUSIC_VOL_PARAM);
        sfxSlider.value = PlayerPrefs.GetFloat(SFX_VOL_PARAM);
    }


    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void Quit()
    {
        Application.Quit();
    }

}
