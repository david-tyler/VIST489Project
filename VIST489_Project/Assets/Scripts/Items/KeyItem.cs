using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Key Item", menuName = "Inventory/Key")]
public class KeyItem : Item
{
    DoorController doorScript;
    public override void Use()
    {
        base.Use();
        doorScript = DoorController.instance;
        doorScript.ToggleDoor();


    }
}
