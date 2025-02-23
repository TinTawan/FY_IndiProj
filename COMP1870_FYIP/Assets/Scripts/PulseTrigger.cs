using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseTrigger : MonoBehaviour
{
    private SphereCollider sphereCol;
    private ParticleSystem ps;

    float timer;


    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        sphereCol = GetComponent<SphereCollider>();

        sphereCol.radius = .5f;
    }

    private void Update()
    {
        ExpandColWithPS();

    }


    void ExpandColWithPS()
    {
        if(ps.isPlaying)
        {
            //timer += Time.deltaTime;

            ParticleSystem.SizeOverLifetimeModule sol = ps.sizeOverLifetime;
            float delta = Mathf.Clamp01(ps.time / ps.main.startLifetime.constant);
            float currentSize = sol.size.Evaluate(delta);

            sphereCol.radius = currentSize * ps.main.startSize.constant * 0.5f;
        }
        else
        {
            sphereCol.radius = 0.5f;
            //timer = 0;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"HIT {other.name}");

        Renderer rend = other.GetComponent<Renderer>();

        if (other.TryGetComponent(out ObjectOutline outline) && rend.isVisible)
        {
            if (!outline.GetIsOutlined())
            {
                outline.SetCanBeTriggered(true);

            }
            else
            {
                outline.SetCanBeTriggered(false);
            }
        }


    }


}
