using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameUI : MonoBehaviour
{
    EcholocationPulse echoPulse;

    [Header("Pulse Cooldowns")]
    [SerializeField] Image highFreqFillImage;
    [SerializeField] Image lowFreqFillImage;
    [SerializeField] Image highCover, lowCover;

    [Header("Edge Gradient info")]
    [SerializeField] RawImage edgeGradientRI;
    [SerializeField] Color highFreqCol;
    [SerializeField] Color lowFreqCol;
    //min val for the radius that is furthest off the screen, max for closest towards middle
    [SerializeField] float edgeRadMin = 0.75f, edgeRadMax = 0.5f;

    Material edgeGradientMat;

    [SerializeField] float colLerpSpeed = 2f;
    Color currentEdgeCol;

    private void Start()
    {
        echoPulse = FindObjectOfType<EcholocationPulse>();

        highFreqFillImage.fillAmount = 0;
        lowFreqFillImage.fillAmount = 0;

        highCover.enabled = false;
        lowCover.enabled = true;

        edgeGradientMat = edgeGradientRI.material;
        edgeGradientMat.SetColor("_edgeColour", highFreqCol);


    }

    private void Update()
    {
        //CooldownFillImages();
        //SetEdgeCol();

        LerpEdgeCol();
        EdgeCooldownLerp();
    }


    void CooldownFillImages()
    {
        float hFill = Mathf.InverseLerp(0, echoPulse.GetMaxHighCD(), echoPulse.GetHTimer());
        float lFill = Mathf.InverseLerp(0, echoPulse.GetMaxLowCD(), echoPulse.GetLTimer());

        highFreqFillImage.fillAmount = hFill;
        lowFreqFillImage.fillAmount = lFill;

        int currentPulse = echoPulse.GetCurrentPulse();

        if (currentPulse == 1)
        {
            highCover.enabled = false;
            lowCover.enabled = true;


        }
        if (currentPulse == 0)
        {
            highCover.enabled = true;
            lowCover.enabled = false;

        }
    }

    void SetEdgeCol()
    {
        switch (echoPulse.GetCurrentPulse())
        {
            case 0:
                edgeGradientMat.SetColor("_edgeColour", lowFreqCol);

                break;
            case 1:
                edgeGradientMat.SetColor("_edgeColour", highFreqCol);

                break;
        }
    }

    void LerpEdgeCol()
    {
        Color targetCol;

        if(echoPulse.GetCurrentPulse() == 0)
        {
            targetCol = lowFreqCol;
        }
        else
        {
            targetCol = highFreqCol;

        }

        currentEdgeCol = Color.Lerp(currentEdgeCol, targetCol, colLerpSpeed * Time.deltaTime);

        edgeGradientMat.SetColor("_edgeColour", currentEdgeCol);


    }

    void EdgeCooldownLerp()
    {

        float hRad = Mathf.Lerp(edgeRadMax, edgeRadMin, echoPulse.GetHTimer());
        float lRad = Mathf.Lerp(edgeRadMax, edgeRadMin, echoPulse.GetLTimer());

        if (echoPulse.GetCurrentPulse() == 0) 
        {
            edgeGradientMat.SetFloat("_radius", lRad);

        }
        else
        {
            edgeGradientMat.SetFloat("_radius", hRad);

        }

    }



    private void OnDisable()
    {
        edgeGradientMat.SetColor("_edgeColour", highFreqCol);

    }

}
