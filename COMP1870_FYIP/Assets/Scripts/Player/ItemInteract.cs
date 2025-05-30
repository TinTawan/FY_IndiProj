using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Rendering;

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
        playerInput = InputManager.Instance.playerInput;
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
            if(heldItem != null)
            {
                AudioManager.instance.PlaySound(AudioManager.soundType.itemPickUp, transform.position, 0.3f);
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

            itemCols = null;

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

        Material itemMat = inHeldItem.GetComponent<MeshRenderer>().material;
        itemMat.SetInt("_materialZTestMode", (int)CompareFunction.Always);
        itemMat.SetInt("_outlineZTestMode", (int)CompareFunction.Always);
        itemMat.renderQueue = (int)RenderQueue.Geometry;

        StartCoroutine(ForceDepthReset(inHeldItem));

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

        Material itemMat = heldItem.GetComponent<MeshRenderer>().material;
        itemMat.SetInt("_materialZTestMode", (int)CompareFunction.LessEqual);
        itemMat.SetInt("_outlineZTestMode", (int)CompareFunction.Less);
        itemMat.renderQueue = (int)RenderQueue.Overlay;

        StartCoroutine(ForceDepthReset(heldItem));

        heldItem.transform.SetParent(null);
        heldItem = null;

        canPickUp = true;
        holdingItem = false;


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

    IEnumerator ForceDepthReset(GameObject go)
    {
        go.GetComponent<MeshRenderer>().enabled = false;
        yield return null;
        go.GetComponent<MeshRenderer>().enabled = true;
    }

    private void OnDisable()
    {
        playerInput.Player.Disable();
    }
}
