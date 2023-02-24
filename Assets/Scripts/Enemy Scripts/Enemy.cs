using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    
    void Start(){
        health = maxHealth;
    }

    void Update(){
        if (transform.position.y < platform.position.y + fallLimit){
            Destroy(gameObject);
        }
    }

    public void isHit(float damage){
        print("EnemyTookDamage");
        health -= damage;
        if(health <= 0){
            // Destroy the cube when it has no health left
            Destroy(gameObject);
        }
    }

    public IEnumerator waitForAnimation(string animationName) {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        animator.SetBool(animationName, false);
        animationFinished = true;
    }
}
