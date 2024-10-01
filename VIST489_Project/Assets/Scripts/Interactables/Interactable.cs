using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for objects that we can interact with
public class Interactable : MonoBehaviour
{
    public float radius = 20f; // How close we have to be to an object to interact with it

    bool isFocus = false;
    bool hasInteracted = false;
    Transform player;

    public virtual void Interact()
    {
        // Method to be overwritten by interactable object types
        Debug.Log("Interacting with " + transform.name);

    }

    void Update()
    {
        if (isFocus == true && hasInteracted == false)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            if (distance <= radius)
            {

                hasInteracted = true;
                Interact();
            }

        }
    }

    public void OnFocused(Transform playerTransform)
    {
        isFocus = true;
        player = playerTransform;
        hasInteracted = false;
    }

    public void OnDefocused()
    {
        isFocus = false;
        player = null;
        hasInteracted = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);

    }
}
