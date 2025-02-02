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
    GameObject pulsePrefab;
    float pulseDuration;
    int pulseSize;


    //Switch pulse
    int currentPulse;


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
        pulsePrefab = highPulsePrefab;
        pulseDuration = highPulseDuration;
        pulseSize = highPulseSize;
        currentPulse = 0;
    }

    private void SwitchPulse_performed(InputAction.CallbackContext ctx)
    {
        currentPulse += (int)ctx.ReadValue<float>();
        currentPulse = Mathf.Clamp(currentPulse, 0, 1);

        if (currentPulse == 0)
        {
            pulsePrefab = highPulsePrefab;
            pulseDuration = highPulseDuration;
            pulseSize = highPulseSize;

            Debug.Log($"Current pulse: {currentPulse} : High Frequency");

        }
        if (currentPulse == 1)
        {
            pulsePrefab = lowPulsePrefab;
            pulseDuration = lowPulseDuration;
            pulseSize = lowPulseSize;

            Debug.Log($"Current pulse: {currentPulse} : Low Frequency");

        }

    }

    void Echo_performed(InputAction.CallbackContext ctx)
    {
        GameObject pulse = Instantiate(pulsePrefab, transform.position, Quaternion.identity);
        ParticleSystem pulsePS = pulse.transform.GetComponentInChildren<ParticleSystem>();

        pulsePS.Stop();

        ParticleSystem.MainModule pulseMain = pulsePS.main;
        pulseMain.startLifetime = pulseDuration;
        pulseMain.startSize = pulseSize;

        pulsePS.Play();


        Destroy(pulse, pulseDuration + 1f);
    }



    private void OnDisable()
    {
        playerInput.Player.Disable();
    }
}
