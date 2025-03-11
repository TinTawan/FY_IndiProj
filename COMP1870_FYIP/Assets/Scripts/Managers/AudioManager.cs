using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public enum soundType
    {
        lowPulseOut,
        lowPulseHit,
        highPulseOut,
        highPulseHit,
        emitterPulseOut,
        emitterPulseHit,
        canInteract,
        itemPickUp,
        itemDrop,
        itemPlaced,



    }

    public GameObject audioObject;

    [SerializeField] AudioClip[] lowPulseOutSound;
    [SerializeField] AudioClip[] lowPulseHitSound;
    [SerializeField] AudioClip[] highPulseOutSound;
    [SerializeField] AudioClip[] highPulseHitSound;
    [SerializeField] AudioClip[] emitterPulseOutSound;
    [SerializeField] AudioClip[] emitterPulseHitSound;
    [SerializeField] AudioClip[] canInteractSound;
    [SerializeField] AudioClip[] itemPickUpSound;
    [SerializeField] AudioClip[] itemDropSound;
    [SerializeField] AudioClip[] itemPlacedSound;




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


    public void PlaySound(soundType type, Vector3 pos, float pitchDelta)
    {
        GameObject newSound = Instantiate(audioObject, pos, Quaternion.identity);
        AudioObject soundObj = newSound.GetComponent<AudioObject>();

        switch (type)
        {
            case (soundType.lowPulseOut):
                soundObj.SetClip(lowPulseOutSound[Random.Range(0, lowPulseOutSound.Length)]);
                break;
            case (soundType.lowPulseHit):
                soundObj.SetClip(lowPulseHitSound[Random.Range(0, lowPulseHitSound.Length)]);
                break;
            case (soundType.highPulseOut):
                soundObj.SetClip(highPulseOutSound[Random.Range(0, highPulseOutSound.Length)]);
                break;
            case (soundType.highPulseHit):
                soundObj.SetClip(highPulseHitSound[Random.Range(0, highPulseHitSound.Length)]);
                break;
            case (soundType.emitterPulseOut):
                soundObj.SetClip(emitterPulseOutSound[Random.Range(0, emitterPulseOutSound.Length)]);
                break;
            case (soundType.emitterPulseHit):
                soundObj.SetClip(emitterPulseHitSound[Random.Range(0, emitterPulseHitSound.Length)]);
                break;
            case (soundType.canInteract):
                soundObj.SetClip(canInteractSound[Random.Range(0, canInteractSound.Length)]);
                break;
            case (soundType.itemPickUp):
                soundObj.SetClip(itemPickUpSound[Random.Range(0, itemPickUpSound.Length)]);
                break;
            case (soundType.itemDrop):
                soundObj.SetClip(itemDropSound[Random.Range(0, itemDropSound.Length)]);
                break;
            case (soundType.itemPlaced):
                soundObj.SetClip(itemPlacedSound[Random.Range(0, itemPlacedSound.Length)]);
                break;


        }

        soundObj.SetPitchDelta(Random.Range(-pitchDelta, pitchDelta));

        soundObj.StartAudio();
    }


    //AudioManager.instance.PlaySound(AudioManager.soundType.lowPulseOut, transform.position, 0);

}