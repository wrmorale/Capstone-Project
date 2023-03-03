using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Enemy
{
    void Update() {
        enemyAttack();
    }

    private void enemyMovement() {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // if player is in range
        if(Vector3.Distance(enemyBody.position, playerBody.position) < movementRange) {
            // move enemy towards player
            if (stateInfo.normalizedTime >= 1f){
                animator.SetBool("Dash", true);
                movement = (playerBody.position - enemyBody.position) * movementSpeed;
                enemyBody.MovePosition(enemyBody.position + (movement * Time.fixedDeltaTime));
            }
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
                        actionCooldownTimer = ability.abilityCooldown;
                        break;
                    }
                    abilityCounter++;
                }
            }
        }
    }

    private void useAbility(int abilityNum){
        abilityCooldownTimer = abilities[abilityNum].abilityCooldown;
        //first check what type of ability it is and will do stuff depending on type of ability
        if(abilities[abilityNum].abilityType == "Movement"){
            animator.SetBool("Dash", true);
            checkCollision(abilities[abilityNum].abilityDamage);
            StartCoroutine(waitForAnimation("Dash"));
        }
        else if(abilities[abilityNum].abilityType == "Sweep"){
            animator.SetBool("SpinAttack", true);
            checkCollision(abilities[abilityNum].abilityDamage);
            StartCoroutine(waitForAnimation("SpinAttack"));
        }
        else if(abilities[abilityNum].abilityType == "Melee"){
            animator.SetBool("Light1", true);
            checkCollision(abilities[abilityNum].abilityDamage);
            StartCoroutine(waitForAnimation("Light1"));
            if(Vector3.Distance(enemyBody.position, playerBody.position) < movementRange){ //if player still in range finish combo
                animator.SetBool("Light2", true);
                checkCollision(abilities[abilityNum].abilityDamage);
                StartCoroutine(waitForAnimation("Light2"));
            } 
        }
        //have other ability types as else if statments and we can add simple code to deal damage correctly. 
    }
}
