using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitPlatform : MonoBehaviour
{
    public FallingCheck fallingCheck;

    private void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.name == "Player")
        {
            fallingCheck.stepHit = true;
        }
    }
}
