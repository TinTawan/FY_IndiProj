using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameUI : MonoBehaviour
{
    EcholocationPulse echoPulse;

    [Header("Pulse Cooldowns")]
    [SerializeField] Image HighFreqFillImage;
    [SerializeField] Image LowFreqFillImage;

    private void Start()
    {
        echoPulse = FindObjectOfType<EcholocationPulse>();

        HighFreqFillImage.fillAmount = 0;
        LowFreqFillImage.fillAmount = 0;

    }

    private void Update()
    {
        CooldownFillImages();
    }


    void CooldownFillImages()
    {
        float hFill = Mathf.InverseLerp(0, echoPulse.GetMaxHighCD(), echoPulse.GetHTimer());
        float lFill = Mathf.InverseLerp(0, echoPulse.GetMaxLowCD(), echoPulse.GetLTimer());

        HighFreqFillImage.fillAmount = hFill;
        LowFreqFillImage.fillAmount = lFill;

    }

}
