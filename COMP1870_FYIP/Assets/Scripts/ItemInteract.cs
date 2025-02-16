using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class ItemInteract : MonoBehaviour
{
    PlayerInput playerInput;

    bool canPickUp;


    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Enable();
        playerInput.Player.Interact.performed += Interact_performed;
    }

    private void Interact_performed(InputAction.CallbackContext ctx)
    {

        if (canPickUp)
        {
            Debug.Log("Interacted with item");
        }

    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("ObjectiveItem"))
        {
            canPickUp = true;
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("ObjectiveItem"))
        {
            canPickUp = false;
        }
    }

}
