using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ResetAllBindings : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset inputActions;

    RebindControls[] allRebinds;
    ScrollRect scroll;

    public void ResetBindings()
    {
        if (scroll == null || allRebinds == null)
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

    }
}