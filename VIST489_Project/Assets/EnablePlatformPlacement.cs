using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablePlatformPlacement : MonoBehaviour
{
    public GameObject planeFinder;
    public List<GameObject> objects = new List<GameObject>();


    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if(other.gameObject.name == "Player")
        {
            Debug.Log("finder on");
            planeFinder.SetActive(true);
            for(int i =0;i < objects.Count; i++)
            {
                objects[i].SetActive(true);
            }
        }
        else
        {
            Debug.Log("finder off");
            planeFinder.SetActive(false);
        }
    }
}
