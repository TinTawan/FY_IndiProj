using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendEchoPulse : MonoBehaviour
{
    [SerializeField] private GameObject pulsePrefab;
    [SerializeField] private float pulseDuration;
    [SerializeField] private int pulseSize;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Pulse();
        }
    }

    void Pulse()
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
}
