using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class ItemInteract : MonoBehaviour
{
    [SerializeField] Transform itemSlot;

    PlayerInput playerInput;

    GameObject heldItem;

    Collider[] itemCols;

    bool canPickUp, holdingItem;


    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Enable();
        playerInput.Player.Interact.started += Interact_started;
    }

    private void Start()
    {
        canPickUp = false;
        holdingItem = false;
    }

    private void Interact_started(InputAction.CallbackContext ctx)
    {
        //player can pick up or drop item by interacting
        if (holdingItem)
        {
            Debug.Log("Dropped item");
            Dropitem();
            return;
        }
        if (canPickUp)
        {
            Debug.Log("Interacted with item");
            CarryItem(heldItem);
        }


    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("ObjectiveItem"))
        {
            canPickUp = true;
            heldItem = col.gameObject;

        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("ObjectiveItem"))
        {
            canPickUp = false;
            heldItem = null;

        }
    }

    void CarryItem(GameObject inHeldItem)
    {
        SetCols(false);

        inHeldItem.transform.SetPositionAndRotation(itemSlot.position, Quaternion.identity);
        inHeldItem.transform.SetParent(itemSlot);

        canPickUp = false;
        holdingItem = true;

    }

    void Dropitem()
    {
        SetCols(true);

        heldItem.transform.SetParent(null);
        heldItem = null;

        canPickUp = true;
        holdingItem = false;

    }

    void SetCols(bool areCollidersOn)
    {
        /*GameObject[] objs = GetComponentsInChildren<GameObject>();
        foreach(GameObject go in objs)
        {
            GameObject.findta
        }*/


        //so far this removes the players colliders
        itemCols = heldItem.GetComponents<Collider>();
        foreach (Collider col in itemCols)
        {
            col.enabled = areCollidersOn;
        }
    }


    private void OnDisable()
    {
        playerInput.Player.Disable();
    }
}
