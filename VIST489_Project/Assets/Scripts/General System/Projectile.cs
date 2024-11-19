using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float baseDamage = 10f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            other.GetComponent<Player>().TakeDamage(baseDamage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Destroy Zone") == true)
        {
            Destroy(gameObject);
        }
        
    }

    public void SetDamage(float damage)
    {
        baseDamage = damage;
    }

}
