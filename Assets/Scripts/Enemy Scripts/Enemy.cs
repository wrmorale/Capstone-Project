using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DamageFlash;

public class Enemy : MonoBehaviour
{
    [Header("stats")]
    [SerializeField]public string enemyName;
    [SerializeField]public double maxHealth;
    [SerializeField]public double health;
    [SerializeField]public float basicAttackDamage;
    [SerializeField]public float basicAttackSpeed;
    [SerializeField]public float attackRange;
    [SerializeField]public float movementSpeed;
    [SerializeField]public float movementRange;
    [SerializeField]public Transform player;
    public Rigidbody body;
    public Rigidbody playerBody;
    public List<Ability> abilities; 
    public Transform platform;
    public float fallLimit = -10; 

    public enemyMovement movement;
    public enemyAttacks attacks;

    public Animator animator;
    public AnimatorStateInfo stateInfo;
    public bool animationFinished = true;

    public float detectionRange = 5.0f;
    public float healingSpeed = 0.1f;
    public GameObject dustPilePrefab;
    public int maxDustPiles = 5;
    private List<DustPile> dustPiles = new List<DustPile>();

    public GameObject damageFlashPrefab;
    public DamageFlash damageFlash;
    
    GameObject damageFlashObject;
    void Start(){
        health = maxHealth;
        GameObject damageFlashObject = Instantiate(damageFlashPrefab, transform.position, Quaternion.identity);
        damageFlash = damageFlashObject.GetComponent<DamageFlash>();
    }

    void Update(){
        if (transform.position.y < platform.position.y + fallLimit){
            Destroy(gameObject);
        }

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
        print("EnemyTookDamage");
        health -= damage;
        damageFlash.FlashStart();
        if(health <= 0){
            // Destroy the cube when it has no health left
            Destroy(gameObject);
            Destroy(damageFlashPrefab);
            Destroy(damageFlashObject);
        }
    }

    public IEnumerator waitForAnimation(string animationName) {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        animator.SetBool(animationName, false);
        animationFinished = true;
    }
}
