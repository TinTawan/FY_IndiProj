using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchBindingInfo : MonoBehaviour
{
    RebindControls[] rebinds;


    private void Start()
    {
        rebinds = GetComponentsInChildren<RebindControls>();

        StartCoroutine(ShowBindsOnStart());
    }


    public void SetKeyboardBinds()
    {
        foreach(RebindControls r in rebinds)
        {
            r.SelectKeyboardAndMouseBindingInfo();
        }
    }

    public void SetGamepadBinds()
    {
        for(int i = 0; i < rebinds.Length; i++)
        {
            if(i == 0)
            {
                rebinds[i].ContollerVectorBindingInfo();
            }
            else
            {
                rebinds[i].SelectControllerBindingInfo();
            }
        }
    }

    IEnumerator ShowBindsOnStart()
    {
        yield return new WaitForEndOfFrame();

        if (Gamepad.current == null)
        {
            SetKeyboardBinds();
        }
        else
        {
            SetGamepadBinds();
        }
    }
}
