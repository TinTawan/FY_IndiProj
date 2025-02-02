using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameUI : MonoBehaviour
{
    EcholocationPulse echoPulse;

    [Header("Pulse Cooldowns")]
    [SerializeField] Image HighFreqImage;
    [SerializeField] Image LowFreqImage;

    private void Start()
    {
        echoPulse = FindObjectOfType<EcholocationPulse>();

        HighFreqImage.fillAmount = 0;
        LowFreqImage.fillAmount = 0;

    }

    private void Update()
    {
        CooldownFillImages();
    }


    void CooldownFillImages()
    {
        float hFill = Mathf.InverseLerp(0, echoPulse.GetMaxHighCD(), echoPulse.GetHTimer());
        float lFill = Mathf.InverseLerp(0, echoPulse.GetMaxLowCD(), echoPulse.GetLTimer());

        HighFreqImage.fillAmount = hFill;
        LowFreqImage.fillAmount = lFill;

    }

}
