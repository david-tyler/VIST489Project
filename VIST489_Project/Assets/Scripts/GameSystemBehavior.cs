using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class GameSystemBehavior : MonoBehaviour
{
    public GameObject pkWorldBttn;
    public Camera mainCamera;
    private ParaLensesButtonBehavior paraLenses;

    // Start is called before the first frame update
    void Start()
    {
        paraLenses = this.GetComponent<ParaLensesButtonBehavior>();

    }

    // Update is called once per frame
    void Update()
    {
        // Check if there is at least one touch.
        if (Input.touchCount > 0)
        {
            Debug.Log("TOUCHING SCREEN");
            Touch touch = Input.GetTouch(0);

            // Check if the touch is just beginning.
            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log("TOUCHING Begin");

                // Create a ray from the screen point where the touch occurred.
                Ray ray = mainCamera.ScreenPointToRay(touch.position);

                // Variable to store the hit information.
                RaycastHit hit;

                // Perform the raycast.
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log("Ray Begin");

                    
                    
                    // Call a function on the object hit.
                    Debug.Log("Tapped on " + hit.collider.gameObject.name);
                    hit.collider.gameObject.SendMessage("OnTap", SendMessageOptions.DontRequireReceiver);
                    
                }
            }
        }
        else if (Input.GetMouseButtonDown(0)) // 0 is for left-click
        {
            Debug.Log("TOUCHING Begin");

            // Create a ray from the screen point where the touch occurred.
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // Variable to store the hit information.
            RaycastHit hit;

            // Perform the raycast.
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Ray Begin");

                // Check if the object hit has the tag "Interactable" (optional).
                //if (hit.collider != null && hit.collider.CompareTag("Interactable"))
                //{
                    // Call a function on the object hit.
                    Debug.Log("Tapped on " + hit.collider.gameObject.name);
                    hit.collider.gameObject.SendMessage("OnTap", SendMessageOptions.DontRequireReceiver);
                //
            }
        }

    }
    
}
