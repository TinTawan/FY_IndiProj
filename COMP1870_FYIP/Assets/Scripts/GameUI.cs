using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameUI : MonoBehaviour
{
    EcholocationPulse echoPulse;

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

        edgeGradientMat = edgeGradientRI.material;
        edgeGradientMat.SetColor("_edgeColour", highFreqCol);

    }

    private void Update()
    {
        LerpEdgeCol();
        EdgeCooldownLerp();
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
        //reset material attributes
        edgeGradientMat.SetColor("_edgeColour", highFreqCol);
        edgeGradientMat.SetFloat("_radius", edgeRadMax);

    }

}
