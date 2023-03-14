using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DamageFlash;
using States;
using Extensions;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]public string enemyName;
    [SerializeField]public float maxHealth;
    [SerializeField]public float health;
    [SerializeField]public float basicAttackDamage;
    [SerializeField]public float basicAttackSpeed;
    [SerializeField]public float attackRange;
    [SerializeField]public float movementSpeed;
    [SerializeField]public float movementRange;
    [SerializeField]public float idleMovementRange;
    [SerializeField]public float staggerLimit;
    public List<Ability> abilities; 

    [Header("Attacks info")]
    //public float basicAttackCooldownTimer = 0;
    public float abilityCooldownTimer = 0;
    public float dashCooldownTimer = 0;
    public float actionCooldownTimer = 0;
    public int abilityCounter = 0;
    public float longestAttackRange = 0;
    public bool isAttacking = false;

    [Header("Movement info")]
    public Vector3 movement;
    public Vector3 idleMovement;
    public float rotationSpeed;
    public float elapsedTime = 0;
    public bool isIdle = false;

    [Header("Collider + Physics info")]
    public GameObject enemyInstance;
    public GameObject bodyCollider;
    public Rigidbody enemyBody;
    public Rigidbody playerBody;
    public Transform player;

    [Header("Animator info")]
    public Animator animator;
    public AnimatorStateInfo stateInfo;
    public bool animationFinished = true;

    [Header("Dust Piles info")]
    public float detectionRange = 5.0f;
    public float healingSpeed = 0.1f;
    public GameObject dustPilePrefab;
    public int maxDustPiles = 5;
    private List<DustPile> dustPiles = new List<DustPile>();

    public GameObject damageFlashPrefab;
    public DamageFlash damageFlash;

    private EnemyHealthBar enemyHealthBar;
    private float HealthPercent = 1;
    
    GameObject damageFlashObject;
    void Start(){
        enemyBody = GetComponent<Rigidbody>();
        playerBody = player.GetComponent<Rigidbody>();
        animator = gameObject.GetComponentInChildren<Animator>();
        enemyHealthBar  = GetComponentInChildren<EnemyHealthBar>();
        enemyHealthBar.setMaxHealth(HealthPercent);
        health = maxHealth;
        damageFlashObject = Instantiate(damageFlashPrefab, transform.position, Quaternion.identity);
        damageFlash = damageFlashObject.GetComponent<DamageFlash>();
        //this just gets longest range to see when the enemy can start to cast abilities or attacking the player
        foreach (Ability ability in abilities) {
            if(longestAttackRange < ability.abilityRange){
                longestAttackRange = ability.abilityRange;
            }
        }
        if(longestAttackRange < attackRange){
            longestAttackRange = attackRange;
        }
    }

    void Update(){
        // Check for nearby dust piles that need healing
        foreach (DustPile dustPile in dustPiles) {
            if (dustPile.health < dustPile.maxHealth) {
                float distance = Vector3.Distance(transform.position, dustPile.transform.position);
                if (distance <= detectionRange) {
                    dustPile.IncreaseHealth(healingSpeed * Time.deltaTime);
                }
            }
        }

        // Check if we need to generate a new dust pile
        if (dustPiles.Count < maxDustPiles) {
            GameObject newDustPile = Instantiate(dustPilePrefab, transform.position + new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)), Quaternion.identity);
            DustPile newDustPileScript = newDustPile.GetComponent<DustPile>();
            newDustPileScript.SetHealth(0.1f); // set a low starting health
            dustPiles.Add(newDustPileScript);
        }
    }

    public void isHit(float damage){
        health -= damage;
        //damageFlash.FlashStart();
        health = Mathf.Clamp(health, 0, maxHealth);
        HealthPercent = health / maxHealth;
        enemyHealthBar.setHealth(HealthPercent);
        if(health <= 0){
            // Destroy the cube when it has no health left
            //this should work for death animation but not all enemies have one so it gets errors
            animator.SetBool("Death", true);
            StartCoroutine(waitForAnimation("Death"));
            Destroy(gameObject);
        }
    }
    
    public bool playerInRange(float maxRange){
        //checks to see if enemy is in range for an attack or movement
        if(Vector2.Distance(enemyBody.position, playerBody.position) < maxRange) {
            return true;
        }
        return false;
    }

    public bool canAttack(){
        if(actionCooldownTimer < 0) {
            return true;
        }
        return false;
    }

    public void attack(){
        //play basic attack animation of this enemy - for now something generic for our enemy (makes enemy jump)
        //the attack animation should be checking for collisions so it should do damage that way. 
        //checkCollision(basicAttackDamage); //temp damage dealing
    }

    public void checkCollision(float damage){ //for now just checks for collisions to deal damage. Will probably change once hitboxes and animations are in for enemies
        //checks to see if "attack" collides with player
        Collider[] collidersFF = Physics.OverlapSphere(enemyBody.position, attackRange);
        foreach (Collider collider in collidersFF) {
            if (collider.tag == "Player") {
                // apply damage to player
                //print("Collide");
                Player player = collider.GetComponent<Player>();
                player.isHit(damage);
            }
        }
    }

    public IEnumerator waitForAnimation(string animationName) {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        animator.SetBool(animationName, false);
        animationFinished = true;
    }
}
