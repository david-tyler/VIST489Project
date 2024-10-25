using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{

    public ParticleSystem particles;
    public GameObject objectToActivate;
    void OnTap()
    {
        // Action to take when the object is tapped.
        Debug.Log("Object tapped: " + gameObject.name);
        // Example: change color

        if(particles != null)
        {
            particles.Play();
        }
        if(objectToActivate != null)
        {
            objectToActivate.SetActive(true);
            Debug.Log("turn on");
        }
    }
}


