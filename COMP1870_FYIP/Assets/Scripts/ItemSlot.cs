using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    PlayerInput playerInput;
    ItemInteract itemInteract;

    bool canPlace;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Enable();
        playerInput.Player.Interact.started += Interact_started;
    }

    private void Interact_started(InputAction.CallbackContext ctx)
    {
        if (canPlace)
        {
            itemInteract.Dropitem();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            canPlace = true;
            itemInteract = col.GetComponent<ItemInteract>();
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            canPlace = false;
        }
    }

    private void OnDisable()
    {
        playerInput.Player.Disable();
    }
}
