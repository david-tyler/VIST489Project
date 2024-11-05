using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeBallTower : Interactable
{

    public GlyphPuzzleController controller;

    public Material nextColor;
    public bool foundBall = false;

    public GameObject ball;

  
    public void AddSelfToCurrentList()
    {
        controller.currentOrder.Add(this.gameObject);


        if(controller.currentOrder.Count >= 4)
        {
            controller.CheckOrder();
        }
    }


    public override void Interact()
    {
        //base.Interact();

       
    
        // Event triggered when object is hit by raycast
        Debug.Log("Object hit by raycast: " + gameObject.name);

        if(foundBall)
        {
            AddSelfToCurrentList();
            ball.SetActive(true);
            controller.coloredPokeball.material = nextColor;
        }
       
        // You can add custom logic here, like changing color or triggering an animation
    }
}