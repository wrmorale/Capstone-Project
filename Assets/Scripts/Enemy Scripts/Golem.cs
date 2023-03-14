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
        if (movement != Vector3.zero) {
            enemyBody.rotation = Quaternion.LookRotation(movement);
        }
        if (state == GolemState.Idle){
            enemyAction();
        }
        if (state == GolemState.Attacking){
            attackManager.updateMe(Time.deltaTime);
        }
        dashCooldownTimer -= Time.deltaTime;
        actionCooldownTimer -= Time.deltaTime;
        abilityCooldownTimer -= Time.deltaTime;
        if(abilityCooldownTimer < 0) {
            abilityCooldownTimer = 0;
        }
    }

    void FixedUpdate() {
        if (movement != Vector3.zero) {
            movement = playerBody.position - enemyBody.position;
            //lookAtPos.y = enemyBody.transform.position.y; // do not rotate the player around x
            Quaternion newRotation = Quaternion.LookRotation(movement, enemyBody.transform.up);
            enemyBody.transform.rotation = Quaternion.Slerp(enemyBody.transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
        }
        if (isDashing) {
            // move enemy towards player during dash animation
            directionToPlayer = playerBody.position - enemyBody.position;
            movement = directionToPlayer.normalized * (movementSpeed * 5) * Time.fixedDeltaTime;
            enemyBody.MovePosition(enemyBody.position + movement);
        }
        if (state != GolemState.Attacking){
            enemyMovement();
        }
    }

    private void enemyMovement() {
        directionToPlayer = playerBody.position - enemyBody.position;
        // if player is in range
        if(playerInRange(movementRange)) {
            // move enemy towards player
            movement = directionToPlayer.normalized * movementSpeed * Time.fixedDeltaTime;
            enemyBody.MovePosition(enemyBody.position + movement);
        }
    }

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
        } //this is for spin
        else if (distanceToPlayer <= abilities[0].abilityRange && randomNumber < abilities[0].abilityChance && actionCooldownTimer <= 0){
            state = GolemState.Attacking;
            attackManager.handleAttacks(abilities[0]);
            actionCooldownTimer = abilities[0].abilityCooldown;
        } //light 1
        else if (distanceToPlayer <= abilities[1].abilityRange && actionCooldownTimer <= 0){
            state = GolemState.Attacking;
            attackManager.handleAttacks(abilities[1]);
            actionCooldownTimer = abilities[1].abilityCooldown;
        } //light 2 if light 1 went before
        if (light1Complete && distanceToPlayer <= abilities[2].abilityRange){
            state = GolemState.Attacking;
            actionCooldownTimer = abilities[2].abilityCooldown;
            attackManager.handleAttacks(abilities[2]);
        }
    }
}