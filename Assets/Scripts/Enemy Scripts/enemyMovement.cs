using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private Transform player;
    private Rigidbody body;
    private Rigidbody playerBody;
    private Vector3 movement;

    private void Awake() {
        body = GetComponent<Rigidbody>();
        playerBody = player.GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        // if player is in range
        if(Vector2.Distance(body.position, playerBody.position) < range) {
            // move enemy towards player
            movement = (playerBody.position - body.position) * speed;
            body.MovePosition(body.position + (movement * Time.fixedDeltaTime));
        }
        else {
            //enemy is idle
            movement = Vector2.zero;
        }
    }
}
