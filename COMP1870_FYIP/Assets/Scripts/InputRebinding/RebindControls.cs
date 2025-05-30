using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class RebindControls : MonoBehaviour
{
    [SerializeField] private InputActionReference inputActionReference; //this is on the SO
    [SerializeField] private bool excludeMouse = true;
    [Range(0,5)] [SerializeField] private int selectedBinding;
    [SerializeField] private InputBinding.DisplayStringOptions displayStringOptions;
    [Header("Binding Info - DO NOT Edit")] [SerializeField] private InputBinding inputBinding;
    
    private int bindingIndex;
    private int selectBinding;
    private string actionName;

    [Header("UI Fields")]
    [SerializeField] private TMP_Text actionText;
    [SerializeField] private Button rebindButton;
    [SerializeField] private GameObject rebindOverlay;
    [SerializeField] private TMP_Text rebindOverlayText;
    [SerializeField] private TMP_Text rebindText;
    [SerializeField] private Button resetButton;

    private void OnEnable()
    {
        InputManager.rebindComplete += UpdateUI;
        InputManager.rebindCanceled += UpdateUI;
    }

    private void OnDisable() 
    {
        InputManager.rebindComplete -= UpdateUI;
        InputManager.rebindCanceled -= UpdateUI;
    }

    private void Start()
    {
        rebindButton.onClick.AddListener(() => DoRebind());
        resetButton.onClick.AddListener(() => ResetBinding());

        if (inputActionReference != null)
        {
            GetBindingInfo(selectBinding);
            InputManager.Instance.LoadBindingOverride(actionName);
            UpdateUI();
        }

    }


    private void OnValidate() 
    {
        if (Application.isPlaying)
        {
            if(InputManager.Instance == null)
                return;
        }

        if(inputActionReference == null)
            return;

        GetBindingInfo(selectBinding);
        UpdateUI();
    }

    private void GetBindingInfo(int selectBinding)
    {

        if(inputActionReference.action != null)
            actionName = inputActionReference.action.name;

        if(inputActionReference.action.bindings.Count > selectedBinding)
        {
            inputBinding = inputActionReference.action.bindings[selectedBinding = selectBinding];
            bindingIndex = selectedBinding;
        }
    }

    public void UpdateUI()
    {
        /*if (actionName == null)
            //Debug.Log("action name null");
            return;*/
        if(actionText != null)
            actionText.text = actionName;

        if(rebindText != null)
        {
            if(Application.isPlaying)
            {
                rebindText.text = InputManager.Instance.GetBindingName(actionName, bindingIndex);
            }
            else
            {
                rebindText.text = inputActionReference.action.GetBindingDisplayString(bindingIndex);
            }
        }
    }

    private void DoRebind()
    {
        InputManager.StartRebind(actionName, bindingIndex, rebindText, rebindOverlayText,  rebindOverlay, excludeMouse);
    }

    public void ResetBinding()
    {
        InputManager.Instance.ResetBinding(actionName, bindingIndex);
        UpdateUI();
    }

    public void SelectKeyboardAndMouseBindingInfo()
    {
        GetBindingInfo(0); // Change number to K&M binding if not 0
        UpdateUI();
    }

    public void SelectControllerBindingInfo()
    {   
        GetBindingInfo(1); // Change number to Controller binding if not 1
        UpdateUI();
    }

    public void ContollerVectorBindingInfo()
    {
        GetBindingInfo(5); // Change number to controller move binding
        UpdateUI();
    }

}