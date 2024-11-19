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
    [SerializeField] GameObject weakPoints;
    [SerializeField] string backgroundMusicName;
    [SerializeField] string beatZoroarkText;
    [SerializeField] GameObject model;

    AudioManager audioManagerScript;
    PokemonWorld pokeWorld;
    MessageBehavior messageBehavior;
    UIBehavior uiBehaviorScript;
    bool encounterStarted;
   




    float timer = 0f;
 
    public HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        weakPoints.SetActive(false);
        encounterStarted = false;
    }

    void Update()
    {
        if (encounterStarted == true)
        {
            timer += Time.deltaTime;
            if (timer >= attackInterval)
            {
                timer = 0f;
                AttackPlayer();
            }
        }
        
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

        if (currentHealth == 0)
        {
            EnemyDefeated();
        }
    }

    public void EnemyDefeated()
    {
        SetEncounterStarted(false);
        model.SetActive(false);
        
        audioManagerScript = AudioManager.instance;
        audioManagerScript.PlayEventSound(backgroundMusicName);

        messageBehavior = MessageBehavior.instance;

        messageBehavior.SetHaveMessage(true);
        messageBehavior.SetMessageText(beatZoroarkText);
        
        pokeWorld = PokemonWorld.instance;

        pokeWorld.SolvedMaze();
        uiBehaviorScript = UIBehavior.instance;
        uiBehaviorScript.SetState(UIBehavior.UiState.RoamState);
    }

    public void ShowWeakPoints()
    {
        weakPoints.SetActive(true);
    }

    public void SetEncounterStarted(bool status)
    {

        encounterStarted = status;
        if (status == false)
        {
            timer = 0f;
        }
    }

    public bool GetEncounterStarted()
    {
        return encounterStarted;
    }
}
