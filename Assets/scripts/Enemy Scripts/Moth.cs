using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moth : Enemy
{
    public float cooldownTime = 5f;
    public float cooldownRemaining = 0f;
    public float fleeDuration = 1f;
    public float fleeRemaining = 0f;
    
    void FixedUpdate()
    {
        enemyMovement();
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
        }
    }

    void Attack(){

    }

}
