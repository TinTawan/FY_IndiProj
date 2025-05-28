using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ResetAllBindings : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset inputActions;

    //[SerializeField] List<RebindControls> allRebinds = new List<RebindControls>();
    RebindControls[] allRebinds;
    ScrollRect scroll;

    public void ResetBindings()
    {
        /*foreach (InputActionMap map in inputActions.actionMaps)
        {
            map.RemoveAllBindingOverrides();

        }*/

        if(scroll == null || allRebinds == null)
        {
            scroll = Resources.FindObjectsOfTypeAll<ScrollRect>()[0];
            allRebinds = scroll.GetComponentsInChildren<RebindControls>();  
        }

        foreach (RebindControls r in allRebinds)
        {
            Debug.Log($"{r.name} ResetBinding");
            r.ResetBinding();
        }


        PlayerPrefs.DeleteKey("rebinds");

        //allRebinds = null;
    }
}