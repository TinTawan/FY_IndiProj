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

        }

    }

    private void OnTriggerEnter(Collider col)
    {
        Renderer rend = col.GetComponent<Renderer>();

        if (col.TryGetComponent(out ObjectOutline outline)/* && rend.isVisible*/)
        {
            float[] vals = FindLeftOrRightOfPlayer(col.gameObject.transform.position);


            if (!outline.GetIsOutlined())
            {
                //outline the object
                outline.SetCanBeTriggered(true);

                //give lower outline time if hit by low pulse
                if (gameObject.CompareTag("LowPulse"))
                {
                    outline.SetOutlineTime(lowPulseDuration);

                    HapticManager.instance.HapticFeedback(vals[0] * 0.2f, vals[1] * 0.7f, 0.3f);

                    AudioManager.instance.PlaySound(AudioManager.soundType.lowPulseHit, col.transform.position, 0.2f);

                }
                //and longer outline time if hit by high pulse
                if (gameObject.CompareTag("HighPulse"))
                {
                    outline.SetOutlineTime(highPulseDuration);

                    HapticManager.instance.HapticFeedback(vals[0] * 0.3f, vals[1] * 1f, 0.4f);

                    AudioManager.instance.PlaySound(AudioManager.soundType.highPulseHit, col.transform.position, 0.2f);

                }
                //and middle time if hit by emitting object
                if (gameObject.CompareTag("EmitterPulse"))
                {
                    outline.SetOutlineTime(emittingObjectPulseDuration);

                    HapticManager.instance.HapticFeedback(vals[0] * 0.1f, vals[1] * 0.5f, 0.2f);

                    AudioManager.instance.PlaySound(AudioManager.soundType.emitterPulseHit, col.transform.position, 0.2f);

                }

            }
            else
            {
                outline.SetCanBeTriggered(false);
            }
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

    float DistanceBetweenObjects(Vector3 obj1, Vector3 obj2)
    {
        return Vector3.Distance(obj1, obj2);
    }

}
