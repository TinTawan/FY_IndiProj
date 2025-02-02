using System.Collections;
using System.Collections.Generic;
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

    //current pulse
    /*GameObject pulsePrefab;
    float pulseDuration;
    int pulseSize;*/

    //Switch pulse
    int currentPulse;


    [Header("Pulse Cooldown")]
    [SerializeField] float highCD = 2f;
    [SerializeField] float lowCD = 1f;
    float hTimer, lTimer;

    bool canHPulse, canLPulse;
    bool canPulse;


    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Enable();
        playerInput.Player.Echo.performed += Echo_performed;
        playerInput.Player.SwitchPulse.performed += SwitchPulse_performed;
    }
    private void Start()
    {
        //start game with high freq pulse
        /*pulsePrefab = highPulsePrefab;
        pulseDuration = highPulseDuration;
        pulseSize = highPulseSize;*/
        
        currentPulse = 0;
        //canPulse = canHPulse;

    }

    private void Update()
    {
        PulseCooldown();
    }

    private void SwitchPulse_performed(InputAction.CallbackContext ctx)
    {
        currentPulse += (int)ctx.ReadValue<float>();
        currentPulse = Mathf.Clamp(currentPulse, 0, 1);

        if (currentPulse == 0)
        {
            /*pulsePrefab = highPulsePrefab;
            pulseDuration = highPulseDuration;
            pulseSize = highPulseSize;*/

            Debug.Log($"Current pulse: {currentPulse} : High Frequency");

        }
        if (currentPulse == 1)
        {
            /*pulsePrefab = lowPulsePrefab;
            pulseDuration = lowPulseDuration;
            pulseSize = lowPulseSize;*/

            Debug.Log($"Current pulse: {currentPulse} : Low Frequency");

        }

    }

    void Echo_performed(InputAction.CallbackContext ctx)
    {
        if (canHPulse && currentPulse == 0)
        {
            GameObject pulse = Instantiate(highPulsePrefab, transform.position, Quaternion.identity);
            ParticleSystem pulsePS = pulse.transform.GetComponentInChildren<ParticleSystem>();

            pulsePS.Stop();

            ParticleSystem.MainModule pulseMain = pulsePS.main;
            pulseMain.startLifetime = highPulseDuration;
            pulseMain.startSize = highPulseSize;

            pulsePS.Play();


            Destroy(pulse, highPulseDuration + 1f);

            canHPulse = false;
            hTimer = highCD;
        }
        if (canLPulse && currentPulse == 1)
        {
            GameObject pulse = Instantiate(lowPulsePrefab, transform.position, Quaternion.identity);
            ParticleSystem pulsePS = pulse.transform.GetComponentInChildren<ParticleSystem>();

            pulsePS.Stop();

            ParticleSystem.MainModule pulseMain = pulsePS.main;
            pulseMain.startLifetime = lowPulseDuration;
            pulseMain.startSize = lowPulseSize;

            pulsePS.Play();


            Destroy(pulse, lowPulseDuration + 1f);

            canLPulse = false;
            lTimer = lowCD;
        }
        
    }

    void PulseCooldown()
    {
        //Debug.Log($"HTimer: {hTimer}");
        if(hTimer <= 0)
        {
            canHPulse = true;
        }
        else
        {
            hTimer -= Time.deltaTime;
        }

        //Debug.Log($"LTimer: {lTimer}");
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

    private void OnDisable()
    {
        playerInput.Player.Disable();
    }
}
