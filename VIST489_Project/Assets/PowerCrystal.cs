using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCrystal : MonoBehaviour
{
    public SpawnObjects spawner;
    public HealthSystem health;


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
        if(other.gameObject.name == "Sword14_Red")
        {
            spawner.spawnRate += 2;
            health.crystalCount--;

            this.gameObject.SetActive(false);
        }
    }
}
