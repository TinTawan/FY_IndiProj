using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameMenuManager : MonoBehaviour
{
    public static GameMenuManager instance;

    [Header("UI Sections")]
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject pauseUI;


    PlayerInput playerInput;

    bool isPaused;

    List<AudioSource> currentSounds = new List<AudioSource>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }


        playerInput = new PlayerInput();
        playerInput.Player.Enable();
        playerInput.Player.Pause.performed += Pause_performed;

        playerInput.UI.Get();
        playerInput.UI.UnPause.performed += UnPause_performed;

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
        playerInput.Player.Disable();
        playerInput.UI.Enable();

        Cursor.lockState = CursorLockMode.Confined;

        //find sounds currently playing
        AudioSource[] sounds = FindObjectsOfType<AudioSource>();
        foreach (AudioSource sound in sounds)
        {
            currentSounds.Add(sound);
        }
        //if there are any, pause them
        if (currentSounds.Count > 0)
        {
            foreach (AudioSource sound in currentSounds)
            {
                if(sound != null)
                {
                    sound.Pause();
                }

            }
        }


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

    public void SetIsPaused(bool inBool)
    {
        isPaused = inBool;
    }
}
