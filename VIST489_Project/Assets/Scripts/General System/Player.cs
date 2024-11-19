using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;
    [SerializeField] int baseDamage = 10;
    [SerializeField] float damageMultiplier = 2.5f;
    [SerializeField] Camera cam;

    public string[] moveset = new string[4];
    private const int movesetCapacity = 4;
    private int knownMoves = 1;

    public HealthBar healthBar;
    private bool canAttack = false;
    private bool hitWeakPoint = false;

    Enemy currentEnemy;
    GameObject currentWeakPoint;

    public float cooldownDuration = 2f; // Cooldown duration in seconds

    private bool isCooldown = false;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        for (int i = 1; i < movesetCapacity; i++)
        {
            moveset[i] = "";
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Enemy") == true)
            {
                currentEnemy = hit.collider.GetComponent<Enemy>();
                canAttack = true;
                hitWeakPoint = false;
            }
            else if (hit.collider.CompareTag("Weak Point") == true)
            {
                canAttack = true;
                currentEnemy = hit.collider.GetComponent<WeakPoint>().enemyObject;
                damageMultiplier = hit.collider.GetComponent<WeakPoint>().multiplier;
                currentWeakPoint = hit.collider.gameObject;

                hitWeakPoint = true;
            }
            else
            {
                currentEnemy = null;
                canAttack = false;
                hitWeakPoint = false;
            }
            
        }
        else
        {
            canAttack = false;
            hitWeakPoint = false;
        }

        if (isCooldown == true)
        {
            StartCoroutine(StartCooldown());
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.SetHealth(currentHealth);
    }

    public void Attack(int index)
    {
        if (index < 0 || index > 3)
        {
            Debug.LogError("Invalid index for player attack!");
            return;
        }
        
        if (index + 1 <= knownMoves)
        {
            if (canAttack == true)
            {

                Debug.Log("Attacking Enemy");
                PerformAttack(currentEnemy);
            }
            else
            {
                Debug.Log("Cannot Attack");
            }
        }
        else
        {
            Debug.Log("Haven't learned enough moves.");
        }

    }
    void PerformAttack(Enemy enemy)
    {
        if (isCooldown == false)
        {
            float playerDamage = baseDamage;
            if (hitWeakPoint)
            {
                playerDamage *= damageMultiplier;
                if (currentWeakPoint != null)
                {
                    currentWeakPoint.SetActive(false);
                }
            }
            enemy.TakeDamage(playerDamage);
            isCooldown = true;
        }
        
        
    }

    private IEnumerator StartCooldown()
    {

        yield return new WaitForSeconds(cooldownDuration);

        isCooldown = false;
    }
}
