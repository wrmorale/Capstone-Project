using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float idleMovementRange;
    [SerializeField] private Transform player;
    public Enemy enemyInstance;
    private Rigidbody body;
    private Rigidbody playerBody;
    private Vector3 movement;
    private Vector3 idleMovement;
    private float elapsedTime = 0;
    private bool isIdle = false;

    private void Awake() {
        body = GetComponent<Rigidbody>();
        playerBody = player.GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        // if player is in range
        if(Vector2.Distance(body.position, playerBody.position) < range) {
            // move enemy towards player
            movement = (playerBody.position - body.position) * enemyInstance.movementSpeed;
            body.MovePosition(body.position + (movement * Time.fixedDeltaTime));
        }
        else {
            //enemy idle movement
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 3f) {
                elapsedTime = 0;
                isIdle = !isIdle;
                if (isIdle) {
                    idleMovement = body.position + new Vector3(Random.Range(-idleMovementRange, idleMovementRange), 0, Random.Range(-idleMovementRange, idleMovementRange));
                    movement = (idleMovement - body.position).normalized * enemyInstance.movementSpeed;
                } 
                else {
                    movement = Vector3.zero;
                }
            }
            body.MovePosition(body.position + (movement * Time.fixedDeltaTime));
        }
    }
}
