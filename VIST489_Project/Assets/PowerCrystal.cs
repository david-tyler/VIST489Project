using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCrystal : MonoBehaviour
{
    public SpawnObjects spawner;
    public HealthSystem health;
    public AudioSource source;
    public AudioClip victoryClip;
   

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
            source.Play();

            this.gameObject.SetActive(false);

            if (health.crystalCount <= 0)
            {
                //Play Victory Sound
                StartCoroutine(VictorySound());
                spawner.gameObject.SetActive(false);
            }
        }
    }

    public IEnumerator VictorySound()
    {
        yield return new WaitForSeconds(2.0f);

        source.PlayOneShot(victoryClip);

        yield return null;
    }
}
