using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseTrigger : MonoBehaviour
{
    [SerializeField] GameObject lightObj;
    [SerializeField] float lowPulseDuration = 3f, highPulseDuration = 8f, emittingObjectPulseDuration = 4f;
    private SphereCollider sphereCol;
    private ParticleSystem ps;

    Light pulseLight;

    bool doOnce = true;

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

            ParticleSystem.SizeOverLifetimeModule sol = ps.sizeOverLifetime;
            float delta = Mathf.Clamp01(ps.time / ps.main.startLifetime.constant);
            float currentSize = sol.size.Evaluate(delta);

            sphereCol.radius = currentSize * ps.main.startSize.constant * 0.5f;

            if (doOnce)
            {
                GameObject obj = Instantiate(lightObj, transform.position, Quaternion.identity);
                pulseLight = obj.GetComponent<Light>();


                LightPulse lp = pulseLight.GetComponent<LightPulse>();
                lp.SetLifetime(ps.main.startLifetime.constant + 1f);
                lp.SetFadeSpeed(0.1f);


                doOnce = false;
            }

            if(pulseLight != null)
            {
                pulseLight.range = sphereCol.radius;
                pulseLight.intensity = ps.main.startSize.constant / 3;
            }
            


        }
        else
        {
            sphereCol.radius = 0.5f;

        }

    }

    private void OnTriggerEnter(Collider other)
    {       
        Renderer rend = other.GetComponent<Renderer>();

        if (other.TryGetComponent(out ObjectOutline outline) && rend.isVisible)
        {
            if (!outline.GetIsOutlined())
            {
                //outline the object
                outline.SetCanBeTriggered(true);

                //give lower outline time if hit by low pulse
                if (gameObject.CompareTag("LowPulse"))
                {
                    outline.SetOutlineTime(lowPulseDuration);
                }
                //and longer outline time if hit by high pulse
                if (gameObject.CompareTag("HighPulse"))
                {
                    outline.SetOutlineTime(highPulseDuration);

                }
                //and middle time if hit by emitting object
                else
                {
                    outline.SetOutlineTime(emittingObjectPulseDuration);

                }

            }
            else
            {
                outline.SetCanBeTriggered(false);
            }
        }


    }

    
}
