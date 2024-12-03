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
    [SerializeField] AudioClip backgroundMusicName;
    [SerializeField] string beatZoroarkText;
    [SerializeField] GameObject model;

    AudioManager audioManagerScript;
    PokemonWorld pokeWorld;
    MessageBehavior messageBehavior;
    UIBehavior uiBehaviorScript;
    GameSystemBehavior gameSystem;
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
                AttackPlayer(player);
            }
        }
        
        
    }
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void AttackPlayer(Transform currentPlayerTransform)
    {
        int index = Random.Range(0, attackOrigins.Count);
        Transform origin = attackOrigins[index];

        GameObject projectile = Instantiate(projectilePrefab, origin.position, Quaternion.identity);
        Vector3 direction = (currentPlayerTransform.position - origin.position).normalized;
        
        // Rotate the projectile to face the player
        //projectile.transform.rotation = Quaternion.LookRotation(direction);
        
        // projectile.transform.LookAt(currentPlayerTransform);
        
        // Vector3 projectileRotation = projectile.transform.localEulerAngles;
        // projectile.transform.localEulerAngles = new Vector3(projectileRotation.x, projectileRotation.y, projectileRotation.z + 90);
        projectile.GetComponent<Rigidbody>().freezeRotation = true;
       

        projectile.GetComponent<Projectile>().SetDamage(baseDamage);
        
        // projectile.GetComponent<Projectile>().SetTarget(currentPlayerTransform.position, projectileSpeed);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }

    }

    public void TakeDamage(float damage)
    {
         gameSystem = GameSystemBehavior.instance;
        GameSystemBehavior.NarrativeEvent currentState = gameSystem.GetCurrentState();
        currentHealth -= damage;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.SetHealth(currentHealth);

        if (currentHealth == 0)
        {
            switch (currentState)
            {
                case GameSystemBehavior.NarrativeEvent.ZoroarkBattle:
                    StartCoroutine(ZoroarkDefeated());
                    break;
                
            }
        }
    }

    IEnumerator ZoroarkDefeated()
    {
        yield return new WaitForSeconds(5f);
        
        SetEncounterStarted(false);
        model.SetActive(false);
        
        audioManagerScript = AudioManager.instance;
        audioManagerScript.PlayEventSound(audioManagerScript.GetBackgroundMusic().name);

        messageBehavior = MessageBehavior.instance;

        messageBehavior.SetHaveMessage(true);
        messageBehavior.SetMessageText(beatZoroarkText);
        
        pokeWorld = PokemonWorld.instance;

        pokeWorld.SolvedMaze();
        uiBehaviorScript = UIBehavior.instance;
        uiBehaviorScript.SetState(UIBehavior.UiState.RoamState);
        ResetHealth();

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
