using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugViewer : MonoBehaviour
{
    public List<Renderer> modelsToToggle = new List<Renderer>();
    public List<GameObject> UIToToggle = new List<GameObject>();
    public bool debugOn = false;

    private void Start()
    {
        for (int i = 0; i < modelsToToggle.Count; i++)
        {
            modelsToToggle[i].enabled = false;
        }
    }

    public void ToggleModels()
    {
        if(debugOn == true)
        {
            for(int i = 0; i< modelsToToggle.Count; i++)
            {
                modelsToToggle[i].enabled = false;
            }

            debugOn = false;
        }
        else
        {
            for (int i = 0; i < modelsToToggle.Count; i++)
            {
                modelsToToggle[i].enabled = true;
            }

            debugOn = true;
        }
    }

    public void ToggleUI()
    {
        if (debugOn == true)
        {
            for (int i = 0; i < UIToToggle.Count; i++)
            {
                UIToToggle[i].SetActive(false);
            }

            debugOn = false;
        }
        else
        {
            for (int i = 0; i < modelsToToggle.Count; i++)
            {
                UIToToggle[i].SetActive(true);
            }

            debugOn = true;
        }
    }
}
