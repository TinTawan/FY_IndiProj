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

    private void Start()
    {
        echoPulse = FindObjectOfType<EcholocationPulse>();

        highFreqFillImage.fillAmount = 0;
        lowFreqFillImage.fillAmount = 0;

        highCover.enabled = false;
        lowCover.enabled = true;

    }

    private void Update()
    {
        CooldownFillImages();
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

}
