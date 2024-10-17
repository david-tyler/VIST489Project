using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCheck : MonoBehaviour
{
    public bool stepHit = false;
    public GameObject pit;
    public Transform fallPosition;
    public float duration = 2.0f;
    public bool onStep = false;
    public float stepTimer = 2.0f;
    public float resetValue = 2.0f;
    public bool touchingPit = false;

    public void Update()
    {
        if(touchingPit)
        {
            stepTimer -= Time.deltaTime;

            if(stepTimer <= 0)
            {
                StartCoroutine(FallIntoPit());
                stepTimer = 10000f;
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {
        Debug.Log("pit triggered");
        Debug.Log(other.gameObject.name);
        if(other.gameObject.name == "Player")
        {
            touchingPit = true;
            
            /*
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
            */
        }
    }

    private void OnTriggerExit(Collider other)
    {
        touchingPit = false;
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
