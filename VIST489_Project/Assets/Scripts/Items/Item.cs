using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public Sprite image;
    new public string name = "New Item";
    private bool inventoryStatus = false; // If the item is in the inventory or not

    public virtual void Use()
    {
        // Use the item

        Debug.Log("Using " + name);
    }

    public virtual void CollectedItem()
    {
        Debug.Log("Collected " + name);
    }

    public void RemoveFromInventory()
    {
        if (inventoryStatus == true)
        {
            // we can remove
        }
        else if (inventoryStatus == false)
        {
            // we can't remove since it's not in the inventory
        }

    }

    public void SetInventoryStatus(bool status)
    {
        inventoryStatus = status;
    }
    public bool GetInventoryStatus()
    {
        return inventoryStatus;
    }
}
