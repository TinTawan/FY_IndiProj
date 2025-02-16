using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    int items = 0;
    bool collected;

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            /*if (col.TryGetComponent(out ItemInteract item))
            {
                if (item.GetDropped())
                {
                    items++;
                    Debug.Log($"Objective Items found: {items}");
                    Destroy(col.gameObject, 0.1f);
                }
                
            }
            if (col.GetComponent<ItemInteract>().GetDropped())
            {
                items++;
                Debug.Log($"Objective Items found: {items}");
                Destroy(col.gameObject, 0.1f);
            }*/

        }
    }

}
