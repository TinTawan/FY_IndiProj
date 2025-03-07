using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] float colourDelta = 0.01f;
    Color objectiveColour;

    ItemInteract item;
    ObjectOutline itemOutline;

    private void Start()
    {
        objectiveColour = GetComponent<ObjectOutline>().GetOutlineColour();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("ObjectiveItem"))
        {
            item = col.GetComponentInParent<ItemInteract>();
            itemOutline = col.GetComponent<ObjectOutline>();
            if (item.GetIsHolding())
            {
                Debug.Log("item inside");

                if(CompareColours(objectiveColour, itemOutline.GetOutlineColour(), colourDelta))
                {
                    Debug.Log($"Slot Colour = {objectiveColour}");
                    Debug.Log($"item Colour = {itemOutline.GetOutlineColour()}");

                }
                else
                {
                    Debug.Log("not same colour");

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
