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

    private void Start() {
        enemyInstance.body = GetComponent<Rigidbody>();
        enemyInstance.playerBody = enemyInstance.player.GetComponent<Rigidbody>();
        enemyInstance.animator = gameObject.GetComponentInChildren<Animator>();
    }

    private void FixedUpdate() {
        enemyInstance.stateInfo = enemyInstance.animator.GetCurrentAnimatorStateInfo(0);
        // if player is in range
        if(Vector3.Distance(enemyInstance.body.position, enemyInstance.playerBody.position) < enemyInstance.movementRange) {
            // move enemy towards player
            if (enemyInstance.stateInfo.normalizedTime >= 1f){
                enemyInstance.animator.SetBool("Moving", true);
                movement = (enemyInstance.playerBody.position - enemyInstance.body.position) * enemyInstance.movementSpeed;
                enemyInstance.body.MovePosition(enemyInstance.body.position + (movement * Time.fixedDeltaTime));
            }
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
                    enemyInstance.animator.SetBool("Moving", true);
                } 
                else {
                    movement = Vector3.zero;
                    enemyInstance.animator.SetBool("Moving", false);
                }
            }
            enemyInstance.body.MovePosition(enemyInstance.body.position + (movement * Time.fixedDeltaTime));
        }
        if (movement != Vector3.zero) {
            enemyInstance.body.rotation = Quaternion.LookRotation(movement); //not sure why but bunny looks at opposite direction w/o -movement
        }
    }
}
