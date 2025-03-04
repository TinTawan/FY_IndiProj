using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPulse : MonoBehaviour
{
    Light pLight;

    float fadeSpeed = 0.1f, lifetime = 8f;


    private void Start()
    {
        pLight = GetComponent<Light>();

        StartCoroutine(FadeLight(lifetime, fadeSpeed));


    }


    IEnumerator FadeLight(float onTime, float subVal)
    {
        Debug.Log("Fade out soon");
        yield return new WaitForSeconds(onTime);
        Debug.Log("Fade out now");

        while (pLight.range > 0)
        {
            Debug.Log("Fading");

            pLight.range -= subVal;
            if(pLight.intensity > 0)
            {
                pLight.intensity -= subVal / 2;
            }

            yield return null;

        }

        Debug.Log("Faded out");

        Destroy(pLight.gameObject, 1f);
    }

    public void SetFadeSpeed(float inVal)
    {
        fadeSpeed = inVal;
    }
    public void SetLifetime(float inVal)
    {
        lifetime = inVal;
    }
}
