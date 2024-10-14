using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCheck : MonoBehaviour
{
    public bool stepHit = false;
    public GameObject pit;
    public Transform fallPosition;
    public float duration = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("pit triggered");
        Debug.Log(other.gameObject.name);
        if(other.gameObject.name == "Player")
        {
            Debug.Log("Player Detected");
            if (stepHit == false)
            {
                //fall
                Debug.Log("Fall now");
                StartCoroutine(FallIntoPit());
            }
            else
            {
                //Do Nothing
            }
        }
    }

    public IEnumerator FallIntoPit()
    {
        float elapsedTime = 0f;

        Vector3 startPosition = pit.transform.position;

        Debug.Log("falling");

        while (elapsedTime < duration)
        {
            // Calculate how far along the duration we are
            elapsedTime += Time.deltaTime;

            // Interpolate between start and target positions
            pit.transform.position = Vector3.Lerp(startPosition, fallPosition.position, elapsedTime / duration);
            Debug.Log(Vector3.Lerp(startPosition, fallPosition.position, elapsedTime / duration));
            // Wait for the next frame before continuing
            yield return new WaitForEndOfFrame();
        }

        // Ensure the object is exactly at the target position at the end
        transform.position = fallPosition.position;

    }

}
