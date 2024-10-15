using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Key Item", menuName = "Inventory/Key")]
public class KeyItem : Item
{
    DoorController doorScript;
    PokemonWorld pokeWorld;

    public override void Use()
    {
        base.Use();

        doorScript = DoorController.instance;
        pokeWorld = PokemonWorld.instance;

        doorScript.ToggleDoor();

        pokeWorld.SetUnlockedDoor(true);




    }
}
