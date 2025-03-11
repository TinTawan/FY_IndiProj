using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerManager : MonoBehaviour
{
    public static AudioMixerManager instance;

    [SerializeField] AudioMixer audioMixer;
    //[SerializeField] Slider masterSlider, musicSlider, sfxSlider;

    private const string MASTER_VOL_PARAM = "Master";
    private const string MUSIC_VOL_PARAM = "Music";
    private const string SFX_VOL_PARAM = "SFX";

    /*public float masterVol { get; set; }
    public float musicVol { get; set; }
    public float sfxVol { get; set; }*/


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        LoadVolSettings();
    }

    public void LoadVolSettings()
    {
        if (PlayerPrefs.HasKey(MASTER_VOL_PARAM))
        {
            float masterVolume = PlayerPrefs.GetFloat(MASTER_VOL_PARAM);
            //masterSlider.value = masterVolume;
            //masterVol = masterVolume;
            SetMasterVol(masterVolume);
        }
        if (PlayerPrefs.HasKey(MUSIC_VOL_PARAM))
        {
            float musicVolume = PlayerPrefs.GetFloat(MUSIC_VOL_PARAM);
            //musicSlider.value = musicVolume;
            //musicVol = musicVolume;
            SetMusicVol(musicVolume);
        }
        if (PlayerPrefs.HasKey(SFX_VOL_PARAM))
        {
            float sfxVolume = PlayerPrefs.GetFloat(SFX_VOL_PARAM);
            //sfxSlider.value = sfxVolume;
            //sfxVol = sfxVolume;
            SetSFXVol(sfxVolume);
        }

    }

    /*private void SaveAudioSettings(float masterVal, float musicVal, float sfxVal)
    {
        PlayerPrefs.SetFloat(MASTER_VOL_PARAM, masterVal); 
        PlayerPrefs.SetFloat(MUSIC_VOL_PARAM, musicVal);
        PlayerPrefs.SetFloat(SFX_VOL_PARAM, sfxVal);
        PlayerPrefs.Save();
    }*/

    public void SetMasterVol(float vol)
    {
        audioMixer.SetFloat("masterVol", Mathf.Log10(vol) * 20f);
        PlayerPrefs.SetFloat(MASTER_VOL_PARAM, vol);
        PlayerPrefs.Save();

    }

    public void SetMusicVol(float vol)
    {
        audioMixer.SetFloat("musicVol", Mathf.Log10(vol) * 20f);
        PlayerPrefs.SetFloat(MUSIC_VOL_PARAM, vol);
        PlayerPrefs.Save();

    }

    public void SetSFXVol(float vol)
    {
        audioMixer.SetFloat("sfxVol", Mathf.Log10(vol) * 20f);
        PlayerPrefs.SetFloat(SFX_VOL_PARAM, vol);
        PlayerPrefs.Save();

    }
}
