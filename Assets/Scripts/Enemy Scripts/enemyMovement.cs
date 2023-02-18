using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    [SerializeField] private float idleMovementRange;
    public Enemy enemyInstance;
    private Vector3 movement;
    private Vector3 idleMovement;
    private float elapsedTime = 0;
    private bool isIdle = false;

    private void Awake() {
        enemyInstance.body = GetComponent<Rigidbody>();
        enemyInstance.playerBody = enemyInstance.player.GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        // if player is in range
        if(Vector3.Distance(enemyInstance.body.position, enemyInstance.playerBody.position) < enemyInstance.movementRange) {
            // move enemy towards player
            movement = (enemyInstance.playerBody.position - enemyInstance.body.position) * enemyInstance.movementSpeed;
            enemyInstance.body.MovePosition(enemyInstance.body.position + (movement * Time.fixedDeltaTime));
        }
        else {
            //enemy idle movement
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 3f) {
                elapsedTime = 0;
                isIdle = !isIdle;
                if (isIdle) {
                    idleMovement = enemyInstance.body.position + new Vector3(Random.Range(-idleMovementRange, idleMovementRange), 0, Random.Range(-idleMovementRange, idleMovementRange));
                    movement = (idleMovement - enemyInstance.body.position).normalized * enemyInstance.movementSpeed;
                    
                } 
                else {
                    movement = Vector3.zero;
                }
            }
            enemyInstance.body.MovePosition(enemyInstance.body.position + (movement * Time.fixedDeltaTime));
        }
        if (movement != Vector3.zero) {
            enemyInstance.body.rotation = Quaternion.LookRotation(movement); //not sure why but bunny looks at opposite direction w/o -movement
        }
    }

    public void abilityMovement(){
        movement = (enemyInstance.playerBody.position - enemyInstance.body.position) * enemyInstance.movementSpeed * 3; //*3 so it will move faster like if it was a dash
        enemyInstance.body.rotation = Quaternion.LookRotation(-movement);
        enemyInstance.body.MovePosition(enemyInstance.body.position + (movement * Time.fixedDeltaTime));
    }
}
