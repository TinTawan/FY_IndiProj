using System.Collections;
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
        yield return new WaitForSeconds(onTime);

        while (pLight.range > 0)
        {

            pLight.range -= subVal;
            if (pLight.intensity > 0)
            {
                pLight.intensity -= subVal / 2;
            }

            yield return null;

        }


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
