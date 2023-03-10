using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using States;
using Extensions;

public class Golem : Enemy
{
    private GolemAttackManager attackManager;
    private Vector3 directionToPlayer;
    private float distanceToPlayer;
    public bool isDashing;
    public bool light1Complete;

    public enum GolemState{
        Idle,
        Attacking
    }
    public GolemState state = GolemState.Idle;

    private void Awake(){
        isDashing = false;
        light1Complete = false;
        attackManager = gameObject.GetComponent<GolemAttackManager>();
    }

    void Update() {
        if (state == GolemState.Idle){
            enemyAction();
        }
        if (state == GolemState.Attacking){
            attackManager.updateMe(Time.deltaTime);
        }
        else{
            //isDashing = false;
            //enemyMovement();
        }
        dashCooldownTimer -= Time.deltaTime;
        actionCooldownTimer -= Time.deltaTime;
        abilityCooldownTimer -= Time.deltaTime;
        if(abilityCooldownTimer < 0) {
            abilityCooldownTimer = 0;
        }
    }

    void FixedUpdate() {
        if (isDashing) {
            // move enemy towards player during dash animation
            directionToPlayer = playerBody.position - enemyBody.position;
            movement = directionToPlayer.normalized * (movementSpeed * 5) * Time.fixedDeltaTime;
            enemyBody.MovePosition(enemyBody.position + movement);
        }
    }

    private void enemyMovement() {
        directionToPlayer = playerBody.position - enemyBody.position;
        distanceToPlayer = directionToPlayer.magnitude;
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // if player is in range
        if(playerInRange(movementRange)) {
            // move enemy towards player
            if (stateInfo.normalizedTime >= 1f){
                //animator.SetBool("Dash", true);
                movement = (playerBody.position - enemyBody.position) * movementSpeed;
                enemyBody.MovePosition(enemyBody.position + (movement * Time.fixedDeltaTime));
            }
        }
    }

    /*public bool enemyCanAttack(){
        //if able to attack, the enemy does so
        //first checks to see if enemy is in range for an attack
        if(playerInRange(longestAttackRange) && canAttack()) {
            return true;
        }
        return false;
    }*/

    public void enemyAction(){
        directionToPlayer = playerBody.position - enemyBody.position;
        distanceToPlayer = directionToPlayer.magnitude;
        float randomNumber = Random.Range(0, 100);
        //first checks if it can dash to player
        if (distanceToPlayer <= abilities[3].abilityRange && dashCooldownTimer <= 0){
            state = GolemState.Attacking;
            isDashing = true;
            attackManager.handleAttacks(abilities[3]);
            dashCooldownTimer = abilities[3].abilityCooldown;
            actionCooldownTimer = abilities[3].abilityCooldown;
        }
        else if (distanceToPlayer <= abilities[0].abilityRange && randomNumber < abilities[0].abilityChance){
            state = GolemState.Attacking;
            attackManager.handleAttacks(abilities[0]);
            actionCooldownTimer = abilities[0].abilityCooldown;
        }
        else if (distanceToPlayer <= abilities[1].abilityRange){
            state = GolemState.Attacking;
            attackManager.handleAttacks(abilities[1]);
            actionCooldownTimer = abilities[1].abilityCooldown;
        }
        if (light1Complete && distanceToPlayer <= abilities[2].abilityRange){
            state = GolemState.Attacking;
            attackManager.handleAttacks(abilities[2]);
            actionCooldownTimer = abilities[2].abilityCooldown;
        }
    }
}