using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class ItemInteract : MonoBehaviour
{
    [SerializeField] Transform itemSlot;
    [SerializeField] float outlineDepth = 0.02f, interactOutlineDepth = 0.005f;

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
            AudioManager.instance.PlaySound(AudioManager.soundType.itemDrop, transform.position, 0.2f);

            Dropitem();
            return;
        }
        if (canPickUp)
        {
            AudioManager.instance.PlaySound(AudioManager.soundType.itemPickUp, transform.position, 0.3f);
            if(heldItem != null)
            {
                CarryItem(heldItem);
            }
        }


    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("ObjectiveItem") && !holdingItem)
        {
            AudioManager.instance.PlaySound(AudioManager.soundType.canInteract, transform.position, 0.3f);
            HapticManager.instance.HapticFeedback(.1f, .3f, .2f);

            itemCols = col.GetComponents<Collider>();

        }
    }
    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("ObjectiveItem") && !holdingItem)
        {
            canPickUp = true;
            heldItem = col.gameObject;

            if (col.TryGetComponent(out ObjectOutline outline))
            {
                if (!outline.GetIsOutlined())
                {
                    outline.SetIsOutlined(true);
                    outline.SetOutlineDepth(interactOutlineDepth);
                }
                
            }

        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("ObjectiveItem"))
        {

            if (col.TryGetComponent(out ObjectOutline outline))
            {
                if(outline.GetFadeOutCoroutine() == null && !holdingItem)
                {
                    outline.SetIsOutlined(false);
                    outline.SetOutlineDepth(0f);
                }
                
            }

            canPickUp = false;
        }
    }

    void CarryItem(GameObject inHeldItem)
    {
        SetCol(false, 1);


        inHeldItem.transform.SetPositionAndRotation(itemSlot.position, Quaternion.identity);
        inHeldItem.transform.SetParent(itemSlot);

        canPickUp = false;
        holdingItem = true;

        if (heldItem.TryGetComponent(out ObjectOutline outline))
        {
            outline.StopFadeCR();
            outline.SetOutlineDepth(outlineDepth);
            outline.SetIsOutlined(true);

        }

    }

    void Dropitem()
    {
        SetCols(true);

        if (heldItem.TryGetComponent(out ObjectOutline outline))
        {
            outline.SetOutlineTime(3f);
            outline.SetCanBeTriggered(false);
            outline.CallFadeOutCoroutine();
        }

        heldItem.transform.SetParent(null);
        heldItem = null;

        canPickUp = true;
        holdingItem = false;

        itemCols = null;

    }

    void SetCols(bool areCollidersOn)
    {
        //so far this removes the players colliders
        if(itemCols != null && itemCols.Length > 0)
        {
            itemCols = heldItem.GetComponents<Collider>();
            foreach (Collider col in itemCols)
            {
                col.enabled = areCollidersOn;
            }
        }
        
    }
    void SetCol(bool isColOn, int colIndex)
    {
        if (itemCols != null && itemCols.Length > 0)
        {
            itemCols = heldItem.GetComponents<Collider>();
            itemCols[colIndex].enabled = isColOn;

        }
    }

    public bool GetIsHolding()
    {
        return holdingItem;
    }
    public void PlacedItem()
    {
        Dropitem();
    }


    private void OnDisable()
    {
        playerInput.Player.Disable();
    }
}
