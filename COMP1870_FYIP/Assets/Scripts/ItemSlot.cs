using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] float colourCheckDelta = 0.01f, outlineDepth = 0.02f;
    Color objectiveColour;

    ItemInteract itemInteract;
    ObjectOutline itemOutline, slotOutline;

    private void Start()
    {
        objectiveColour = GetComponent<ObjectOutline>().GetOutlineColour();
        slotOutline = GetComponent<ObjectOutline>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("ObjectiveItem"))
        {
            GameObject item = col.gameObject;

            itemInteract = col.GetComponentInParent<ItemInteract>();
            itemOutline = col.GetComponent<ObjectOutline>();
            if (itemInteract != null && itemInteract.GetIsHolding())
            {
                if(CompareColours(objectiveColour, itemOutline.GetOutlineColour(), colourCheckDelta))
                {
                    //correct item to correct slot
                    Debug.Log("correct item inside");

                    //show slot has been filled
                    slotOutline.StopFadeCR();
                    slotOutline.TurnOnOutline(outlineDepth);
                    slotOutline.SetIsOutlined(true);

                    //remove item 
                    itemInteract.PlacedItem();
                    Destroy(item, 0.1f);

                }
                else
                {
                    //wrong item for this slot
                    Debug.Log("wrong item for this slot");

                    Debug.Log($"Item: R {itemOutline.GetOutlineColour().r}, G {itemOutline.GetOutlineColour().g}, B {itemOutline.GetOutlineColour().b}, A {itemOutline.GetOutlineColour().a}");
                    Debug.Log($"Slot: R {objectiveColour.r}, G {objectiveColour.g}, B {objectiveColour.b}, A {objectiveColour.a}");

                }

            }

        }
    }

    bool CompareColours(Color col1, Color col2, float delta)
    {
        if ((Mathf.Abs(col1.r - col2.r) < delta) && (Mathf.Abs(col1.g - col2.g) < delta) && (Mathf.Abs(col1.b - col2.b) < delta) && (Mathf.Abs(col1.a - col2.a) < delta))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
