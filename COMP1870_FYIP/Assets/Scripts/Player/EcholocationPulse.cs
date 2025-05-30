using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class EcholocationPulse : MonoBehaviour
{
    [Header("Echo pulses")]
    [SerializeField] private GameObject highPulsePrefab;
    [SerializeField] private GameObject lowPulsePrefab;
    [SerializeField] private float highPulseDuration, lowPulseDuration;
    [SerializeField] private int highPulseSize, lowPulseSize;

    PlayerInput playerInput;
    PlayerMovement player;

    //Switch pulse
    int currentPulse = 1;


    [Header("Pulse Cooldown")]
    [SerializeField] float highCD = 2f;
    [SerializeField] float lowCD = 1f;
    float hTimer, lTimer;

    bool canHPulse, canLPulse;

    [Header("Pulse Haptics")]
    [SerializeField][Range(0,1)] float lowHapticStrength = 0.7f;
    [SerializeField][Range(0,1)] float lowHapticDuration = 0.3f;
    [SerializeField][Range(0,1)] float highHapticStrength = 0.3f;
    [SerializeField][Range(0,1)] float highHapticDuration = 0.2f;



    private void Awake()
    {
        playerInput = InputManager.Instance.playerInput;
        playerInput.Player.Enable();
        playerInput.Player.Echolocate.performed += Echo_performed;
        playerInput.Player.HighPulse.performed += HighPulse_performed;
        playerInput.Player.LowPulse.performed += LowPulse_performed;

    }


    private void Start()
    {
        player = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        PulseCooldown();

    }

    private void HighPulse_performed(InputAction.CallbackContext ctx)
    {
        if (!GameMenuManager.instance.isPaused)
        {
            //clamp the current pulse so it can only be 0 or 1
            //currentPulse += (int)ctx.ReadValue<float>();
            //currentPulse = Mathf.Clamp(currentPulse, 0, 1);

            currentPulse = 1;
        }
        
    }
    private void LowPulse_performed(InputAction.CallbackContext ctx)
    {
        if (!GameMenuManager.instance.isPaused)
        {
            currentPulse = 0;
        }

    }

    void Echo_performed(InputAction.CallbackContext ctx)
    {
        if (!GameMenuManager.instance.isPaused)
        {
            if (canHPulse && currentPulse == 1)
            {
                HapticManager.instance.HapticFeedback(highHapticStrength, 0, highHapticDuration);

                AudioManager.instance.PlaySound(AudioManager.soundType.highPulseOut, transform.position, 0.1f);

                GameObject pulse = Instantiate(highPulsePrefab, transform.position, Quaternion.identity);
                ParticleSystem pulsePS = pulse.transform.GetComponentInChildren<ParticleSystem>();

                pulsePS.Stop();

                ParticleSystem.MainModule pulseMain = pulsePS.main;
                pulseMain.startLifetime = highPulseDuration;
                pulseMain.duration = highPulseDuration;
                pulseMain.startSize = highPulseSize;

                pulsePS.Play();


                Destroy(pulse, highPulseDuration + 1f);

                canHPulse = false;
                hTimer = highCD;

                if (player.inArea)
                {
                    player.hurt = true;
                }
            }
            if (canLPulse && currentPulse == 0)
            {
                HapticManager.instance.HapticFeedback(0, lowHapticStrength, lowHapticDuration);

                AudioManager.instance.PlaySound(AudioManager.soundType.lowPulseOut, transform.position, 0.1f);

                GameObject pulse = Instantiate(lowPulsePrefab, transform.position, Quaternion.identity);
                ParticleSystem pulsePS = pulse.transform.GetComponentInChildren<ParticleSystem>();

                pulsePS.Stop();

                ParticleSystem.MainModule pulseMain = pulsePS.main;
                pulseMain.startLifetime = lowPulseDuration;
                pulseMain.duration = lowPulseDuration;
                pulseMain.startSize = lowPulseSize;

                pulsePS.Play();


                Destroy(pulse, lowPulseDuration + 1f);

                canLPulse = false;
                lTimer = lowCD;
            }
        }

        

    }

    void PulseCooldown()
    {
        if (hTimer <= 0)
        {
            canHPulse = true;

        }
        else
        {
            hTimer -= Time.deltaTime;
        }

        if (lTimer <= 0)
        {
            canLPulse = true;

        }
        else
        {
            lTimer -= Time.deltaTime;

        }

    }

    public float GetHTimer()
    {
        return hTimer;
    }
    public float GetMaxHighCD()
    {
        return highCD;
    }
    public float GetLTimer()
    {
        return lTimer;
    }
    public float GetMaxLowCD()
    {
        return lowCD;
    }

    public int GetCurrentPulse()
    {
        return currentPulse;
    }


    private void OnDisable()
    {
        playerInput.Player.Disable();
    }
}
