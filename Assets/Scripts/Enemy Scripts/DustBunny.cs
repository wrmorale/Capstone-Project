using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustBunny : Enemy
{
    void FixedUpdate()
    {
        enemyMovement();
    }

    public void enemyMovement() {
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

    void Attack(){

    }

}
