using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuPanelFollow : MonoBehaviour
{
    RectTransform rectTransform;

    bool clicked = false;
    [SerializeField] float minBreathSize, maxBreathSize, maxExpandSize, scaleSpeed = 1f, scaleSpeedMult = 3f, smoothMoveSpeed = 5f;

    PlayerInput playerInput;

    Vector3 val = Vector3.zero, mousePos;

    Coroutine expandRoutine;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        playerInput = new PlayerInput();
        playerInput.UI.Enable();
        playerInput.UI.Submit.performed += Submit_performed;

        if (Gamepad.current == null)
        {
            playerInput.UI.Point.performed += Point_performed;
            playerInput.UI.Click.performed += Click_performed;

        }
        else
        {
            rectTransform.position = EventSystem.current.firstSelectedGameObject.transform.position;
        }
    }

    private void Submit_performed(InputAction.CallbackContext ctx)
    {
        if (!clicked)
        {
            clicked = true;
            expandRoutine = StartCoroutine(ExpandAndReturn());
        }
    }

    private void Click_performed(InputAction.CallbackContext ctx)
    {
        if (!clicked)
        {
            clicked = true;
            expandRoutine = StartCoroutine(ExpandAndReturn());
        }

    }

    private void Point_performed(InputAction.CallbackContext ctx)
    {
        mousePos = ctx.ReadValue<Vector2>();
    }

    private void Update()
    {
        ScaleWithSin();

        if(Gamepad.current == null)
        {
            rectTransform.position = Vector3.SmoothDamp(rectTransform.position, mousePos, ref val, Time.deltaTime * smoothMoveSpeed);
        }
        else
        {
            //if selected object has the slider component, move to its handle rather than to the slider
            if (EventSystem.current.currentSelectedGameObject.TryGetComponent(out Slider slider))
            {
                rectTransform.position = Vector3.SmoothDamp(rectTransform.position, slider.handleRect.transform.position, ref val, Time.deltaTime * smoothMoveSpeed);
            }
            else
            {
                rectTransform.position = Vector3.SmoothDamp(rectTransform.position, EventSystem.current.currentSelectedGameObject.transform.position, ref val, Time.deltaTime * smoothMoveSpeed);

            }
        }
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

        yield return new WaitForSeconds(0.5f);

        while (rectTransform.localScale.x > maxBreathSize && clicked)
        {
            ScaleRect(-scaleSpeed * scaleSpeedMult * 3f);
            yield return null;
        }

        clicked = false;
    }

    float Remap(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }


    private void OnDisable()
    {
        expandRoutine = null;
        playerInput.UI.Disable();
    }
}
