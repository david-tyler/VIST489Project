using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;
    public GameObject parentObject;

    // variable used to store the item's model for later if we want to spawn it again;
    public GameObject itemModel;

    PokemonWorld pokeWorld;
    DoorController doorScript;

    GameSystemBehavior gameSystem;
    MessageBehavior messageBehavior;

    public AudioSource ItemPickupSound;
    public PokeBallTower tower;

    public string TryOpeningDoor = "Great you got the key! You should be able to unlock that door now to free Ash.";
    bool completedPickUp = false;

    public override void Interact()
    {
        base.Interact();

        PickUp();
    }


    private void PickUp()
    {

        pokeWorld = PokemonWorld.instance;
        gameSystem = GameSystemBehavior.instance;
        messageBehavior = MessageBehavior.instance;

        Debug.Log("Picking Up " + item.name);

        completedPickUp = Inventory.instance.Add(item);

        if (completedPickUp == true)
        {
            ItemPickupSound.PlayOneShot(ItemPickupSound.clip);
            // if we picked up the key set the bool that we have the key to true
            if (item.name == "Key")
            {
                pokeWorld.SetPickedUpKey(true);
                doorScript = DoorController.instance;
                doorScript.gotKey = true;

                messageBehavior.SetHaveMessage(true);
                messageBehavior.SetMessageText(TryOpeningDoor);
            }

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
        if (tower != null)
        {
            tower.foundBall = true;
        }
        
    }
    public void SetCompletedPickUp(bool status)
    {
        completedPickUp = status;
    }

    public bool GetCompletedPickUp()
    {
        return completedPickUp;
    }

}
