using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlyphRaycaster : MonoBehaviour
{
    Camera mainCamera;
    void Update()
    {


       

        // *******Touch Interactions
        // --------------------------------------
        // Check if there is at least one touch.
        if (Input.touchCount > 0)
        {
            // Debug.Log("TOUCHING SCREEN");
            Touch touch = Input.GetTouch(0);

            // Check if the touch is just beginning.
            if (touch.phase == TouchPhase.Began)
            {
                // Debug.Log("TOUCHING Begin");

                // Create a ray from the screen point where the touch occurred.
                Ray ray = mainCamera.ScreenPointToRay(touch.position);

                // Variable to store the hit information.
                RaycastHit hit;

       

                if (Physics.Raycast(ray, out hit))
                {
                    // Try to find the IRaycastHitHandler interface on the hit object
                    IRaycastHitHandler hitHandler = hit.collider.GetComponent<IRaycastHitHandler>();

                    // If the object has the interface, fire the event
                    if (hitHandler != null)
                    {
                        hitHandler.OnRaycastHit();
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(0)) // 0 is for left-click
        {
            // Debug.Log("TOUCHING Begin");

            // Create a ray from the screen point where the touch occurred.
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // Variable to store the hit information.
            RaycastHit hit;

            // Perform the raycast.
            if (Physics.Raycast(ray, out hit))
            {
                // Try to find the IRaycastHitHandler interface on the hit object
                IRaycastHitHandler hitHandler = hit.collider.GetComponent<IRaycastHitHandler>();

                // If the object has the interface, fire the event
                if (hitHandler != null)
                {
                    hitHandler.OnRaycastHit();
                }
            }

        }
    }
}
