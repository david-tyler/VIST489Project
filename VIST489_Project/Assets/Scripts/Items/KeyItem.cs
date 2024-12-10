using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Key Item", menuName = "Inventory/Key")]
public class KeyItem : Item
{
    DoorController doorScript;
    PokemonWorld pokeWorld;
    GameSystemBehavior gameSystem;

    public override void Use()
    {
        base.Use();

        doorScript = DoorController.instance;
        pokeWorld = PokemonWorld.instance;
        if (doorScript.GetPlayerInZone() == true && pokeWorld.GetUnlockedDoor() == false)
        {
            doorScript.ToggleDoor();

            pokeWorld.SetUnlockedDoor(true);
        }
        

    }
    public override void CollectedItem()
    {
        base.CollectedItem();
        gameSystem = GameSystemBehavior.instance;
        gameSystem.SetNarrativeState(GameSystemBehavior.NarrativeEvent.FreedAsh);

    }

}
