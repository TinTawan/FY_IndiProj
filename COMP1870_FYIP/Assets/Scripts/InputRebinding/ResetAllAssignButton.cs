using UnityEngine;
using UnityEngine.UI;

public class ResetAllAssignButton : MonoBehaviour
{
    Button resetButton;

    private void OnEnable()
    {
        resetButton = GetComponent<Button>();

        resetButton.onClick.AddListener(() => { OnClick(); });
    }

    private void OnClick()
    {
        if (InputManager.Instance.gameObject.TryGetComponent(out ResetAllBindings reset))
        {
            reset.ResetBindings();
        }

    }

    private void OnDisable()
    {
        resetButton.onClick.RemoveAllListeners();
    }
}
