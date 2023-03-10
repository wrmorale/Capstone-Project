using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothProjectile : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 10f;
    public float lifetime = 5f;

    private void Start()
    {
        // Destroy the projectile after its lifetime has expired
        Destroy(gameObject, lifetime);
    }

    public void Launch(Vector3 direction)
    {
        // Add force to the rigidbody to launch the projectile
        rb.AddForce(direction * speed, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object is the player
        if (other.CompareTag("Player"))
        {
            // Destroy the projectile when it hits the player
            Destroy(gameObject);
        }
    }
}
