using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowFinger : MonoBehaviour
{
    public TrailRenderer renderer;
    public Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Get the screen position of the mouse click
            Vector3 screenPosition = Input.mousePosition;

            // Convert the screen position to world position
            // Set the z coordinate to the distance from the camera to the point in the world
            Vector3 worldPosition = camera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 10f));

            // Log the world position to the console (or use it for other actions)
            Debug.Log("World Position: " + worldPosition);

            // Example: Move an object to this position
            renderer.transform.position = worldPosition;

        }
        else if (Input.GetMouseButton(0))
        {
           

            // Get the screen position of the mouse click
            Vector3 screenPosition = Input.mousePosition;

            // Convert the screen position to world position
            // Set the z coordinate to the distance from the camera to the point in the world
            Vector3 worldPosition = camera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 10f));

            // Log the world position to the console (or use it for other actions)
            //Debug.Log("World Position: " + worldPosition);

            // Example: Move an object to this position
            renderer.transform.position = worldPosition;

        }
    }
}
