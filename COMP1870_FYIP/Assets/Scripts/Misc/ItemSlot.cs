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
                    AudioManager.instance.PlaySound(AudioManager.soundType.itemPlaced, transform.position, 0.3f);
                    HapticManager.instance.HapticFeedback(0.1f, 0.3f, 0.5f);
                    GameManager.instance.IncrementObjects();

                    //show slot has been filled
                    slotOutline.StopFadeCR();
                    slotOutline.SetOutlineDepth(outlineDepth);
                    slotOutline.SetIsOutlined(true);

                    //remove item 
                    itemInteract.PlacedItem();
                    Destroy(item, 0.1f);

                }
                else
                {
                    //wrong item for this slot
                    AudioManager.instance.PlaySound(AudioManager.soundType.wrongItemPlaced, transform.position, 0.5f);
                    HapticManager.instance.HapticFeedback(0.2f, 0.4f, 0.2f);

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
