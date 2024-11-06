using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlyphTrail : MonoBehaviour
{

    public TrailRenderer trail;
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
            Vector3 touchPosition = camera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f));

            if (touch.phase == TouchPhase.Began)
            {
                // Create a new trail instance when the touch begins
                //currentTrail = Instantiate(trailPrefab, touchPosition, Quaternion.identity);
                //lineRenderer = currentTrail.GetComponent<LineRenderer>();
                //lineRenderer.positionCount = 0; // Initialize position count for the trail

                trail.gameObject.transform.position = touchPosition;
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                // Update the trail position as the touch moves
                trail.gameObject.transform.position = touchPosition;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                
            }
        }
        else if (Input.GetMouseButton(0)) // 0 is for left-click
        {

                // Create a ray from the screen point where the touch occurred.
                trail.gameObject.transform.position = camera.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
