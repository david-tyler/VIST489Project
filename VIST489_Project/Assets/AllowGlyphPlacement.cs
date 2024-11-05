using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowGlyphPlacement : MonoBehaviour
{

    public bool glyphPlaced = false;
    public GameObject stage;
    public GameObject positioner;
    

    void Update()
    {
        
    }

    public void OnTriggerStay(Collider other)
    {
        if (!glyphPlaced)
        {
            stage.SetActive(true);
            positioner.SetActive(true);
        }
        else
        {
            stage.SetActive(false);
            positioner.SetActive(false);
        }


        
    }

    public void OnTriggerExit(Collider other)
    {
        stage.SetActive(false);
        positioner.SetActive(false);
    }


    public void GlyphPlaced()
    {
        glyphPlaced = true;
    }
}
