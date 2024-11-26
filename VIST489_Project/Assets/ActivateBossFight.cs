using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBossFight : MonoBehaviour
{
    public List<GameObject> bossFightobjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        for(int i = 0;i< bossFightobjects.Count;i++)
        {
            bossFightobjects[i].SetActive(true);
        }
    }
}
