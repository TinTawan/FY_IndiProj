using UnityEngine;

public class AudioObject : MonoBehaviour
{
    AudioSource audioSource;

    AudioClip clip;
    float vol = 1f, pitch = 1f;


    public void StartAudio()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = vol;
        audioSource.pitch = 1f + pitch;
        audioSource.Play();

        Destroy(gameObject, audioSource.clip.length);
    }

    public void SetClip(AudioClip inClip)
    {
        clip = inClip;
    }
    public void SetVolume(float inVol)
    {
        vol = inVol;
    }
    public void SetPitchDelta(float inDelta)
    {
        pitch = inDelta;
    }

}