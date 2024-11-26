using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public HealthSystem healthSystem;
    public AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        healthSystem = GameObject.Find("UI Canvas").GetComponent<HealthSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        healthSystem.health--;
        if(other.gameObject.name == "Player")
        {
            source.Play();
        }
        
    }
}
