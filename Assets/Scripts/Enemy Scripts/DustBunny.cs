using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustBunny : Enemy
{
    void FixedUpdate() {
        enemyMovement();
    }

    void Update() {
        enemyAttack();
    }

    private void enemyMovement() {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // if player is in range
        if(Vector3.Distance(enemyBody.position, playerBody.position) < movementRange) {
            // move enemy towards player
            if (stateInfo.normalizedTime >= 1f){
                animator.SetBool("Moving", true);
                movement = (playerBody.position - enemyBody.position) * movementSpeed;
                enemyBody.MovePosition(enemyBody.position + (movement * Time.fixedDeltaTime));
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
            abilityCounter = 0;
            foreach (Ability ability in abilities) {
                //before checking if an ability can be cast check if the player is in ability range
                if(Vector2.Distance(enemyBody.position, playerBody.position) < ability.abilityRange){
                    float randomNumber = Random.Range(0, 100);
                    if (randomNumber < ability.abilityChance) {
                        useAbility(abilityCounter);
                        actionCooldownTimer = (1 / basicAttackSpeed);
                        break;
                    }
                    abilityCounter++;
                }
            }
        }
        //if ability not used will basic attack
        if(Vector2.Distance(enemyBody.position, playerBody.position) < attackRange){
            attack(); //basic attack
            actionCooldownTimer = (1 / basicAttackSpeed);
        }
    }

    private void useAbility(int abilityNum){
        abilityCooldownTimer = abilities[abilityNum].abilityCooldown;
        //first check what type of ability it is and will do stuff depending on type of ability
        if(abilities[abilityNum].abilityType == "Movement"){
            animator.SetBool("MovementAttack", true);
            checkCollision(abilities[abilityNum].abilityDamage);
            StartCoroutine(waitForAnimation("MovementAttack"));
        }
        //have other ability types as else if statments and we can add simple code to deal damage correctly. 
    }
}
