using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;
    [SerializeField] int baseDamage = 20;
    [SerializeField] float attackInterval = 3f;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] List<Transform> attackOrigins;
    [SerializeField] Transform player;


    float timer = 0f;
 
    public HealthBar healthBar;
    
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= attackInterval)
        {
            timer = 0f;
            AttackPlayer();
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void AttackPlayer()
    {
        int index = Random.Range(0, attackOrigins.Count);
        Transform origin = attackOrigins[index];
        GameObject projectile = Instantiate(projectilePrefab, origin.position, Quaternion.identity);
        projectile.transform.LookAt(player);
        
        projectile.GetComponent<Projectile>().SetDamage(baseDamage);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(rb.transform.forward * projectileSpeed);
        }
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.SetHealth(currentHealth);
    }
}
