using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PulseTrigger : MonoBehaviour
{
    [SerializeField] GameObject lightObj;
    [SerializeField] float lowPulseDuration = 3f, highPulseDuration = 8f, emittingObjectPulseDuration = 4f;
    private SphereCollider sphereCol;
    private ParticleSystem ps;

    Light pulseLight;

    bool doOnce = true;

    PlayerMovement pMovement;
    EcholocationPulse ePulse;

    private HashSet<GameObject> hitObjects = new HashSet<GameObject>();

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        sphereCol = GetComponent<SphereCollider>();

        sphereCol.radius = .2f;

        pMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        ePulse = pMovement.gameObject.GetComponent<EcholocationPulse>();
    }

    private void Update()
    {
        ExpandColWithPS();

    }


    void ExpandColWithPS()
    {
        if (ps.isPlaying)
        {

            ParticleSystem.SizeOverLifetimeModule sol = ps.sizeOverLifetime;
            float delta = Mathf.Clamp01(ps.time / ps.main.startLifetime.constant);
            float currentSize = sol.size.Evaluate(delta);

            sphereCol.radius = currentSize * ps.main.startSize.constant * 0.5f;

            if (doOnce && !pMovement.inArea)
            {

                GameObject obj = Instantiate(lightObj, transform.position, Quaternion.identity);
                pulseLight = obj.GetComponent<Light>();
                LightPulse lp = pulseLight.GetComponent<LightPulse>();
                lp.SetLifetime(ps.main.startLifetime.constant + 1f);
                lp.SetFadeSpeed(0.1f);
    
                doOnce = false;
    
            }
            else if(doOnce && pMovement.inArea && ePulse.GetCurrentPulse() == 0)
            {

                GameObject obj = Instantiate(lightObj, transform.position, Quaternion.identity);
                pulseLight = obj.GetComponent<Light>();
                LightPulse lp = pulseLight.GetComponent<LightPulse>();
                lp.SetLifetime(ps.main.startLifetime.constant + 1f);
                lp.SetFadeSpeed(0.1f);

                doOnce = false;
            }
            else if(doOnce && pMovement.inArea)
            {
                pulseLight = null;
                doOnce = false;
            }
            

            if (pulseLight != null)
            {
                pulseLight.range = sphereCol.radius;
                pulseLight.intensity = ps.main.startSize.constant / 3;
            }



        }
        else
        {
            sphereCol.radius = 0.2f;
            hitObjects.Clear();

        }

    }

    private void OnTriggerEnter(Collider col)
    {
        //objective items have 2 colliders, so only work with non-duplicates
        if (hitObjects.Contains(col.gameObject))
        {
            return;
        }

        if (col.TryGetComponent(out ObjectOutline outline))
        {
            GameObject hitObject = col.gameObject;
            hitObjects.Add(hitObject);

            float outlineTime = 0f;
            float hapticTime = 0f;
            Vector2 hapticStrength = Vector2.zero;
            AudioManager.soundType sfx = default;

            float[] vals = null;
            if(Gamepad.current != null)
            {
                vals = FindLeftOrRightOfPlayer(col.gameObject.transform.position);
            }

            if (gameObject.CompareTag("LowPulse"))
            {
                outlineTime = lowPulseDuration;
                sfx = AudioManager.soundType.lowPulseHit;
                if(vals != null)
                {
                    hapticStrength = new Vector2(vals[0] * 0.2f, vals[1] * 0.7f);
                    hapticTime = 0.3f;
                }
                
            }
            else if (gameObject.CompareTag("HighPulse"))
            {
                outlineTime = highPulseDuration;
                sfx = AudioManager.soundType.highPulseHit;
                if(vals != null)
                {
                    hapticStrength = new Vector2(vals[0] * 0.3f, vals[1] * 1f);
                    hapticTime = 0.4f;
                }
               
            }
            else if (gameObject.CompareTag("EmitterPulse"))
            {
                outlineTime = emittingObjectPulseDuration;
                sfx = AudioManager.soundType.emitterPulseHit;
                if(vals != null)
                {
                    hapticStrength = new Vector2(vals[0] * 0.1f, vals[1] * 0.5f);
                    hapticTime = 0.2f;
                }

            }


            if (!outline.GetIsOutlined())
            {
                outline.SetCanBeTriggered(true);
                outline.SetOutlineTime(outlineTime);

                if (Gamepad.current != null && vals != null)
                {
                    HapticManager.instance.HapticFeedback(hapticStrength.x, hapticStrength.y, hapticTime);
                }
            }
            else
            {
                outline.SetCanBeTriggered(false);
            }

            AudioManager.instance.PlaySound(sfx, hitObject.transform.position, 0.2f);

        }


    }

    float[] FindLeftOrRightOfPlayer(Vector3 obj)
    {
        if(Gamepad.current != null)
        {
            PlayerMovement player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

            float[] output = new float[] { 0, 0 };

            Vector3 directionToPlayer = (obj - player.transform.position).normalized;
            float dotProd = Vector3.Dot(directionToPlayer, player.transform.right);

            if (dotProd > 0.1f)
            {
                //right
                output[0] = 0;
                output[1] = 1;

            }
            else if (dotProd < -0.1f)
            {
                //left
                output[0] = 1;
                output[1] = 0;

            }
            else
            {
                //same plane
                output[0] = 1;
                output[1] = 1;


            }

            return output;
        }
        else
        {
            return null;
        }
        

    }


}
