using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moth : Enemy
{
    public float cooldownTime = 5f;
    public float cooldownRemaining = 0f;
    public float fleeDuration = 1f;
    public float fleeRemaining = 0f;

    [Header("Projectile Stats")]
    public Projectile projectilePrefab;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileLifetime;
    [SerializeField] private float projectileDamage;
    public Transform bulletSpawn;

    void FixedUpdate()
    {
        enemyMovement();
    }

    void Update(){
        enemyAttack();
    }

    public void enemyMovement() {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // if player is in range
        if(Vector3.Distance(enemyBody.position, playerBody.position) < movementRange) {
            if(cooldownRemaining <= 0f) {
                // Move away from the player for a short duration
                if(fleeRemaining > 0f) {
                    animator.SetBool("Moving", true);
                    movement = (enemyBody.position - playerBody.position).normalized * movementSpeed;
                    enemyBody.MovePosition(enemyBody.position + (movement * Time.fixedDeltaTime));
                    fleeRemaining -= Time.fixedDeltaTime;
                }
                else {
                    fleeRemaining = fleeDuration;
                    animator.SetBool("Moving", false);
                    cooldownRemaining = cooldownTime;
                }
            }
            else {
                animator.SetBool("Moving", false);
                cooldownRemaining -= Time.fixedDeltaTime;
            }
        }
        else {
            //enemy idle movement
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 3f) {
                elapsedTime = 0;
                isIdle = !isIdle;
                if (isIdle) {
                    idleMovement = enemyBody.position + new Vector3(Random.Range(-idleMovementRange, idleMovementRange), 0, Random.Range(-idleMovementRange, idleMovementRange));
                    movement = (idleMovement - enemyBody.position).normalized * movementSpeed;
                    animator.SetBool("Moving", true);
                } 
                else {
                    movement = Vector3.zero;
                    animator.SetBool("Moving", false);
                }
            }
            enemyBody.MovePosition(enemyBody.position + (movement * Time.fixedDeltaTime));
        }
        if (movement != Vector3.zero) {
            enemyBody.rotation = Quaternion.LookRotation(movement);
        } else {
            enemyBody.transform.LookAt(playerBody.transform);
        }
    }

    public void enemyAttack(){
        //if able to attack, the enemy does so
        //first checks to see if enemy is in range for an attack
        if(Vector2.Distance(enemyBody.position, playerBody.position) < longestAttackRange && actionCooldownTimer <= 0) {
            enemyAction();
        }
        actionCooldownTimer -= Time.deltaTime;
        abilityCooldownTimer -= Time.deltaTime;
        if(abilityCooldownTimer < 0) {
            abilityCooldownTimer = 0;
        }
    }

    public void enemyAction(){
        //if off ability cooldown can use ability depending on chance to use that ability
        if(abilityCooldownTimer == 0){
            //before checking if an ability can be cast check if the player is in ability range
            if(Vector2.Distance(enemyBody.position, playerBody.position) < abilities[0].abilityRange){
                float randomNumber = Random.Range(0, 100);
                if (randomNumber < abilities[0].abilityChance) {
                    useAbility();
                    actionCooldownTimer = abilities[0].abilityCooldown;
                }
            }
        }
    }

    private void useAbility(){
        abilityCooldownTimer = abilities[0].abilityCooldown;
        //first check what type of ability it is and will do stuff depending on type of ability
        if(abilities[0].abilityType == "Ranged"){
            animator.SetBool("RangedAbility", true);
            LaunchProjectile();
            StartCoroutine(waitForAnimation("RangedAbility"));
        }
    }

    public void SpawnProjectile(Vector3 heading)
    {
        Projectile clone = Instantiate(projectilePrefab, bulletSpawn.position, Quaternion.LookRotation(heading));
        clone.gameObject.SetActive(true);
        clone.Initialize(projectileSpeed, projectileLifetime, projectileDamage, 1f, heading);
    }

    public void LaunchProjectile()
    {
        Vector3 direction = (playerBody.position - transform.position).normalized;
        SpawnProjectile(direction);
    }
}
