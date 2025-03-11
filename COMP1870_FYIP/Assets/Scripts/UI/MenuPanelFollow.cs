using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuPanelFollow : MonoBehaviour
{
    RectTransform rectTransform;

    float startSize;
    bool clicked = false;
    [SerializeField] float minBreathSize, maxBreathSize, maxExpandSize, scaleSpeed = 1f, scaleSpeedMult = 3f;

    PlayerInput playerInput;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        startSize = rectTransform.localScale.x;

        playerInput = new PlayerInput();
        playerInput.UI.Enable();

        if (Gamepad.current == null)
        {
            playerInput.UI.Point.performed += Point_performed;
            playerInput.UI.Click.performed += Click_performed;

        }
        else
        {
            rectTransform.position = EventSystem.current.currentSelectedGameObject.transform.position;
        }
    }

    private void Click_performed(InputAction.CallbackContext ctx)
    {
        if (!clicked)
        {
            clicked = true;
            StartCoroutine(ExpandAndReturn());
        }

    }

    private void Point_performed(InputAction.CallbackContext ctx)
    {
        rectTransform.position = ctx.ReadValue<Vector2>();
    }

    private void Update()
    {
        ScaleWithSin();

    }

    void ScaleWithSin()
    {
        if (!clicked)
        {
            float sinVal = Mathf.Sin(scaleSpeed * Time.time);
            float scale = Remap(sinVal, -1, 1, minBreathSize, maxBreathSize);
            rectTransform.localScale = new(scale, scale, scale);
        }

    }

    void ScaleRect(float addVal)
    {
        rectTransform.localScale = new(rectTransform.localScale.x + addVal, rectTransform.localScale.y + addVal, rectTransform.localScale.z + addVal);

    }

    IEnumerator ExpandAndReturn()
    {
        while (rectTransform.localScale.x < maxExpandSize)
        {
            ScaleRect(scaleSpeed * scaleSpeedMult);
            yield return null;
        }

        while (rectTransform.localScale.x > maxBreathSize && clicked)
        {
            ScaleRect(-scaleSpeed * scaleSpeedMult * 1.5f);
            yield return null;
        }

        //yield return new WaitForSeconds(0.5f);

        clicked = false;
    }

    float Remap(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    private void OnDisable()
    {
        playerInput.UI.Disable();
    }
}
