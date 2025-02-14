using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEcholocationPulse : MonoBehaviour
{
    [Header("Burst info")]
    [SerializeField] int burstCount;
    [SerializeField] [Range(0.2f, 0.6f)] float burstInterval = 0.4f;

    [Header("Particle System info")]
    [SerializeField] float fullDuration;
    [SerializeField] float lifetime, size;
    [SerializeField] Color pulseColour = Color.white;

    ParticleSystem ps;
    ParticleSystem.MainModule psMain;
    ParticleSystem.ColorOverLifetimeModule psCol;
    ParticleSystem.EmissionModule psEm;
    float burstTime = 0f;

    private void Awake()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        psMain = ps.main;
        psCol = ps.colorOverLifetime;
        psEm = ps.emission;

        SetPS();
        SetBurst();

    }
    private void Start()
    {
        ps.Play();

    }

    void SetPS()
    {
        //set colour and colorOverLifetime colour
        /*ParticleSystem.MinMaxGradient colour = pulseColour;
        psMain.startColor = colour.color;*/


        Gradient grad = new Gradient();
        grad.SetKeys(
            new GradientColorKey[] { new GradientColorKey(pulseColour, 0.0f), new GradientColorKey(pulseColour, 1.0f)}, 
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 0.95f)}
        );
        psCol.color = new ParticleSystem.MinMaxGradient(grad);

        //set timings
        psMain.duration = fullDuration;
        psMain.startLifetime = lifetime;

        //set size (radius of sphere)
        psMain.startSize = size;
    }

    void SetBurst()
    {
        burstTime = 0f;

        //set burst count
        ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[burstCount];
        psEm.SetBursts(bursts);

        //set new bursts with given interval
        for (int i = 0; i < burstCount; i++)
        {
            ParticleSystem.Burst psB = new ParticleSystem.Burst(burstTime, 1);
            psEm.SetBurst(i, psB);
            burstTime += burstInterval;

        }
    }

    /*private void OnValidate()
    {
        SetPS();
        SetBurst();
    }*/

}
