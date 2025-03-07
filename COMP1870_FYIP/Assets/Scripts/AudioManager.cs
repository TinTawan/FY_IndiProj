using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum soundType
    {
        slip,
        presentHit,
        presentBreak,
        winCheer,
        partyPopper,
        sad,
        buttonHover,
        buttonClick,
        alien

    }

    public GameObject audioObject;

    [SerializeField] AudioClip[] slipSound;
    [SerializeField] AudioClip[] presentHitSound;
    [SerializeField] AudioClip[] presentBreakSound;
    [SerializeField] AudioClip[] winCheerSound;
    [SerializeField] AudioClip[] partyPopperSound;
    [SerializeField] AudioClip[] sadSound;
    [SerializeField] AudioClip[] buttonHoverSound;
    [SerializeField] AudioClip[] buttonClickSound;
    [SerializeField] AudioClip[] alienSound;



    public void PlaySound(soundType type, Vector3 pos/*, float vol*/, float pitchDelta)
    {
        GameObject newSound = Instantiate(audioObject, pos, Quaternion.identity);
        AudioObject soundObj = newSound.GetComponent<AudioObject>();

        switch (type)
        {
            case (soundType.slip):
                soundObj.SetClip(slipSound[Random.Range(0, slipSound.Length)]);
                break;
            case (soundType.presentHit):
                soundObj.SetClip(presentHitSound[Random.Range(0, presentHitSound.Length)]);
                break;
            case (soundType.presentBreak):
                soundObj.SetClip(presentBreakSound[Random.Range(0, presentBreakSound.Length)]);
                break;
            case (soundType.winCheer):
                soundObj.SetClip(winCheerSound[Random.Range(0, winCheerSound.Length)]);
                break;
            case (soundType.partyPopper):
                soundObj.SetClip(partyPopperSound[Random.Range(0, partyPopperSound.Length)]);
                break;
            case (soundType.sad):
                soundObj.SetClip(sadSound[Random.Range(0, sadSound.Length)]);
                break;
            case (soundType.buttonHover):
                soundObj.SetClip(buttonHoverSound[Random.Range(0, buttonHoverSound.Length)]);
                break;
            case (soundType.buttonClick):
                soundObj.SetClip(buttonClickSound[Random.Range(0, buttonClickSound.Length)]);
                break;
            case (soundType.alien):
                soundObj.SetClip(alienSound[Random.Range(0, alienSound.Length)]);
                break;

        }

        //soundObj.SetVolume(vol);
        soundObj.SetPitchDelta(Random.Range(-pitchDelta, pitchDelta));

        soundObj.StartAudio();
    }

    public void ButtonClickSound()
    {
        PlaySound(soundType.buttonClick, transform.position, 0);


    }
    public void ButtonHoverSound()
    {
        PlaySound(soundType.buttonHover, transform.position, 0);

    }

    //FindObjectOfType<AudioManager>().PlaySound(AudioManager.soundType.buttonPress, transform.position, 0);

}