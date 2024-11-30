using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseTrigger : MonoBehaviour
{
    SphereCollider sphereCol;
    ParticleSystem ps;

    private void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();
        sphereCol = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        ExpandColWithPS();
    }


    void ExpandColWithPS()
    {
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
