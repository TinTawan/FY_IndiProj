using UnityEngine;
using UnityEngine.UI;

public class ResetAllAssignButton : MonoBehaviour
{
    Button resetButton;

    private void OnEnable()
    {
        resetButton = GetComponent<Button>();

        //resetButton.onClick.RemoveAllListeners();
        resetButton.onClick.AddListener(() => { OnClick(); });
    }

    private void OnClick()
    {
        if (InputManager.Instance.gameObject.TryGetComponent(out ResetAllBindings reset))
        {
            //Debug.Log("Reset all");
            //Debug.Log(reset.gameObject.name);
            reset.ResetBindings();
        }

    }

    private void OnDisable()
    {
        resetButton.onClick.RemoveAllListeners();
    }
}
