using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using States;
using Extensions;

public class Golem : Enemy
{
    private GolemAttackManager attackManager;
    public enum GolemState{
        Idle,
        Attacking
    }
    public GolemState state = GolemState.Idle;

    private void Awake(){
        attackManager = gameObject.GetComponent<GolemAttackManager>();
    }

    void Update() {
        if (enemyCanAttack()){
            enemyAction();
        }
        if (state == GolemState.Attacking){
            attackManager.updateMe(Time.deltaTime);
        }
        else{
            //enemyMovement();
        }
        dashCooldownTimer -= Time.deltaTime;
        actionCooldownTimer -= Time.deltaTime;
        abilityCooldownTimer -= Time.deltaTime;
        if(abilityCooldownTimer < 0) {
            abilityCooldownTimer = 0;
        }
    }

    private void enemyMovement() {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // if player is in range
        if(playerInRange(movementRange)) {
            // move enemy towards player
            if (stateInfo.normalizedTime >= 1f){
                animator.SetBool("Dash", true);
                movement = (playerBody.position - enemyBody.position) * movementSpeed;
                enemyBody.MovePosition(enemyBody.position + (movement * Time.fixedDeltaTime));
            }
        }
    }

    public bool enemyCanAttack(){
        //if able to attack, the enemy does so
        //first checks to see if enemy is in range for an attack
        if(playerInRange(longestAttackRange) && canAttack()) {
            return true;
        }
        return false;
    }

    public void enemyAction(){
        //first checks if it can dash to player
        if (playerInRange(abilities[3].abilityRange) && dashCooldownTimer <= 0){
            dashCooldownTimer = abilities[3].abilityCooldown;
            state = GolemState.Attacking;
            attackManager.handleAttacks(abilities[3]);
            //actionCooldownTimer = ability.abilityCooldown;
        }

        /*
        if(abilityCooldownTimer == 0){
            abilityCounter = 0;
            foreach (Ability ability in abilities) {
                
                //before checking if an ability can be cast check if the player is in ability range
                if(playerInRange(ability.abilityRange)){
                    
                    float randomNumber = Random.Range(0, 100);
                    if (randomNumber < ability.abilityChance) {
                        state = GolemState.Attacking;
                        attackManager.handleAttacks(ability);
                        actionCooldownTimer = ability.abilityCooldown;
                        break;
                    }
                    abilityCounter++;
                }
            }
        }*/
    }
}