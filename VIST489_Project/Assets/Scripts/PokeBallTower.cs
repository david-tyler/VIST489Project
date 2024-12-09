using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeBallTower : Interactable
{

    public GlyphPuzzleController controller;
    public Camera camera;

    public Material nextColor;
    public bool foundBall = false;

    public GameObject ball;

    public AudioSource placePokeballAudio;

    public bool placed = false;


    public void AddSelfToCurrentList()
    {
        controller.currentOrder.Add(this.gameObject);


        if (controller.currentOrder.Count >= 4)
        {
            controller.CheckOrder();
        }
    }


    public override void Interact()
    {
        //base.Interact();



        // Event triggered when object is hit by raycast
        Debug.Log("Object hit by raycast: " + gameObject.name);

        if (foundBall)
        {
            if(!placed)
            {
                placePokeballAudio.PlayOneShot(placePokeballAudio.clip);
                controller.placedBalls.Add(ball);
                placed = true;
                AddSelfToCurrentList();
            }
            


            
            ball.SetActive(true);
            
            controller.coloredPokeball.material = nextColor;
        }

        // You can add custom logic here, like changing color or triggering an animation
    }

    public void Update()
    {

        if (Input.touchCount > 0)
        {
             Debug.Log("TOUCHING SCREEN");
            Touch touch = Input.GetTouch(0);

            if (touch.phase != TouchPhase.Ended)
            {
                // Debug.Log("TOUCHING Begin");
                Debug.Log("TOUCHING SCREEN 2");
                // Create a ray from the screen point where the touch occurred.
                Ray ray = camera.ScreenPointToRay(touch.position);

                // Variable to store the hit information.
                RaycastHit hit;

                // Perform the raycast.
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject == this.gameObject)
                    {
                        Debug.Log("TOUCHING tower");
                        Interact();
                    }
                }
            }
            else
            {

                controller.IncorrectOrder();
            }
        }
        else if (Input.GetMouseButton(0)) // 0 is for left-click
        {
            Debug.Log("TOUCHING SCREEN");
            // Create a ray from the screen point where the touch occurred.
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            Debug.Log("TOUCHING SCREEN 2");
            // Variable to store the hit information.
            RaycastHit hit;

            // Perform the raycast.
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("raycast hit " + hit.collider.gameObject.name);

                if (hit.collider.gameObject == this.gameObject)
                {
                    Debug.Log("TOUCHING tower");
                    Interact();
                }
            }
        }
        else
        {
            controller.IncorrectOrder();
        }

        if(!ball.activeInHierarchy)
        {
            placed = false;
        }
    }
}
