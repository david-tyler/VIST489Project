using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBehaviorUI : MonoBehaviour
{
    public GameObject ItemsParent;
    private bool toggle = false;
    Inventory inventory;

    InventorySlot[] slots;

    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI; // so we call UpdateUI everytime an item is changed

        slots = ItemsParent.GetComponentsInChildren<InventorySlot>();
    }

    void Update()
    {
        
    }

    void UpdateUI()
    {

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }


    public void ToggleInventory()
    {
        if (toggle == true)
        {
            toggle = false;
        }
        else if (toggle == false)
        {
            toggle = true;
        }
        ItemsParent.SetActive(toggle);
    }

}
