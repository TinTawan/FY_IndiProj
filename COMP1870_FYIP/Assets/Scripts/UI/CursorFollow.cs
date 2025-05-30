using UnityEngine;
using UnityEngine.InputSystem;

public class CursorFollow : MonoBehaviour
{
    PlayerInput playerInput;

    private void Start()
    {
        //playerInput = InputManager.Instance.playerInput;
        playerInput = new PlayerInput();
        playerInput.UI.Enable();

        playerInput.UI.Point.performed += Point_performed;
    }

    private void Point_performed(InputAction.CallbackContext ctx)
    {
        if(this != null)
        {
            if (Gamepad.current == null)
            {
                transform.position = ctx.ReadValue<Vector2>();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        
    }

    private void OnDestroy()
    {
        playerInput.UI.Disable();
    }
}
