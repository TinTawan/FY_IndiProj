using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameMenuManager : MonoBehaviour
{
    public static GameMenuManager instance;

    [Header("UI Sections")]
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject pauseUI;

    [Header("UI Elements")]
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    private const string MASTER_VOL_PARAM = "Master";
    private const string MUSIC_VOL_PARAM = "Music";
    private const string SFX_VOL_PARAM = "SFX";

    PlayerInput playerInput;

    public bool isPaused { get; set; }

    [SerializeField] List<AudioSource> currentSounds = new List<AudioSource>();


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }


        playerInput = new PlayerInput();
        playerInput.Player.Enable();
        playerInput.Player.Pause.performed += Pause_performed;

        playerInput.UI.Get();
        playerInput.UI.UnPause.performed += UnPause_performed;

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

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        isPaused = !isPaused;
    }
    private void UnPause_performed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        isPaused = !isPaused;

    }


    private void Start()
    {
        gameUI.SetActive(true);
        pauseUI.SetActive(false);

        AudioMixerManager.instance.LoadVolSettings();

        masterSlider.value = PlayerPrefs.GetFloat(MASTER_VOL_PARAM);
        musicSlider.value = PlayerPrefs.GetFloat(MUSIC_VOL_PARAM);
        sfxSlider.value = PlayerPrefs.GetFloat(SFX_VOL_PARAM);
    }

    private void Update()
    {
        if (isPaused)
        {
            Pause();
        }
        else
        {
            UnPause();
        }
    }

    private void Pause()
    {

        //set correct time and panels
        Time.timeScale = 0f;
        gameUI.SetActive(false);
        pauseUI.SetActive(true);

        //set correct input map
        playerInput.Player.SwitchPulse.Disable();
        playerInput.Player.Disable();
        playerInput.UI.Enable();

        Cursor.lockState = CursorLockMode.Confined;

        //find sounds currently playing
        AudioSource[] sounds = FindObjectsOfType<AudioSource>();
        foreach (AudioSource sound in sounds)
        {
            //only add new sounds to list
            if(currentSounds.IndexOf(sound) < 0)
            {
                currentSounds.Add(sound);
            }
        }
        //if there are any, pause them
        if (currentSounds.Count > 0)
        {
            foreach (AudioSource sound in currentSounds)
            {
                if (sound != null)
                {
                    //only pause sound if it isnt on the camera (music does not pause)
                    if(!sound.TryGetComponent(out Camera cam))
                    {
                        sound.Pause();
                    }
                }

            }
        }

        HapticManager.instance.StopHaptics();
    }


    private void UnPause()
    {

        //set correct time and panels
        Time.timeScale = 1f;
        gameUI.SetActive(true);
        pauseUI.SetActive(false);

        //set correct input map
        playerInput.Player.Enable();
        playerInput.UI.Disable();

        Cursor.lockState = CursorLockMode.Locked;

        //if there were any sounds playing when paused, unpause them and clear the array
        if (currentSounds.Count > 0)
        {
            foreach (AudioSource sound in currentSounds)
            {
                if (sound != null)
                {
                    sound.UnPause();
                }
            }

            currentSounds.Clear();
        }

    }

    public void ToMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);

    }

}
