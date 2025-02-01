using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EcholocationPulse : MonoBehaviour
{
    [SerializeField] private GameObject pulsePrefab;
    [SerializeField] private float pulseDuration;
    [SerializeField] private int pulseSize;

    PlayerInput playerInput;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Enable();
        playerInput.Player.Echo.performed += Echo_performed;
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
