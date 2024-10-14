using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
    public GameObject parentObject;

    // variable used to store the item's model for later if we want to spawn it again;
    public GameObject itemModel;

    public override void Interact()
    {
        base.Interact();

        PickUp();
    }


    private void PickUp()
    {
        

        Debug.Log("Picking Up " + item.name);

        bool completedPickUp = Inventory.instance.Add(item);

        if (completedPickUp == true)
        {
            // store the model for later if we want to spawn it again;
            

            // if we have a model as a parent to the 3d object destroy that if not destroy just the 3d object
            if (parentObject != null)
            {
                itemModel = parentObject;
                Destroy(parentObject);

            }
            else
            {
                itemModel = gameObject;
                Destroy(gameObject);
            }
            
            
        }
        
    }

}
