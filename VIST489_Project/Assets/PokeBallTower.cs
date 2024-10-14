using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeBallTower : MonoBehaviour, IRaycastHitHandler
{

    public GlyphPuzzleController controller;

    public Material nextColor;

  
    public void AddSelfToCurrentList()
    {
        controller.currentOrder.Add(this.gameObject);


        if(controller.currentOrder.Count >= 4)
        {
            controller.CheckOrder();
        }
    }


    public void OnRaycastHit()
    {
        // Event triggered when object is hit by raycast
        Debug.Log("Object hit by raycast: " + gameObject.name);


        controller.coloredPokeball.material = nextColor;
        // You can add custom logic here, like changing color or triggering an animation
    }
}
