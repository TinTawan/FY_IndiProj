using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;

    public void SetMasterVol(float vol)
    {
        audioMixer.SetFloat("masterVol", Mathf.Log10(vol) * 20f);
    }

    public void SetMusicVol(float vol)
    {
        audioMixer.SetFloat("musicVol", Mathf.Log10(vol) * 20f);

    }

    public void SetSFXVol(float vol)
    {
        audioMixer.SetFloat("sfxVol", Mathf.Log10(vol) * 20f);

    }
}
