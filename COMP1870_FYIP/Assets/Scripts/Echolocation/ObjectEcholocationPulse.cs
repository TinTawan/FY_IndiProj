using UnityEngine;

public class ObjectEcholocationPulse : MonoBehaviour
{
    [Header("Burst info")]
    [SerializeField] int burstCount;
    [SerializeField] [Range(0.2f, 0.6f)] float burstInterval = 0.4f;

    [Header("Particle System info")]
    [SerializeField] float fullDuration;
    [SerializeField] float lifetime, size;

    Color pulseColour;

    ParticleSystem ps;
    ParticleSystem.MainModule psMain;
    ParticleSystem.ColorOverLifetimeModule psCol;
    ParticleSystem.EmissionModule psEm;
    float burstTime = 0f;

    ObjectOutline objOutline;
    bool playOnce = true;

    private void Awake()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        psMain = ps.main;
        psCol = ps.colorOverLifetime;
        psEm = ps.emission;

        
        objOutline = GetComponent<ObjectOutline>();
        pulseColour = objOutline.GetOutlineColour();

        SetPS();
        SetBurst();
    }

    private void Update()
    {
        if (objOutline.GetIsOutlined() && playOnce)
        {
            ps.Play();
            AudioManager.instance.PlaySound(AudioManager.soundType.emitterPulseOut, transform.position, 0.2f);

        }

    }

    void SetPS()
    {
        //set the colour of the system
        psMain.startColor = pulseColour;

        //set the colour over lifetime module colour using a gradient
        Gradient grad = new Gradient();
        grad.SetKeys(
            new GradientColorKey[] { new GradientColorKey(pulseColour, 0.0f), new GradientColorKey(pulseColour, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 0.95f) }
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

    public void SetPlayOnce(bool inBool)
    {
        playOnce = inBool;
    }


}
