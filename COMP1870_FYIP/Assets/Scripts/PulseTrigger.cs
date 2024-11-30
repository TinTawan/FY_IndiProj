using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseTrigger : MonoBehaviour
{
    private SphereCollider sphereCol;
    private ParticleSystem ps;

    private Material outlineMat;

    [SerializeField] private float outlineTime = 3f;



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


        if(other.TryGetComponent(out MeshRenderer mr))
        {
            outlineMat = mr.materials[1];
            StartCoroutine(FadeOutline(outlineMat));

            Debug.Log(outlineMat);

            

        }
        else
        {
            Debug.Log("no material");

        }


    }

    IEnumerator FadeOutline(Material mat)
    {

        mat.SetFloat("_outlineDepth", 0.02f);

        yield return new WaitForSeconds(outlineTime);


        for(float i = 0.02f; i >= -0.001f; i -= 0.001f)
        {
            mat.SetFloat("_outlineDepth", i);
            yield return null;
            //yield return new WaitForSeconds(0.1f);

        }


    }

}
