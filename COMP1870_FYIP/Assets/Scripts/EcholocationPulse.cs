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

    //Switch pulse
    int currentPulse;


    [Header("Pulse Cooldown")]
    [SerializeField] float highCD = 2f;
    [SerializeField] float lowCD = 1f;
    float hTimer, lTimer;

    bool canHPulse, canLPulse;

    [Header("Visibility")]
    [SerializeField] float sinStrength = 1f;
    [SerializeField] float expandSpeedHigh = 6f, rangeHigh, intensityHigh;
    [SerializeField] float expandSpeedLow = 2f, rangeLow, intensityLow;
    Light pLight;
    bool lightOn = false;


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
        
        currentPulse = 1;
        //canPulse = canHPulse;

        pLight = GetComponentInChildren<Light>();
        pLight.range = 0f;
        pLight.intensity = 0f;

    }

    private void Update()
    {
        PulseCooldown();

        Light();
    }

    private void SwitchPulse_performed(InputAction.CallbackContext ctx)
    {
        currentPulse += (int)ctx.ReadValue<float>();
        currentPulse = Mathf.Clamp(currentPulse, 0, 1);

        if (currentPulse == 1)
        {
            /*pulsePrefab = highPulsePrefab;
            pulseDuration = highPulseDuration;
            pulseSize = highPulseSize;*/

            Debug.Log($"Current pulse: {currentPulse} : High Frequency");

        }
        if (currentPulse == 0)
        {
            /*pulsePrefab = lowPulsePrefab;
            pulseDuration = lowPulseDuration;
            pulseSize = lowPulseSize;*/

            Debug.Log($"Current pulse: {currentPulse} : Low Frequency");

        }

    }

    void Echo_performed(InputAction.CallbackContext ctx)
    {
        lightOn = true;

        if (canHPulse && currentPulse == 1)
        {
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
        }
        if (canLPulse && currentPulse == 0)
        {
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

    void Light()
    {
        if (lightOn)
        {
            pLight.enabled = true;
            pLight.intensity = intensityLow;
            pLight.range = Mathf.Lerp(pLight.range, rangeLow, Time.deltaTime * expandSpeedLow);
        }
        else
        {
            pLight.enabled = false;
            pLight.range = 0f;
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

    public void SetLightOn(bool inBool)
    {
        lightOn = inBool;
    }

    private void OnDisable()
    {
        playerInput.Player.Disable();
    }
}
