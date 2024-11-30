using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseTrigger : MonoBehaviour
{
    SphereCollider sphereCol;

    ParticleSystem ps;
    //List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

    private void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();
        sphereCol = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        ExpandColWithPS();
    }

    /*private void OnParticleTrigger()
    {
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        for(int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = enter[i];
            Debug.Log($"HIT {p}");
        }
    }*/

    void ExpandColWithPS()
    {
        /*ParticleSystem.MainModule main = ps.main;
        float max = main.startSize.constant;
        float delta = ps.time / main.startLifetime.constant;
        sphereCol.radius = max * delta * 0.5f;*/

        ParticleSystem.SizeOverLifetimeModule sol = ps.sizeOverLifetime;
        float delta = Mathf.Clamp01(ps.time / ps.main.startLifetime.constant);
        float currentSize = sol.size.Evaluate(delta);

        sphereCol.radius = currentSize * ps.main.startSize.constant * 0.5f;

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"HIT {other.name}");
    }
}
