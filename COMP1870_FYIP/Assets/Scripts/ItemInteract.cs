using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class ItemInteract : MonoBehaviour
{
    [SerializeField] Transform itemSlot;
    [SerializeField] float outlineDepth = 0.02f;

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
            //Debug.Log("Dropped item");
            Dropitem();
            return;
        }
        if (canPickUp)
        {
            //Debug.Log("Interacted with item");
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
        //SetCols(false);
        SetCol(false, 0);

        inHeldItem.transform.SetPositionAndRotation(itemSlot.position, Quaternion.identity);
        inHeldItem.transform.SetParent(itemSlot);

        canPickUp = false;
        holdingItem = true;

        if (heldItem.TryGetComponent(out ObjectOutline outline))
        {
            outline.StopFadeCR();
            outline.TurnOnOutline(outlineDepth);
            outline.SetIsOutlined(true);

        }

    }

    void Dropitem()
    {
        SetCols(true);

        if (heldItem.TryGetComponent(out ObjectOutline outline))
        {
            outline.TurnOnOutline(0f);
            outline.SetCanBeTriggered(false);
            outline.SetIsOutlined(false);
        }

        heldItem.transform.SetParent(null);
        heldItem = null;

        canPickUp = true;
        holdingItem = false;

    }

    void SetCols(bool areCollidersOn)
    {
        //so far this removes the players colliders
        itemCols = heldItem.GetComponents<Collider>();
        foreach (Collider col in itemCols)
        {
            col.enabled = areCollidersOn;
        }
    }
    void SetCol(bool isColOn, int colIndex)
    {
        itemCols = heldItem.GetComponents<Collider>();
        itemCols[colIndex].enabled = isColOn;
    }

    public bool GetIsHolding()
    {
        return holdingItem;
    }


    private void OnDisable()
    {
        playerInput.Player.Disable();
    }
}
