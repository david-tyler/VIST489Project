using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float baseDamage = 10f;

    private Vector3 target;
    private float speed;
    private bool haveTarget = false;

    void Update()
    {
        if (haveTarget)
        {
            //transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
        }
    }

    public void SetTarget(Vector2 newTarget, float projectileSpeed)
    {
        target = newTarget;
        speed = projectileSpeed;
        haveTarget = true;
    }
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
