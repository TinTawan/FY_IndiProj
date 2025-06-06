using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOutline : MonoBehaviour
{
    bool isOutlined = false, canBeTriggered = false;

    private float outlineTime = 3f;
    [SerializeField] private Color outlineColour;
    Material outlineMat;

    ObjectEcholocationPulse objEchoPulse;

    Coroutine fadeCR;

    private void Start()
    {
        outlineMat = GetComponent<MeshRenderer>().materials[0];
        outlineMat.SetColor("_outlineColour", outlineColour);

        if(TryGetComponent(out ObjectEcholocationPulse oep))
        {
            objEchoPulse = oep;
        }
        else
        {
            objEchoPulse = null;
        }
        
    }

    private void Update()
    {
        if (canBeTriggered && !isOutlined)
        {
            canBeTriggered = false;

            fadeCR = StartCoroutine(FadeOutline(outlineMat));
        }
    }


    IEnumerator FadeOutline(Material mat)
    {
        isOutlined = true;

        if(objEchoPulse != null)
        {
            yield return new WaitForFixedUpdate();
            objEchoPulse.SetPlayOnce(false);
        }

        mat.SetFloat("_outlineDepth", 0.02f);

        yield return new WaitForSeconds(outlineTime);


        for (float i = 0.02f; i >= -0.001f; i -= 0.001f)
        {
            mat.SetFloat("_outlineDepth", i);
            yield return null;

        }

        isOutlined = false;

        if (objEchoPulse != null)
        {
            objEchoPulse.SetPlayOnce(true);
        }

        fadeCR = null;
    }

    public void SetOutlineTime(float inTime)
    {
        outlineTime = inTime;
    }

    public bool GetIsOutlined()
    {
        return isOutlined;
    }
    public void SetIsOutlined(bool inBool)
    {
        isOutlined = inBool;
    }

    public void SetCanBeTriggered(bool inBool)
    {
        canBeTriggered = inBool;
    }

    public void SetOutlineDepth(float depth)
    {
        outlineMat.SetFloat("_outlineDepth", depth);
    }

    public void StopFadeCR()
    {
        if(fadeCR != null)
        {
            StopCoroutine(fadeCR);

        }
    }

    public Color GetOutlineColour()
    {
        return outlineColour;
    }

    public void CallFadeOutCoroutine()
    {
        fadeCR = StartCoroutine(FadeOutline(outlineMat));
    }
    public Coroutine GetFadeOutCoroutine()
    {
        if (fadeCR == null) return null;

        return fadeCR;
    }
}
