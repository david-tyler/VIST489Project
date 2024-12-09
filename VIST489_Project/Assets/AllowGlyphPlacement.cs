using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowGlyphPlacement : MonoBehaviour
{

    public bool glyphPlaced = false;
    public GameObject stage;
    public GameObject positioner;
    public TrailRenderer trail;
    

    void Update()
    {
        
    }

    public void OnTriggerStay(Collider other)
    {
        trail.gameObject.SetActive(true);
        if (!glyphPlaced)
        {
            stage.SetActive(true);
            positioner.SetActive(true);
        }
        else
        {
            stage.SetActive(false);
            positioner.SetActive(false);
            trail.gameObject.SetActive(true);
        }


        
    }

    public void OnTriggerExit(Collider other)
    {
        stage.SetActive(false);
        positioner.SetActive(false);
        trail.gameObject.SetActive(false);
    }


    public void GlyphPlaced()
    {
        glyphPlaced = true;
    }
}
